using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class ShowText : MonoBehaviour
    {
        public TextMeshProUGUI textDisplay; 
        public float typingSpeed = 0.05f;  

        void Start()
        {
            string message = "YOU LOSE"; 
            StartCoroutine(DisplayText(message));
        }

        IEnumerator DisplayText(string input)
        {
            textDisplay.text = ""; 
            foreach (char letter in input.ToCharArray())
            {
                textDisplay.text += letter;
                yield return new WaitForSeconds(typingSpeed); 
            }
        }
    }
}
