using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class CameraSetUp : MonoBehaviour
    {
        public Camera cam;
        
        public  virtual void Start()
        {
            cam = GetComponent<Camera>();
            float aspectRatio = (float)Mathf.Max(Screen.width, Screen.height) / Mathf.Min(Screen.width, Screen.height);

            if (aspectRatio >= 1.6f) 
            {
                cam.orthographicSize = 9.6f;
            }
            else 
            {
                cam.orthographicSize = 7.4F;

            }
        }

    }
}