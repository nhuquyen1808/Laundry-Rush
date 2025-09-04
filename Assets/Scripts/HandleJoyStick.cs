using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class HandleJoyStick : MonoBehaviour
    {
        public GameObject handleTutorial;

        private void OnMouseDown()
        {
            handleTutorial.SetActive(false);
        }
    }
}
