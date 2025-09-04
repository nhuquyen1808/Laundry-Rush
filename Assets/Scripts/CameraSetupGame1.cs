using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class CameraSetupGame1 : CameraSetUp     
    {
        public override void Start()
        {
            cam = GetComponent<Camera>();
            float aspectRatio = (float)Mathf.Max(Screen.width, Screen.height) / Mathf.Min(Screen.width, Screen.height);

            Debug.Log(aspectRatio);
            if (aspectRatio >= 1.6f) 
            {
                //  Debug.Log("Device: Phone");
                cam.orthographicSize = 12f;
            }
            else 
            {
                // Debug.Log("Device: Tablet");
                cam.orthographicSize = 9.6f;

            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
