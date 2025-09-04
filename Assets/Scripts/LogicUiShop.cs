using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DevDuck
{
    public class LogicUiShop : MonoBehaviour
    {
        public static LogicUiShop instance;
        public List<GameObject> tabsFashion = new List<GameObject>();
        public List<TabFashionButton> tabFashionButton = new List<TabFashionButton>();
        public Image table;
        public Button returnButton;

        [Header("text index : ")] public TextMeshProUGUI ideaIndexText;
        public TextMeshProUGUI energyIndexText;
        public TextMeshProUGUI powerIndexText;
        public TextMeshProUGUI bookIndexText;
        public TextMeshProUGUI gameIndexText;
        public TextMeshProUGUI cookText;
        
         public TutorialShop tutorialShop;
        private void Awake()
        {
            instance = this;
            returnButton.onClick.AddListener(OnReturnButtonClicked);
        }

        void Start()
        {
            ShowTable();
            Observer.AddObserver(EventAction.EVENT_TAB_SELECTED, HandleTabSelected);
        }

        private void OnReturnButtonClicked()
        {
            for (int i = 0; i < ShowElementsShop.instance.idItemsUserLoaded.Count; i++)
            {
                for (int j = 0; j < LogicShop.instance.model.listModelFashion.Count; j++)
                {
                    if (!ShowElementsShop.instance.idItemsUserLoaded.Contains(LogicShop.instance.model
                            .listModelFashion[j].id))
                    {
                        LogicShop.instance.model
                            .listModelFashion[j].id = -1;
                    }
                }
            }
            
            LogicShop.instance.model.SaveFashion();
            if (!tutorialShop.isDoneTutShop)
            {
                tutorialShop.handHintReturn.gameObject.SetActive(false);
                PlayerPrefs.SetInt("isDoneTutShop", 1);
            }
            int index = PlayerPrefs.GetInt(  PlayerPrefsManager.AREA_UNLOCK);
            PlayerPrefs.SetInt(  PlayerPrefsManager.AREA_UNLOCK, index+1);
            PlayerPrefs.SetInt(PlayerPrefsManager.SHOPPING_DONE, 1);
            DOVirtual.DelayedCall(0.35f, () =>
                {
                    DOTween.KillAll();
                    ManagerSceneDuck.ins.LoadScene("SceneMenu");
                })
           ;
        }
        private void OnDestroy()
        {
            Observer.RemoveObserver(EventAction.EVENT_TAB_SELECTED, HandleTabSelected);
        }
        private void ShowTable()
        {
            table.DOFade(1f, .4f).From(0).OnComplete(() =>
            {
                for (int i = 0; i < tabFashionButton.Count; i++)
                {
                    tabFashionButton[i].transform.DOScale(1, 0.3f).From(0).SetDelay(.1f * i).SetEase(Ease.OutBack);
                }
            });
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
        }

        public void UpdateUiIndex(int idea, int energy, int power, int book, int game, int cook)
        {
            ideaIndexText.text = idea.ToString();
            energyIndexText.text = energy.ToString();
            powerIndexText.text = power.ToString();
            bookIndexText.text = book.ToString();
            gameIndexText.text = game.ToString();
            cookText.text = cook.ToString();
        }
    }
}