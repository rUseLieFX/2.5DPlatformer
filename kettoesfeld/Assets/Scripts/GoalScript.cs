using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalScript : MonoBehaviour
{
    [SerializeField] string sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("P�lyat�lt�s.");
        
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }
}
