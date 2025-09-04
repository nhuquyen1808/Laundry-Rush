using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public enum TypeRotor
    {
        FRONT,
        BACK
    }

    public class RotorHelicopter : MonoBehaviour
    {
        public float speed;
        private float rotY = 0;
        public TypeRotor type;

        void Update()
        {
            rotY += speed*10 * Time.deltaTime;
            if (type == TypeRotor.FRONT)
            {
                transform.rotation = Quaternion.Euler(0, rotY, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(rotY,0 , 0);
            }
        }
    }
}