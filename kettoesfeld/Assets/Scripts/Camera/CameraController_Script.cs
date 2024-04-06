#define DEBUGG
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController_Script : MonoBehaviour
{
    [SerializeField] Transform cameraPoint;
    [SerializeField] Transform cam;
    [SerializeField] float timeToTurn;
    float timeTurned;
    [SerializeField] Vector3 dirToTurn;
    bool blocked; //Lehet-e forgatni a kamerát?

    public Transform CameraTransform
    {
        get { return cameraPoint; }
    }

    public Transform GetCam
    {
        get { return cam; }
    }

    public static CameraController_Script instance;

    /*
    // {0,0}    - nincs semmerre sem ugrás.
    // {1,0}    - jobbra van az ugrás.
    // {-1,0}   - balra van az ugrás.
    // {0,1}    - felfele van az ugrás.
    // {0,-1}   - lefele van az ugrás.
    // {1,1}    - lehetetlen.
    */
    /// <summary>
    /// Megadja azt az irányt relatív a kamerához, amerre az ugrásnak kell történnie.
    /// </summary>
    public Vector2 JumpDimension {
        get {
            Vector3 grav = -Physics.gravity.normalized;
            if (cam.up == grav)
            {
                return Vector2.up;
            }
            else if (cam.right == grav)
            {
                return Vector2.right;
            }
            else if (-cam.right == grav)
            {
                return -Vector2.right;
            }
            else if (-cam.up == grav)
            {
                return -Vector2.up;
            }
            else return Vector2.zero;
            
        }

    }
    public bool Blocked
    {
        get { return blocked; }
        set { blocked = value; }
    }

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        timeTurned = timeToTurn*2; //Azért, hogy ne gondolja azt a játék, hogy a pálya betöltésekor kell már fordulni. igen, ezt illene kicsit átírni a jövõben.

        if (Camera_Startup_Animation_Script.instance != null) //Ha van overview kamera a pályán, akkor...
        {
            Camera_Startup_Animation_Script.instance.onAnimationEnded += AnimationEnded; //Jelezzen, ha a cutscene véget ért.
            blocked = true;
        }
        else blocked = false;
    }

    void AnimationEnded()
    {
        blocked = false;
    }

    void Update()
    {
        if (timeTurned <= timeToTurn)
        {
            transform.RotateAround(transform.position, dirToTurn, 90 * Time.deltaTime / timeToTurn);
            timeTurned += Time.deltaTime;
            if (timeTurned > timeToTurn)
            {
                float x = Mathf.Round((transform.rotation.eulerAngles / 90).x);
                float y = Mathf.Round((transform.rotation.eulerAngles / 90).y);
                float z = Mathf.Round((transform.rotation.eulerAngles / 90).z);

                transform.rotation = Quaternion.Euler(x*90, y*90, z*90);
                blocked = false;
                Camera.main.orthographic = true;
            }
        }
#if (DEBUGG)
        Debug.DrawLine(cam.position, cam.position+cam.right*2, Color.red);
        Debug.DrawLine(cam.position, cam.position+cam.up*2, Color.green);
#endif
        //Ha lehet mozogni...
        if (!blocked)
        {
            //Akkor nézzük az inputot, és forgassuk a kamerát az alapján.
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Turn(0);
            }

            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Turn(1);
            }

            else if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                Turn(2);
            } 
            
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Turn(3);
            }
        }

    }

    void Turn(int key) 
    {
        dirToTurn = Vector3.zero;
        switch (key)
        {
            case 0:
                {
                    dirToTurn = -cam.right;
                    break;
                }
            case 2:
                {
                    dirToTurn = cam.right;
                    break;
                }
            case 1:
                {
                    dirToTurn = cam.up;
                    break;
                }
            case 3:
                {
                    dirToTurn = -cam.up;
                    break;
                }
        }

        Camera.main.orthographic = false; //Lehessen átlátni a mapot 3D-ben.
        timeTurned = 0; //Állítsuk 0-ra azt, hogy mennyi ideje forog a kamera.
        blocked = true; //Ne fogadjon el több inputot a kamerával kapcsolatban.

    }

    public Vector3 GetRight
    {
        get { return cam.right; }
    }

    public Vector3 GetUp
    {
        get { return cam.up; }
    }
}
