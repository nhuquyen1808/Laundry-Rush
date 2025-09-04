using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DevDuck
{
    public class TutorialGame20 : MonoBehaviour
    {
        public Image transparentImage, handImage;
        public bool isDoneTut20;

        private void Start()
        {
            isDoneTut20 = PlayerPrefs.GetInt("isDoneTutorial20", 0) == 1;
        }

        public void HandHintMovement(Sprite transparentSprite)
        {
            transparentImage.sprite = transparentSprite;
            
            //handImage
        }
    }
}