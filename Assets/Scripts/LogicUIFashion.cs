using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class LogicUIFashion : MonoBehaviour
    {
        public List<GameObject> tabsFashion = new List<GameObject>();
        public List<TabFashionButton> tabFashionButton = new List<TabFashionButton>();
        public Button nextButton;
        public Image iconThemeImage;

        [SerializeField] Image fillImage;
        public ParticleSystem heartExplosion, heartFlyParticle;

        public GameObject panelGetClothe;
        public Image iconImage;
        public Button watchAdsButton, closePanelGetClothesButton, buyButton;
        public Image PopUpPanelGetClothes;
        public List<GameObject> popUpPanelGetClothesElement = new List<GameObject>();
        public TextMeshProUGUI priceText;
        private float coinTemp;
        private int idTemp;
        public List<GameObject> LeftGroupButton = new List<GameObject>();
        public RectTransform rightFillbar;
        [SerializeField] TutorialFashion tutorialFashion;

        public FocusTUTFashion focusTutorial;
        private void Awake()
        {
            nextButton.onClick.AddListener(OnClickNextButton);
            closePanelGetClothesButton.onClick.AddListener(OnClickCloseGetClothesPanelButton);
            buyButton.onClick.AddListener(OnClickBuyButton);
            watchAdsButton.onClick.AddListener(OnClickWatchAdsButton);
        }

        void Start()
        {
            Observer.AddObserver(EventAction.EVENT_TAB_SELECTED, HandleTabSelected);
            DOVirtual.DelayedCall(0.7f, () => AnimShowElements());
            SetIconThemeImage();
        }

        private void OnDestroy()
        {
            Observer.RemoveObserver(EventAction.EVENT_TAB_SELECTED, HandleTabSelected);
        }

        private void AnimShowElements()
        {
            rightFillbar.gameObject.SetActive(true);
            rightFillbar.GetComponent<CanvasGroup>().DOFade(1, 0.3f).From(0).SetDelay(0.5f);
            for (int i = 0; i < tabFashionButton.Count; i++)
            {
                tabFashionButton[i].transform.DOScale(1, 0.3f).From(0).SetDelay(.1f * i).SetEase(Ease.OutBack);
            }
            for (int i = 0; i < LeftGroupButton.Count; i++)
            {
                var a = i;
                LeftGroupButton[a].transform.DOScale(1, 0.3f).SetDelay(a * 0.1f).SetEase(Ease.OutBack);
            }
        }
        private void OnClickWatchAdsButton()
        {
            Debug.Log("OnClickWatchAdsButton");
            Observer.Notify(EventAction.EVENT_BUY_ITEM, idTemp);
            for (int i = 0; i < popUpPanelGetClothesElement.Count; i++)
            {
                var a = i;
                popUpPanelGetClothesElement[a].transform.localScale = Vector3.zero;
            }

            panelGetClothe.SetActive(false);
        }

        private void OnClickBuyButton()
        {
            float coin = PlayerPrefs.GetFloat(PlayerPrefsManager.Coin);
            if (coin < coinTemp)
            {
                Debug.Log("Don't have enough coin");
                Handheld.Vibrate();
            }
            else
            {
                coin -= coinTemp;
                buyButton.interactable = false;
                Observer.Notify(EventAction.EVENT_UPDATE_COIN, coinTemp);
                Observer.Notify(EventAction.EVENT_BUY_ITEM, idTemp);
                /*if (tutorialFashion.isDoneTutFashion && !tutorialFashion.isDoneTutBuyInFashion /*&& GlobalData.currentTheme == "RESTAURANT"#1#)
                {
                    if (!tutorialFashion.isDoneTutBuyInFashion)
                    {
                      //  tutorialFashion.isDoneTopBuyInFashion = true;
                        Debug.Log("Start hint buy Bot ???");
                        tutorialFashion.handHint.SetActive(true);
                        tutorialFashion.handHintBuy.SetActive(false);
                        tutorialFashion.handHint.transform.position = tabFashionButton[2].transform.position;
                    }
                
                }*/
                
               // DOVirtual.DelayedCall(0.75f,(() => ClosePanel())) ;
            }
        }

        private void OnClickCloseGetClothesPanelButton()
        {
            ClosePanel();
        }

        Tween tween;
        private void ClosePanel()
        {
            for (int i = 0; i < popUpPanelGetClothesElement.Count; i++)
            {
                var a = i;
                popUpPanelGetClothesElement[a].transform.localScale = Vector3.zero;
            }
            panelGetClothe.SetActive(false);
            /*if (tutorialFashion.isDoneTutFashion && !tutorialFashion.isDoneTutBuyInFashion)
            {
                if (tutorialFashion.isDoneTopBuyInFashion)
                {
                    tutorialFashion.handHint.SetActive(false);

                    Debug.Log("Finish hint buy ???");
                    PlayerPrefs.SetInt("isDoneTutBuyInFashion", 1);
                    tutorialFashion.isDoneTutBuyInFashion = true;
                    tutorialFashion.handHint.gameObject.SetActive(false);
                    focusTutorial.transform.position =rightFillbar.transform.position;
                    
                    focusTutorial.gameObject.SetActive(true);
                    StartCoroutine(focusTutorial.ScaleFocus(2, 3.35f)) ;
                    tween =  DOVirtual.DelayedCall(2.5f, () =>
                    {
                        focusTutorial.gameObject.SetActive(false);
                        for (int i = 0; i < LogicFashion.instance.listFashionItemsGame.Count; i++)
                        {
                            LogicFashion.instance.listFashionItemsGame[i].icon.raycastTarget = true;
                        }
                    });
                }
            }*/
        }

        private void OnClickNextButton()
        {
            LogicFashion.instance.model.SaveFashion();
            if (!tutorialFashion.isDoneTutFashion)
            {
                PlayerPrefs.SetInt("isDoneTutFashion", 1);
                tutorialFashion.isDoneTutFashion = true;
            }

            tween.Kill();
            DOTween.KillAll();
            LogicFashion.instance.bubbleHint.transform.DOKill();
            LogicFashion.instance.emotionBoxChat.transform.DOKill();
            
            ManagerSceneDuck.ins.LoadScene(SceneGame.sceneMakeup);
        }
        private void HandleTabSelected(object obj)
        {
            int tabIndex = (int)obj;
            for (int i = 0; i < tabsFashion.Count; i++)
            {
                if (i == tabIndex)
                {
                    tabsFashion[i].gameObject.SetActive(true);
                }
                else
                {
                    tabFashionButton[i].DisableSelectedImage();
                    tabsFashion[i].gameObject.SetActive(false);
                }
            }
            /*if (!tutorialFashion.isDoneTutFashion)
            {
                if (tabIndex == 2)
                {
                    tutorialFashion.isDoneTopHint = true;
                    tutorialFashion.handHint.transform.position = LogicFashion.instance.listFashionItemsGot[1].transform.position ;
                    Debug.Log("??????? ???");
                }
            }*/
        }

        private void SetIconThemeImage()
        {
            switch (GlobalData.currentTheme)
            {
                case THEME.ARCADE:
                    iconThemeImage.sprite = Resources.Load<Sprite>("Art/UI_store/game_icon");
                    tabsFashion[0].gameObject.SetActive(true);
                    tabFashionButton[0].selectedImage.gameObject.SetActive(true);
                    tabFashionButton[0].icon.gameObject.SetActive(true);
                    break;
                case THEME.FASHION: 
                    iconThemeImage.sprite = Resources.Load<Sprite>("Art/UI_store/fashion_icon");
                    tabsFashion[1].gameObject.SetActive(true);
                    tabFashionButton[1].selectedImage.gameObject.SetActive(true);
                    tabFashionButton[1].icon.gameObject.SetActive(true);
                    //iconThemeImage.color = new Color32(255, 255, 255, 0);
                    break;
                case THEME.RESTAURANT:
                    iconThemeImage.sprite = Resources.Load<Sprite>("Art/UI_store/restaurant_icon");
                    tabsFashion[2].gameObject.SetActive(true);
                    tabFashionButton[2].selectedImage.gameObject.SetActive(true);
                    tabFashionButton[2].icon.gameObject.SetActive(true);
                    break;
                case THEME.SCHOOL:
                    iconThemeImage.sprite = Resources.Load<Sprite>("Art/UI_store/school_icon");
                    tabsFashion[3].gameObject.SetActive(true);
                    tabFashionButton[3].selectedImage.gameObject.SetActive(true);
                    tabFashionButton[3].icon.gameObject.SetActive(true);
                    break;
                case THEME.BAR:
                    iconThemeImage.sprite = Resources.Load<Sprite>("Art/UI_store/coffee_icon");
                    tabsFashion[4].gameObject.SetActive(true);
                    tabFashionButton[4].selectedImage.gameObject.SetActive(true);
                    tabFashionButton[4].icon.gameObject.SetActive(true);
                    break;
                case THEME.PARK:
                    iconThemeImage.sprite = Resources.Load<Sprite>("Art/UI_store/park_icon");
                    tabsFashion[5].gameObject.SetActive(true);
                    tabFashionButton[5].selectedImage.gameObject.SetActive(true);
                    tabFashionButton[5].icon.gameObject.SetActive(true);
                    break;
            }
        }

        public float currentValue;


        float total;

        [Header("Bonus Text")]
        public TextMeshProUGUI bonusText;
        public GameObject bonusObject;
        
        public void UpdateFillImage(float currentVal)
        {
            fillImage.DOKill();


            if (currentVal >= 4)
            {
                Duck.PlayParticle(heartFlyParticle);
            }
            else
            {
                heartFlyParticle.Stop();
            }
            fillImage.DOFillAmount(currentVal/4, 0.5f).SetEase(Ease.Linear);
        }
        public void ShowBonus()
        {
            bonusObject.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        }
        public void ShowPanelGetClothesPanel(Sprite sprite, float price, int id)
        {
            buyButton.interactable = true;
            idTemp = id;
            coinTemp = price;
            priceText.text = "Buy " + price.ToString();
            iconImage.sprite = sprite;
            panelGetClothe.gameObject.SetActive(true);
            PopUpPanelGetClothes.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).From(0);
            PopUpPanelGetClothes.GetComponent<CanvasGroup>().DOFade(1, 0.3f).SetEase(Ease.OutBack).From(0).OnComplete(
                () =>
                {
                    for (int i = 0; i < popUpPanelGetClothesElement.Count; i++)
                    {
                        var a = i;
                        popUpPanelGetClothesElement[a].transform.DOScale(1, 0.2f).SetEase(Ease.OutBack).From(0)
                            .SetDelay(a * 0.07f);
                    }
                });
        }

        public void DisableTabsInTut()
        {
            for (int i = 0; i < tabFashionButton.Count; i++)
            {
                if (tabFashionButton[i].id != 2)
                {
                    tabFashionButton[i].GetComponent<Button>().interactable = false;
                }
            }
        }
    }
}