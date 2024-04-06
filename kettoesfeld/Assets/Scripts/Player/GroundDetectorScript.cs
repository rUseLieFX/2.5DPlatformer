using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundDetectorScript : MonoBehaviour
{
    [SerializeField] Collider[] colliders = new Collider[16]; //Egyszerre 16 objektnél többel csak nem fog ütközni a groundcheck.
    //Igen, ez nem a legszebb megoldás, dinamikusan jobb lenne, de jelenleg még soknak tartom, mivel úgy sem annyira komplikáltak a pályák.

    //Amiért ez kell, az az, hogy ha A objektrõl átmegyünk B objektre, ne tudjon ugrani a karakter.
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
        PlayerController_Script.instance.OnGround(true); //Mivel valamihez hozzáértünk ami nem trigger, emiatt biztos hogy a földön vagyunk.
    }

    private void OnTriggerExit(Collider other)
    {
        //Ha elhagyjuk "A" objektet, de még "B" tart minket, ha csak azt vesszük figyelembe, hogy 'Elhagytam-e azt, amirõl eddig el tudtam ugrani?'
        //akkor hamisan azt jelezné, hogy igen. Emiatt van tárolva, hogy kikkel ütközik aktívan a ground detector.
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

        //Ha már semmivel sem érintkezünk, akkor lehessen ugrani.
        if (uresek == colliders.Length)
        {
            PlayerController_Script.instance.OnGround(false);
        }
    }
}
