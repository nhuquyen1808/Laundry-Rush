using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using ntDev;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace DevDuck
{
    public class ShowElementsShop : MonoBehaviour
    {
        public GameObject clothesHanger, podium;
        public CanvasGroup leftUiGroup;
        public GameObject Mask;

        public ItemShop item;
        public List<DataFashion> dataFashions = new List<DataFashion>();
        public GameObject nPaparItem;
        public Sprite sShadow, mShadow, lShadow;
        [Header("Tabs: ")] public GameObject hairTab;
        public GameObject topTab, bottomTab, overAllTab, acessoryTab, shoesTab;
        public List<int> idItemsUserLoaded = new List<int>();
        public static ShowElementsShop instance;
        public TutorialShop tutorialShop;

        private void Awake()
        {
            instance = this;
        }

        public void InitData()
        {
            LoadData();
            InitItemShop();
            ShowElements();
        }

        private void LoadData()
        {
            SaveGame.LoadArrayFromPlayerPrefs();
            idItemsUserLoaded = SaveGame.ArraySaved.ToList();
        }

        public async void InitItemShop()
        {
            dataFashions = await DataFashion.GetListData();
            Addressables.InitializeAsync();
            // ManagerAsset.PreLoadAsset("IconInShop");
            StartCoroutine(initItem());
        }

        IEnumerator initItem()
        {
            yield return new WaitForSeconds(.5f);
            for (int i = 0; i < dataFashions.Count; i++)
            {
                if (dataFashions[i].Price > 0)
                {
                    yield return new WaitForEndOfFrame();
                    ItemShop newItem = Instantiate(item, nPaparItem.transform).GetComponent<ItemShop>();
                    LogicShop.instance.listItemShops.Add(newItem);
                    newItem.id = dataFashions[i].ID;
                    newItem.transform.localScale = Vector3.zero;
                    newItem.transform.DOScale(1, 0.3f).SetDelay(0.5f);
                    SetIcon(newItem.iconImage, newItem.id);
                    newItem.price = dataFashions[i].Price;
                    newItem.priceText.text = dataFashions[i].Price.ToString();
                    SetShadow(newItem.shadowType, newItem);
                    newItem.fashionType = dataFashions[i].Type;
                    SetTab(newItem.fashionType, newItem);
                    newItem.shadowType = dataFashions[i].Shadow;

                    for (int j = 0; j < idItemsUserLoaded.Count; j++)
                    {
                        if (idItemsUserLoaded[j] == newItem.id)
                        {
                            newItem.ChangeState();
                        }
                    }
                }
            }
        }

        private async void SetIcon(Image image, int id)
        {
            if (ManagerAsset.IsExist($"icon_{id}", (typeof(Sprite))))
            {
                image.sprite = await ManagerAsset.LoadAssetAsync<Sprite>($"icon_{id}");
            }
        }

        private void SetTab(FashionType fashionType, ItemShop itemShop)
        {
            switch (fashionType)
            {
                case FashionType.FASHION_HAIR:
                    itemShop.transform.SetParent(hairTab.transform);
                    break;
                case FashionType.FASHION_TOP:
                    itemShop.transform.SetParent(topTab.transform);
                    break;
                case FashionType.FASHION_BOT:
                    itemShop.transform.SetParent(bottomTab.transform);
                    break;
                case FashionType.FASHION_OVERALL:
                    itemShop.transform.SetParent(overAllTab.transform);
                    break;
                case FashionType.FASHION_SHOES:
                    itemShop.transform.SetParent(shoesTab.transform);
                    break;
                default:
                    itemShop.transform.SetParent(acessoryTab.transform);
                    break;
            }
        }

        private void SetShadow(SHADOW_TYPE shadowType, ItemShop item)
        {
            switch (shadowType)
            {
                case SHADOW_TYPE.S:
                    item.shadowImage.sprite = sShadow;
                    break;
                case SHADOW_TYPE.M:
                    item.shadowImage.sprite = mShadow;
                    break;
                case SHADOW_TYPE.L:
                    item.shadowImage.sprite = lShadow;
                    break;
                default:
                    break;
            }
        }

        public void ShowElements()
        {
            clothesHanger.transform.DOMoveX(5, 1f).SetEase(Ease.OutBack).SetDelay(0.5f);
            podium.transform.DOMoveY(-0.91f, 1f).SetEase(Ease.OutBack).SetDelay(0.5f);
            leftUiGroup.DOFade(1, 0.3f).From(0).SetDelay(0.5f).OnComplete(() => { Mask.SetActive(false); });
            if (!tutorialShop.isDoneTutShop)
            {
                StartCoroutine(DelayShowHand());
            }
        }

        IEnumerator DelayShowHand()
        {
            yield return new WaitForSeconds(2f);
            for (int i = 0; i < LogicUiShop.instance.tabFashionButton.Count; i++)
            {
                if (LogicUiShop.instance.tabFashionButton[i].id != 0)
                {
                    LogicUiShop.instance.tabFashionButton[i].GetComponent<Button>().interactable = false;
                }
            }

            if (!tutorialShop.isDoneTutShop)
            {
                tutorialShop.EnableHandHintBuy(hairTab.transform.GetChild(0).transform.position );
            }
        }
    }
}