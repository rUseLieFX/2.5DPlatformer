using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundDetectorScript : MonoBehaviour
{
    [SerializeField] Collider[] colliders = new Collider[16]; //Egyszerre 16 objektn�l t�bbel csak nem fog �tk�zni a groundcheck.
    //Igen, ez nem a legszebb megold�s, dinamikusan jobb lenne, de jelenleg m�g soknak tartom, mivel �gy sem annyira komplik�ltak a p�ly�k.

    //Ami�rt ez kell, az az, hogy ha A objektr�l �tmegy�nk B objektre, ne tudjon ugrani a karakter.
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
        PlayerController_Script.instance.OnGround(true); //Mivel valamihez hozz��rt�nk ami nem trigger, emiatt biztos hogy a f�ld�n vagyunk.
    }

    private void OnTriggerExit(Collider other)
    {
        //Ha elhagyjuk "A" objektet, de m�g "B" tart minket, ha csak azt vessz�k figyelembe, hogy 'Elhagytam-e azt, amir�l eddig el tudtam ugrani?'
        //akkor hamisan azt jelezn�, hogy igen. Emiatt van t�rolva, hogy kikkel �tk�zik akt�van a ground detector.
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

        //Ha m�r semmivel sem �rintkez�nk, akkor lehessen ugrani.
        if (uresek == colliders.Length)
        {
            PlayerController_Script.instance.OnGround(false);
        }
    }
}
