using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalScript : MonoBehaviour
{
    [SerializeField] string sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Pályatöltés.");
        
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }
}
