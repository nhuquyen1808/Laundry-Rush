using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using System.IO;
using ntDev;
using UnityEngine.Serialization;
using File = System.IO.File;

namespace DevDuck
{
    public class FashionItem : MonoBehaviour
    {
        public string theme;
        public int id;
        public Image icon;
        public FashionType fashionType;
        public float price;
        public bool isAds;
        public TextMeshProUGUI coinText;
        public Image themeFashionImage;
        public Image imageSelected;
        public Image buyButtonImage;
        public ItemFashionGame itemFashionGame;
        public string Ads;
        public Image adIcon;
        public GameObject themePar;
        public Image sprBG;
        public TextMeshProUGUI OwnerText;

        private void Start()
        {
            itemFashionGame.id = this.id;
            Observer.AddObserver(EventAction.EVENT_ITEM_SELECTED, OnItemSelected);
            //SetTheme();
        }

        private void OnDestroy()
        {
            Observer.RemoveObserver(EventAction.EVENT_ITEM_SELECTED, OnItemSelected);
        }

        public void OnItemSelected(object obj)
        {
            int idSelected = (int)obj;
            if (idSelected == this.id && !isAds)
            {
                imageSelected.enabled = true;
                buyButtonImage.gameObject.SetActive(false); 
                sprBG.enabled = false;
            }
        }

        public void OnItemReleased()
        {
            imageSelected.enabled = false;
            sprBG.enabled = true;
        }

        public async void SetData(int idSetted, FashionType fashionType, float price, string themeSetted, string ads,
            Sprite themeSprite)
        {
            this.id = idSetted;
            this.fashionType = fashionType;
            this.price = price;
            this.coinText.text = price.ToString();
            this.Ads = ads;
            this.theme = themeSetted;
            if (ManagerAsset.IsExist($"icon_{this.id}", (typeof(Sprite))))
            {
                Sprite spr = await ManagerAsset.LoadAssetAsync<Sprite>($"icon_{this.id}");
                this.icon.sprite = spr;
            }

            this.themeFashionImage.sprite = themeSprite;

            if (this.theme == "DEFAULT")
            {
                themePar.SetActive(false);
            }
        }
    }
}