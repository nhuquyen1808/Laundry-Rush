using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class TutorialPanel : MonoBehaviour
    {
        public Button nextButton, previousButton;
        int currentPage;
        public List<GameObject> pages = new List<GameObject>();
        public Button okayButton;
        private void Awake()
        {
            nextButton.onClick.AddListener(OnClickNextButton);
            previousButton.onClick.AddListener(OnClickPreviousButton);
        }

        private void OnClickPreviousButton()
        {
            --currentPage;
            
        }

        private void OnClickNextButton()
        {
            currentPage++;
            if (currentPage >= pages.Count)
            {
                nextButton.interactable = false;
                okayButton.gameObject.SetActive(false);
            }
        }


        void Start()
        {
        
        }
        void Update()
        {
        
        }
    }
}
