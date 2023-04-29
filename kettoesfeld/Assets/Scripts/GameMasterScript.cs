using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterScript : MonoBehaviour
{
    [SerializeField] PlayerController_Script.GravityDirection startingGravityDir;
    Quaternion startingRotation;
    [Tooltip("Ez csak debug indokb�l van itt, ne ny�lj hozz�.")]
    [SerializeField] GameObject[] gravityChangerObjects;
    [Header("Kezd� anim�ci�")]
    [SerializeField] bool animationStart;
    [SerializeField] Transform animationStartPoint;
    [SerializeField] float animationTimer;
    Quaternion camStartingRot;

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
        
        foreach (var item in GameObject.FindObjectsOfType<GravityChangerEntityScript>())
        {
            gameObjects.Add(item.gameObject);
        }
        gravityChangerObjects = gameObjects.ToArray();

        if (animationStart)
        {
            StartAnimation();
            camStartingRot = CameraController_Script.instance.transform.rotation;
        }
    }

    float timeOfAnimation = 0;
    bool happened = false;
    private void Update()
    {
        if (animationStart)
        {
            Debug.LogError("ASAP �t kell ezt �rni, ez egy fos.");
            //Debug.Log($"{timeOfAnimation / animationTimer} {timeOfAnimation} {animationTimer}");
            if (timeOfAnimation <= animationTimer && !happened)
            {
                Transform cam = CameraController_Script.instance.GetCam;
                cam.position = Vector3.Lerp(animationStartPoint.position, new Vector3(0, 1, -10), timeOfAnimation / animationTimer);
                cam.rotation = Quaternion.Lerp(animationStartPoint.rotation, camStartingRot, timeOfAnimation / animationTimer);
                timeOfAnimation += Time.deltaTime;
            }

            if (timeOfAnimation > animationTimer && !happened)
            {
                happened = true;
                Transform cam = CameraController_Script.instance.GetCam;
                cam.position = new Vector3(0, 1, -10);
                cam.rotation = new Quaternion(0, 0, 0, 0);
                cam.GetComponent<Camera>().orthographic = true;
                CameraController_Script.instance.Blocked = false;

            }
        }
    }

    void StartAnimation()
    {
        Transform cam = CameraController_Script.instance.GetCam;
        cam.position = animationStartPoint.position;
        cam.rotation = animationStartPoint.rotation;
        cam.GetComponent<Camera>().orthographic = false;

        CameraController_Script.instance.Blocked = true;
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
