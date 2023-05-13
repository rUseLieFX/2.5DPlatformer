using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Camera_Startup_Animation_Script : MonoBehaviour
{

    [SerializeField] bool hasAnimation;

    [SerializeField] Transform playerCamera; 
    [SerializeField] Transform animationPoint; //T�mb?
    [SerializeField] float startingFov;

    [SerializeField] Quaternion cameraDefaultRot;
    [SerializeField] Quaternion animationStartRot;

    [SerializeField] Vector3 cameraDefaultPos;
    [SerializeField] Vector3 animationStartPos;

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


        if (hasAnimation)
        {

            //Kezdetleges �rt�kek megszerz�se.

            cameraDefaultRot = playerCamera.rotation;
            animationStartRot = animationPoint.rotation;
            startingFov = cam.fieldOfView;

            cameraDefaultPos = playerCamera.position;
            animationStartPos = animationPoint.position;



            //Kezdetleges �rt�kek haszn�l�sa.

            transform.rotation = animationStartRot;
            transform.position = animationStartPos;
            animationTimeSpent = 0f;
        }
        else
        {
            Destroy(gameObject);
            Destroy(animationPoint.gameObject);
        }
    }

    public event System.Action onAnimationEnded;

    // Erre fog r�csatlakozni a kamer�t, �s a j�t�kost ir�ny�t� k�d. 
    // A saj�t k�djuk oldja meg a blokkol�st, �s a felold�st.

    void Update()
    {
        if (hasAnimation)
        {
            if (animationTimeSpent < animationTime)
            {
                transform.position = Vector3.Lerp(animationStartPos, cameraDefaultPos, animationTimeSpent / animationTime);
                transform.rotation = Quaternion.Lerp(animationStartRot, cameraDefaultRot, animationTimeSpent / animationTime);
                cam.fieldOfView = Mathf.Lerp(startingFov, 60, animationTimeSpent / animationTime);
                animationTimeSpent += Time.deltaTime;
            }

            if (animationTimeSpent >= animationTime)
            {
                Destroy(gameObject);
                Destroy(animationPoint.gameObject);
                Debug.LogWarning("todo, meg kell csin�lni a blokkol�s dolgokat!"); //A kamer�t meg a j�t�kost ne lehessen mozgatni.
                onAnimationEnded?.Invoke();
            }
        }

    }
}
