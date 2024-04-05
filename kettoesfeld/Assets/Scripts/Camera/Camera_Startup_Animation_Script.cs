using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//K�t kamera van - az egyik a j�t�kos�, a m�sodik az ahonnan indul a kamera, amikor bet�lt a p�lya. Ez ad egy overview-t a p�ly�r�l.
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
        
        cam = GetComponent<Camera>(); //Saj�t gameobject kamer�ja.
        playerCamera = CameraController_Script.instance.GetComponentInChildren<Camera>().transform; //A j�t�kos kamer�ja

        //Kezdetleges �rt�kek megszerz�se.

        cameraDefaultRot = playerCamera.rotation;
        animationStartRot = transform.rotation;
        startingFov = cam.fieldOfView;

        cameraDefaultPos = playerCamera.position;
        animationStartPos = transform.position;

        //Kezdetleges �rt�kek haszn�l�sa.

        transform.rotation = animationStartRot;
        transform.position = animationStartPos;
        animationTimeSpent = 0f;
    }

    public event System.Action onAnimationEnded;

    // Erre fog r�csatlakozni a kamera, �s a j�t�kost ir�ny�t� k�d. 
    // A saj�t k�djuk oldja meg a blokkol�st, �s a felold�st.

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