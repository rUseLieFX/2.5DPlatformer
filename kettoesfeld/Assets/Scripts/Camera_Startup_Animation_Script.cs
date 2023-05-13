using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Camera_Startup_Animation_Script : MonoBehaviour
{

    //[SerializeField] bool hasAnimation;

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

        cam = GetComponent<Camera>();
        playerCamera = CameraController_Script.instance.GetComponentInChildren<Camera>().transform;

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

    // Erre fog r�csatlakozni a kamer�t, �s a j�t�kost ir�ny�t� k�d. 
    // A saj�t k�djuk oldja meg a blokkol�st, �s a felold�st.

    void Update()
    {
        if (animationTimeSpent < animationTime)
        {
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