using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController_Script : MonoBehaviour
{
    [SerializeField] float maxSpeed,moveSpeed, jumpHeight;
    public static PlayerController_Script instance;

    bool grounded;
    Vector3 gravitationTowardsX = new Vector3(9.81f, 0, 0);
    Vector3 gravitationTowardsY = new Vector3(0, 9.81f, 0);
    Vector3 gravitationTowardsZ = new Vector3(0, 0, 9.81f);
    Vector3 startPos;

    Rigidbody rb;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        Vector2 input = GetInput();
        MovePlayer(input);
    }

    void MovePlayer(Vector2 dirs)
    {
        //A gravitáció iránya
        Vector3 gravityDir = Physics.gravity.normalized;
        //A gravitáció iránya abszolút értékben
        Vector3 absGravity = new Vector3(Mathf.Abs(Physics.gravity.x), Mathf.Abs(Physics.gravity.y), Mathf.Abs(Physics.gravity.z));
        //A gravitációs tengelyen lévõ sebesség
        float gravityAxisSpeed = Vector3.Scale(rb.velocity, gravityDir).magnitude;

        if (gravityAxisSpeed > 30)
        {
            //Eventekkel még meglehetne csinálni, de eh.
            GameMasterScript.instance.MapReset();
            rb.position = startPos;
            rb.velocity = Vector3.zero;
        }

        bool jump = false;
        Vector2 jumpDir = CameraController_Script.instance.JumpDimension;

        //Ha {1,*} / {-1,*} inputot adjuk, és {1,0} / {-1,0} a jump dimenson, akkor arra van az ugrás, és akarunk ugrani.
        if (dirs[0] == jumpDir[0] && jumpDir[0] != 0 && grounded)
        {
            jump = true;
        }
        else if (dirs[1] == jumpDir[1] && jumpDir[1] != 0 && grounded)
        {
            jump = true;
        }

        if (jump && Mathf.Round(gravityAxisSpeed) == 0)
        {
            rb.velocity += -gravityDir.normalized * jumpHeight;
            //Debug.LogWarning($"Ugrási vektor: {absGravity.normalized * jumpHeight}");
            //Debug.LogWarning("Ugrás!");
        }

        Vector3 dir = dirs[0]*CameraController_Script.instance.GetRight
            + dirs[1] * CameraController_Script.instance.GetUp;

        Vector3 freeToMove = absGravity.normalized; //Azok a tengelyek, amiken szabadon mozoghat a játékos, tehát nem kell a gravitáció miatt félni.
        freeToMove = new Vector3(Mathf.Abs(freeToMove.x), Mathf.Abs(freeToMove.y), Mathf.Abs(freeToMove.z));

        //A számok invertálása, tehát ami 1-es az legyen nullás, ami nulla az legyen egy.
        //Elméletben átlós gravitációnál is okés.
        freeToMove = Vector3.one - freeToMove;
        //Ha a gravitáció (0,1,0)-t hozott vissza, akkor ennek (1,0,1)-et kellett volna.

        //Debug.Log($"Szabad tengelyek: {freeToMove}");


        //Azok a tengelyek, amiken szabadon mozoghat a játékos, tehát nem kell a gravitáció miatt félni.
        //Debug.Log($"Direction:{dir}");

        dir = Vector3.Scale(dir,freeToMove);
        //Debug.Log($"Direction after freeToMove: {dir}");

        //Azért kell egy fix érték, hogy maga a moveSpeed változó ne legyen ilyen 2000-res érték.
        rb.AddForce(dir*moveSpeed*10*Time.deltaTime,ForceMode.Acceleration);


        //Speed control
        Vector3 vel = Vector3.Scale(rb.velocity, freeToMove); //Csak azokan tengelyek sebességét nézzük, amelyeken mozoghatunk (nem gravitációs)
        if (vel.magnitude > maxSpeed) //Ha a velocity vektor hosszabb, mint a max megengedett sebesség...
        {
            Vector3 velUj = vel.normalized * maxSpeed; //Akkor capeljük le.
            rb.velocity = velUj + Vector3.Scale(rb.velocity,absGravity.normalized) ; //Az ugrási sebességhez ne nyúljunk hozzá.
            //Mivel ez bármilyen tengelyen lehet, bármilyen irányba 

        }


    }
    /// <summary>
    /// A gravitáció irányának megváltoztatása.
    /// | 0 = -Y |
    /// 1 = Y |
    /// 2 = -X |
    /// 3 = X |
    /// 4 = -Z |
    /// 5 = Z |
    /// </summary>
    /// <param name="index">A gravitáció ID-je.</param>
    public void ChangeGravity(int index)
    {
        Quaternion cameraStartRot = CameraController_Script.instance.CameraTransform.rotation;
        //Forgatás még kell!
        switch (index)
        {
            case 0:
                Physics.gravity = -gravitationTowardsY;
                transform.rotation = Quaternion.Euler(0, 0, 0f);
                break;
            case 1:
                Physics.gravity = gravitationTowardsY;
                transform.rotation = Quaternion.Euler(0,0,180f);
                break;
            case 2:
                Physics.gravity = -gravitationTowardsX;
                transform.rotation = Quaternion.Euler(0, 0, 270f);
                break;
            case 3: 
                Physics.gravity = gravitationTowardsX;
                transform.rotation = Quaternion.Euler(0, 0, 90f);
                break;
            case 4:
                Physics.gravity = -gravitationTowardsZ;
                transform.rotation = Quaternion.Euler(90, 0, 0);
                break;
            case 5:
                Physics.gravity = gravitationTowardsZ;
                transform.rotation = Quaternion.Euler(-90, 0, 0);
                break;
            default:
                Debug.LogError("Nem jó gravitációs irányt kaptam!");
                break;
        }

        CameraController_Script.instance.CameraTransform.rotation = cameraStartRot;
    }
    public enum GravityDirection
    {
        NegY,
        PosY,
        NegX,
        PosX,
        NegZ,
        PosZ
    }

    public void OnGround(bool hmmm)
    {
        grounded = hmmm;
    }
    Vector3 eddigiGravitaciosRotation = Vector3.zero;
    Vector2 GetInput()
    {
        float vertical = 0;
        float horizontal = 0;
        if (Input.GetKey(KeyCode.W))
        {
            vertical += 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            vertical -= 1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            horizontal += 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            horizontal -= 1;
        }

        return new Vector2(horizontal,vertical);

    }
}
