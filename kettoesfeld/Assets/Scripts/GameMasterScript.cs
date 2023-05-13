using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterScript : MonoBehaviour
{
    [SerializeField] PlayerController_Script.GravityDirection startingGravityDir;
    Quaternion startingRotation;
    [Tooltip("Ez csak debug indokból van itt, ne nyúlj hozzá.")]
    [SerializeField] GameObject[] gravityChangerObjects;

    public static GameMasterScript instance;
    private void Awake()
    {
        if (instance != null) Debug.LogError("Több GameMaster van!");
        instance = this;
    }

    private void Start()
    {
        PlayerController_Script.instance.ChangeGravity((int)startingGravityDir);
        startingRotation = CameraController_Script.instance.transform.rotation;
        List<GameObject> gameObjects = new List<GameObject>();
        
        foreach (var item in GameObject.FindObjectsOfType<GravityChangerEntityScript>())
        {
            gameObjects.Add(item.gameObject);
        }
        gravityChangerObjects = gameObjects.ToArray();

    }
    private void Update()
    {
       
    }

    public void MapReset()
    {
        PlayerController_Script.instance.ChangeGravity((int)startingGravityDir);
        CameraController_Script.instance.transform.rotation = startingRotation;
        foreach (var item in gravityChangerObjects)
        {
            item.gameObject.SetActive(true);
            Debug.Log("poggers");
        }

    }
}
