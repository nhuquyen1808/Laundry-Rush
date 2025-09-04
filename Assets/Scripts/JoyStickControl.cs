using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class JoyStickControl : MonoBehaviour
    {
        public Image stick;
        public Image Handle;
        
        
        void Update()
        {
            Vector3 direction = Handle.transform.position - stick.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            stick.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }
}
