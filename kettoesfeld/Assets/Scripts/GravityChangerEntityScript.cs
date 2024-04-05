using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class GravityChangerEntityScript : MonoBehaviour
{
    [SerializeField] bool getDeleted;
    [SerializeField] PlayerController_Script.GravityDirection gravityIndex;
    [SerializeField] Transform arrow;

    private void Start()
    {
        if (arrow != null)
        {
            switch ((int)gravityIndex)
            {
                case 0:
                    arrow.rotation = Quaternion.Euler(90, 0, 0f);
                    break;
                case 1:
                    arrow.rotation = Quaternion.Euler(-90, 0, 0f);
                    break;
                case 2:
                    arrow.rotation = Quaternion.Euler(0, -90, 90f);
                    break;
                case 3:
                    arrow.rotation = Quaternion.Euler(0, 90, 90);
                    break;
                case 4:
                    arrow.rotation = Quaternion.Euler(0, 180, 90);
                    break;
                case 5:
                    arrow.rotation = Quaternion.Euler(0, 0, 90);
                    break;
                default:
                    Debug.LogError("Nem jó gravitációs irányt kaptam!");
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController_Script.instance.ChangeGravity((int)gravityIndex);
        if (getDeleted) gameObject.SetActive(false);
    }
}
