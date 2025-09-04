using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class TabFashionButton : MonoBehaviour
    {  
        Button tabFashion;
        public int id;
        public Image selectedImage,icon;
        public bool selected;
        private void Start()
        {
            tabFashion = GetComponent<Button>();
            tabFashion.onClick.RemoveAllListeners();
            tabFashion.onClick.AddListener(TabFashionSelected);
        }
        private void TabFashionSelected()
        {
            selectedImage.gameObject.SetActive(true);
            icon.gameObject.SetActive(true);
            selected = true;
            Observer.Notify(EventAction.EVENT_TAB_SELECTED, this.id);
        }

        public void DisableSelectedImage()
        {
            selectedImage.gameObject.SetActive(false);
            icon.gameObject.SetActive(false);
            selected = false;
        }
    }
}
