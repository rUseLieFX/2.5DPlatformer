using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPoint : MonoBehaviour
{

    private void OnDrawGizmosSelected()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward*2,Color.green);
    }
}
