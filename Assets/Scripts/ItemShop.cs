using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DevDuck
{
    public enum TYPE_CURRENCY
    {
        DIAMOND,
        GOLD
    }

    public enum SHADOW_TYPE
    {
        S,
        M,
        L
    }

    public class ItemShop : MonoBehaviour
    {
        public int id;
        public SHADOW_TYPE shadowType;

        public FashionType fashionType;

        // public TYPE_CURRENCY currencyType;
        public float price;
        [Header("UI Elements : ")] public Image iconImage;
        public Image shadowImage;
        public TextMeshProUGUI priceText;
        public Image iconCurrency;
        public Button iconButton, buyButton;

        public bool isOnModel;

        private void Awake()
        {
            iconButton.onClick.AddListener(OnclickIconButton);
            buyButton.onClick.AddListener(OnclickBuyButton);
        }

        private void OnclickBuyButton()
        {
            //ChangeState();
            Observer.Notify(EventAction.EVENT_BUY_ITEM, this.id);
        }

        public void ChangeState()
        {
            buyButton.interactable = false;
            // iconImage.raycastTarget = false;
            iconCurrency.color = new Color32(255, 255, 255, 160);
            // priceText.color = new Color32(255, 255, 255, 160);
            priceText.text = "Owned";
        }

        private void OnclickIconButton()
        {
            //iconImage.raycastTarget = false;
            if (!isOnModel)
            {
                Observer.Notify(EventAction.EVENT_TRY_ITEM, id);
                isOnModel = true;
            }
            else
            {
                isOnModel = false;
                Observer.Notify(EventAction.EVENT_TRY_RELEASE_ITEM, id);
            }
        }

        public void OnItemReleased()
        {
            //  imageSelected.enabled = false;
            iconImage.raycastTarget = true;
        }
    }
}