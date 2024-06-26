using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterScript : MonoBehaviour
{
    [SerializeField] PlayerController_Script.GravityDirection startingGravityDir;
    Quaternion startingRotation;
    [Tooltip("Ez csak debug indokb�l van itt, ne ny�lj hozz�.")]
    [SerializeField] GameObject[] gravityChangerObjects;

    public static GameMasterScript instance;
    private void Awake()
    {
        if (instance != null) Debug.LogError("T�bb GameMaster van!");
        instance = this;
    }

    private void Start()
    {
        PlayerController_Script.instance.ChangeGravity((int)startingGravityDir);

        startingRotation = CameraController_Script.instance.transform.rotation;
        List<GameObject> gameObjects = new List<GameObject>();
        
        //Legyen lementve az �sszes gravity changer egy list�ba.
        foreach (var item in GameObject.FindObjectsOfType<GravityChangerEntityScript>())
        {
            gameObjects.Add(item.gameObject);
        }
        gravityChangerObjects = gameObjects.ToArray();

    }

    public void MapReset()
    {

        PlayerController_Script.instance.ChangeGravity((int)startingGravityDir);
        CameraController_Script.instance.transform.rotation = startingRotation;
        //Ha resetel�dik a map, akkor legyen aktiv�lva az �sszes gravity changer.
        foreach (var item in gravityChangerObjects)
        {
            item.gameObject.SetActive(true);
        }

    }
}
