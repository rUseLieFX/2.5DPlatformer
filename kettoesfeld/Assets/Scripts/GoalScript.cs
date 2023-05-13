using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalScript : MonoBehaviour
{
    bool loaded = false;

    //Hanyadik pályára kaphat engedélyt, ha ez elkészült? 
    [Header("Jelenlegi pálya adatai")]
    [SerializeField] GameData.Episodes episode;
    [SerializeField] int hanyadik;

    private void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name; //pl e1_m1
        string[] split = sceneName.Split('_'); // ebbõl lesz "e1" és "m1"
        if (split.Length != 2) 
        {
            Debug.LogError("Rosszul van elnevezve a pálya!");
            return;
        }
        
        int ep = int.Parse(split[0].Substring(1));
        int map = int.Parse(split[1].Substring(1));
        Debug.Log($"Episode: {ep}, Map: {map}");

        episode = (GameData.Episodes)ep-1; //Az EP1 értéke 0, emiatt kell kivonni egyet.
        hanyadik = map+1; //A "hanyadik" azt a pályát jelenti, hogy melyik pályát oldja fel, azaz a következõt.

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!loaded)
        {
            if (DataCollector.Instance != null) DataCollector.Instance.MapCompleted(episode,hanyadik);
            else Debug.LogWarning("Nincsen DataCollector!");

            Debug.Log("Pályatöltés.");
            LoadNextLevel();
            loaded = true;
        }
    }

    void LoadNextLevel()
    {
        string nextMapWithinEp = $"e{(int)episode+1}_m{hanyadik}"; 
        //Az EP1 értéke 0. Ha csak behelyettesítenék, akkor az lenne, hogy e0_..., de e1 kell.

        string nextEp = $"e{(int)episode + 2}_m0";
        //Szintén, mivel az epizódok enum értéke 1-gyel kevesebb mint az olvasható érték (EP1 = 0, EP2 = 1), emiatt hozzá kell adni egyet,
        //hogy az olvasható érték megfeleljen az epizód számának ( EP1 = 1 legyen az érték).
        //Így a következõ epizódnál az olvasható érték +2-vel lesz nagyobb. (EP1-nek +1 az olvasható értéke. A következõ epizódnak meg ugye E1+1.
        // Mivel E1 = 1, E1+1 = 1+1, azaz 2.

        // Remélem érthetõ.

        string sceneToLoad;
        if (Application.CanStreamedLevelBeLoaded(nextMapWithinEp))
        {
            sceneToLoad = nextMapWithinEp;
        }
        else if (Application.CanStreamedLevelBeLoaded(nextEp))
        {
            sceneToLoad = nextEp;
        }
        else
        {
            Debug.LogWarning($"Nem találok több pályát! Ezeket kerestem: {nextMapWithinEp}, {nextEp}." );
            return;
        }

        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }
}
