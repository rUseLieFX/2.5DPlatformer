using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundDetectorScript : MonoBehaviour
{
    [SerializeField] Collider[] colliders = new Collider[10]; //Egyszerre 10 objektn�l t�bbel csak nem fog �tk�zni a groundcheck.

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        //Elkezdj�k �tp�rgetni a colliders t�mb�t.
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] != null) continue; //Ha az adott helyen m�r van valami, akkor k�vetkez� helyet n�zz�k.
            else //Ilyenkor nincsen, teh�t jegyezz�k fel, �s hagyjuk abba a p�rget�st.
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
