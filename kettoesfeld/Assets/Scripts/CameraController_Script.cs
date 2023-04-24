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
    bool blocked;

    public Transform CameraTransform
    {
        get { return cameraPoint; }
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

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        blocked = false;
    }

    // Update is called once per frame
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
                /*
                if (Mathf.Abs(x) == 4) x = 0;
                if (Mathf.Abs(y) == 4) y = 0;
                if (Mathf.Abs(z) == 4) z = 0;
                */
                transform.rotation = Quaternion.Euler(x*90, y*90, z*90);
                blocked = false;
                Camera.main.orthographic = true;
            }
        }
#if (DEBUGG)
        Debug.DrawLine(cam.position, cam.position+cam.right*2, Color.red);
        Debug.DrawLine(cam.position, cam.position+cam.up*2, Color.green);
        //Debug.Log($"{camera.right} ; {camera.up}");
#endif
        #region pukeemoji
        if (!blocked)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Turn(KeyCode.RightArrow);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Turn(KeyCode.LeftArrow);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Turn(KeyCode.UpArrow);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Turn(KeyCode.DownArrow);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Turn(KeyCode.Space);
            }
        }
        #endregion

    }

    void Turn(KeyCode key) 
    {
        dirToTurn = Vector3.zero;
        switch (key)
        {
            case KeyCode.LeftArrow:
                {
                    dirToTurn = -cam.right;
                    break;
                }
            case KeyCode.RightArrow:
                {
                    dirToTurn = cam.right;
                    break;
                }
            case KeyCode.UpArrow:
                {
                    dirToTurn = cam.up;
                    break;
                }
            case KeyCode.DownArrow:
                {
                    dirToTurn = -cam.up;
                    break;
                }
        }
        //cameraLerpEndRot = Quaternion.Euler(rot + cameraLerpEndRot.eulerAngles);
        Camera.main.orthographic = false;
        timeTurned = 0;
        blocked = true;

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
