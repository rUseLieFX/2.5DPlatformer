using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundDetectorScript : MonoBehaviour
{
    [SerializeField] Collider[] colliders = new Collider[10]; //Egyszerre 10 objektnél többel csak nem fog ütközni a groundcheck.

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        //Elkezdjük átpörgetni a colliders tömböt.
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] != null) continue; //Ha az adott helyen már van valami, akkor következõ helyet nézzük.
            else //Ilyenkor nincsen, tehát jegyezzük fel, és hagyjuk abba a pörgetést.
            {
                colliders[i] = other;
                break;
            }
        }
        PlayerController_Script.instance.OnGround(true);
    }

    private void OnTriggerExit(Collider other)
    {
        int uresek = 0;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] == other)
            {
                colliders[i] = null;
                uresek++;
                continue;
            }
            if (colliders[i] == null) uresek++;

        }

        if (uresek == colliders.Length)
        {
            PlayerController_Script.instance.OnGround(false);
        }
    }
}
