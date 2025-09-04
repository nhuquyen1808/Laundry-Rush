using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class Mission : MonoBehaviour
    {
        public int id;
        public float currentState, totalState;
        public Button goButton, claimButton;
        public int coinGet;
        public TextMeshProUGUI missionDescription;
        public Image stateFillImage;
        public TextMeshProUGUI stateText;

        private void Awake()
        {
            goButton.onClick.AddListener(OnclickGoButton);
            claimButton.onClick.AddListener(OnClickClaimButton);
        }

        private void OnClickClaimButton()
        {
            Debug.Log("Handle OnClickClaimButton");
            claimButton.interactable = false;
            PlayerPrefs.SetInt($"MISSION_{id}_CLAIMED", 1);
            CurrencyBar.instance.AddCoin(coinGet);
        }

        private void OnclickGoButton()
        {
            Debug.Log("Handle OnclickGoButton");
        }

        public void UpdateMission()
        {
            stateFillImage.fillAmount = currentState / totalState;
            stateText.text = $"{currentState}/{totalState}";
            if (currentState >= totalState)
            {
                if (PlayerPrefs.GetInt($"MISSION_{id}_CLAIMED") == 0)
                {
                    claimButton.interactable = true;
                }
                else
                {
                    claimButton.interactable = false;
                }
            }
            else
            {
                    claimButton.interactable = false;
            }
        }
    }
}