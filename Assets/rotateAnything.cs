using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateAnything : MonoBehaviour

{
    float yAxis;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        yAxis += 200*  Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, yAxis, 0);
    }
}
