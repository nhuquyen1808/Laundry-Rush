using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class UiGame13 : MonoBehaviour
    {
        public Image previewImage,imagePar;
        public GameObject panelPreview;
        public Button previewButton,disablePreview;
        public List<Sprite> previewSprites = new List<Sprite>();
        public UiWinLose uiWinLose;

        public static UiGame13 instance;
        
        private void Awake()
        {
            instance = this;
            previewButton.onClick.AddListener(ShowPreview);
            disablePreview.onClick.AddListener(DisablePreviewPanel);
        }

        private void DisablePreviewPanel()
        {
            imagePar.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).From(1).OnComplete(() =>
            {
                panelPreview.SetActive(false);
            });
        }

        public void ShowPreview()
        {
            panelPreview.SetActive(true);
            imagePar.transform.DOScale(1, 0.3f).From(0).SetEase(Ease.OutBack);
            previewImage.sprite = previewSprites[0];
        }
        
        
    }
}
