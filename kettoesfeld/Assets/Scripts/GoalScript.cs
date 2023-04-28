using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalScript : MonoBehaviour
{
    bool loaded = false;
    [SerializeField] string sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (!loaded)
        {
            Debug.Log("Pályatöltés.");
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
            loaded = true;
        }
    }
}
