using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalScript : MonoBehaviour
{
    bool loaded = false;

    //Hanyadik p�ly�ra kaphat enged�lyt, ha ez elk�sz�lt? 
    [Header("Jelenlegi p�lya adatai")]
    [SerializeField] GameData.Episodes episode;
    [SerializeField] int hanyadik;

    private void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name; //pl e1_m1
        string[] split = sceneName.Split('_'); // ebb�l lesz "e1" �s "m1"
        if (split.Length != 2) 
        {
            Debug.LogError("Rosszul van elnevezve a p�lya!");
            return;
        }
        
        int ep = int.Parse(split[0].Substring(1));
        int map = int.Parse(split[1].Substring(1));
        Debug.Log($"Episode: {ep}, Map: {map}");

        episode = (GameData.Episodes)ep-1; //Az EP1 �rt�ke 0, emiatt kell kivonni egyet.
        hanyadik = map+1; //A "hanyadik" azt a p�ly�t jelenti, hogy melyik p�ly�t oldja fel, azaz a k�vetkez�t.

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!loaded)
        {
            if (DataCollector.Instance != null) DataCollector.Instance.MapCompleted(episode,hanyadik);
            else Debug.LogWarning("Nincsen DataCollector!");

            Debug.Log("P�lyat�lt�s.");
            LoadNextLevel();
            loaded = true;
        }
    }

    void LoadNextLevel()
    {
        string nextMapWithinEp = $"e{(int)episode+1}_m{hanyadik}"; 
        //Az EP1 �rt�ke 0. Ha csak behelyettes�ten�k, akkor az lenne, hogy e0_..., de e1 kell.

        string nextEp = $"e{(int)episode + 2}_m0";
        //Szint�n, mivel az epiz�dok enum �rt�ke 1-gyel kevesebb mint az olvashat� �rt�k (EP1 = 0, EP2 = 1), emiatt hozz� kell adni egyet,
        //hogy az olvashat� �rt�k megfeleljen az epiz�d sz�m�nak ( EP1 = 1 legyen az �rt�k).
        //�gy a k�vetkez� epiz�dn�l az olvashat� �rt�k +2-vel lesz nagyobb. (EP1-nek +1 az olvashat� �rt�ke. A k�vetkez� epiz�dnak meg ugye E1+1.
        // Mivel E1 = 1, E1+1 = 1+1, azaz 2.

        // Rem�lem �rthet�.

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
            Debug.LogWarning($"Nem tal�lok t�bb p�ly�t! Ezeket kerestem: {nextMapWithinEp}, {nextEp}." );
            return;
        }

        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }
}
