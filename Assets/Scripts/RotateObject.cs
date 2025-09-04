using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class RotateObject : MonoBehaviour
    {
        [Range(0f, 300f)]
        [SerializeField] private int speed;
        void Update()
        {
            transform.eulerAngles += new Vector3(0, 0, speed * Time.deltaTime);
        }
    }
}
