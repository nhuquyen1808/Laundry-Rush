using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class LevelButton : MonoBehaviour
    {
        public int id;
        private Image image;
        public Image leftStar, middleStar, rightStar;
        public TextMeshProUGUI levelText;
        
        public Image imageLevel
        {
            get
            {
                if (image == null)
                {
                    image = GetComponent<Image>();
                }

                return image;
            }
        }
        
        private Button levelButton;

        public Button LevelButtonImage
        {
            get
            {
                if (levelButton == null)
                {
                    levelButton = GetComponent<Button>();
                    
                }
                return levelButton;
            }
        }

        private void Awake()
        {
            LevelButtonImage.onClick.AddListener(OnClickLevelButton);
        }
        private void SetLevelText()
        {
            levelText.text = "Level : " + id.ToString();
        }
        private void OnClickLevelButton()
        {
            Debug.Log($"Level {id} selected then load that level");
        }

        public void SetLevelUnLock()
        {
            imageLevel.color = Color.cyan;
            SetLevelText();
        }

        public void SetLevelLock()
        {
            LevelButtonImage.interactable = false;
            imageLevel.color = Color.red;
            leftStar.gameObject.SetActive(false);
            middleStar.gameObject.SetActive(false);
            rightStar.gameObject.SetActive(false);
            SetLevelText();
        }

        public void SetCurrentLevel()
        {
            leftStar.color = Color.black;
            middleStar.color = Color.black;
            rightStar.color = Color.black;
            SetLevelText();
        }
    }
}