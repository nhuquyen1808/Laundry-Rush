using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupShop : MonoBehaviour
{

    public Image shadow;
    public GameObject nPopup;
    public Button closeButton;
    public static PopupShop instance;
    
    private void Awake()
    {
        instance = this;
        closeButton.onClick.AddListener(OnClickCloseButton);
    }

    public void Show()
    {
        shadow.enabled = true;
        nPopup.SetActive(true);
    }

    private void OnClickCloseButton()
    {
       nPopup.SetActive(false);
       shadow.enabled = false;
    }
}
