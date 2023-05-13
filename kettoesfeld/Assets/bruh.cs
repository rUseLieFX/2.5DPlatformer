using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bruh : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject buon;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(buon, canvas.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
