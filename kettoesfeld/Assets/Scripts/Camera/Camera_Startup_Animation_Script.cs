using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Két kamera van - az egyik a játékosé, a második az ahonnan indul a kamera, amikor betölt a pálya. Ez ad egy overview-t a pályáról.
[RequireComponent(typeof(Camera))]
public class Camera_Startup_Animation_Script : MonoBehaviour
{
    Transform playerCamera; 
    float startingFov;

    Quaternion cameraDefaultRot;
    Quaternion animationStartRot;

    Vector3 cameraDefaultPos;
    Vector3 animationStartPos;

    [SerializeField] float animationTime;
    float animationTimeSpent;
    Camera cam;

    public static Camera_Startup_Animation_Script instance;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
        cam = GetComponent<Camera>(); //Saját gameobject kamerája.
        playerCamera = CameraController_Script.instance.GetComponentInChildren<Camera>().transform; //A játékos kamerája

        //Kezdetleges értékek megszerzése.

        cameraDefaultRot = playerCamera.rotation;
        animationStartRot = transform.rotation;
        startingFov = cam.fieldOfView;

        cameraDefaultPos = playerCamera.position;
        animationStartPos = transform.position;

        //Kezdetleges értékek használása.

        transform.rotation = animationStartRot;
        transform.position = animationStartPos;
        animationTimeSpent = 0f;
    }

    public event System.Action onAnimationEnded;

    // Erre fog rácsatlakozni a kamera, és a játékost irányító kód. 
    // A saját kódjuk oldja meg a blokkolást, és a feloldást.

    void Update()
    {
        if (animationTimeSpent < animationTime)
        {
            //Legyen mozgatva a kamera.
            transform.position = Vector3.Lerp(animationStartPos, cameraDefaultPos, animationTimeSpent / animationTime);
            transform.rotation = Quaternion.Lerp(animationStartRot, cameraDefaultRot, animationTimeSpent / animationTime);
            cam.fieldOfView = Mathf.Lerp(startingFov, 60, animationTimeSpent / animationTime);
            animationTimeSpent += Time.deltaTime;
        }
        else 
        {
            Destroy(gameObject);
            onAnimationEnded?.Invoke();
        }
    }
}