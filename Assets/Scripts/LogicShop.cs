using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using ntDev;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DevDuck
{
    public class LogicShop : MonoBehaviour
    {
        public static LogicShop instance;
        [HideInInspector] public List<ItemShop> listItemShops = new List<ItemShop>();
        public Model model;
        public GameObject topDefaultFashion;
        public GameObject botDefaultFashion;
        public GameObject hairDefaultFashion;
        public List<SpriteRenderer> modelFashions = new List<SpriteRenderer>();
        public List<int> idItemsUserGet = new List<int>();
        [HideInInspector] private int ideaIndex, energyIndex, powerIndex, bookIndex, gameIndex, cookIndex;
        public EffectInShop effectInShop;
        public TutorialShop tutorialShop;
        
        
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            Observer.AddObserver(EventAction.EVENT_BUY_ITEM, HandleBuyItem);
            Observer.AddObserver(EventAction.EVENT_TRY_ITEM, HandleTryItem);
            Observer.AddObserver(EventAction.EVENT_TRY_RELEASE_ITEM, HandleReleaseItem);
            ShowElementsShop.instance.InitData();
            model.LoadAndInitFashion();
        }

        private void HandleReleaseItem(object obj)
        {
            int idRelease = (int)obj;
            for (int i = 0; i < model.listModelFashion.Count; i++)
            {
                if (idRelease == model.listModelFashion[i].id)
                {
                    if (model.listModelFashion[i].fashionType == FashionType.FASHION_HAIR)
                    {
                        hairDefaultFashion.SetActive(true);
                    }

                    if (model.listModelFashion[i].fashionType == FashionType.FASHION_TOP)
                    {
                        topDefaultFashion.SetActive(true);
                    }

                    if (model.listModelFashion[i].fashionType == FashionType.FASHION_BOT)
                    {
                        botDefaultFashion.SetActive(true);
                    }

                    if (model.listModelFashion[i].fashionType == FashionType.FASHION_OVERALL)
                    {
                        topDefaultFashion.SetActive(true);
                        botDefaultFashion.SetActive(true);
                    }

                    model.listModelFashion[i].id = -1;
                    Destroy(model.listModelFashion[i].itemFashionInit.gameObject);
                }
            }

        }
        private void OnDestroy()
        {
            Observer.RemoveObserver(EventAction.EVENT_BUY_ITEM, HandleBuyItem);
            Observer.RemoveObserver(EventAction.EVENT_TRY_ITEM, HandleTryItem);
            Observer.RemoveObserver(EventAction.EVENT_TRY_RELEASE_ITEM, HandleReleaseItem);
        }

        private void HandleTryItem(object obj)
        {
            AudioManager.instance.PlaySound("EquipClothe");
            int id = (int)obj;
            int tempId = -1;
            foreach (ItemShop item in listItemShops)
            {
                if (item.id == id)
                {
                    for (int j = 0; j < model.listModelFashion.Count; j++)
                    {
                        if (item.fashionType == model.listModelFashion[j].fashionType)
                        {
                            tempId = model.listModelFashion[j].id;
                        }
                    }
                }
            }
            foreach (ItemShop item in listItemShops)
            {
                if (item.id == tempId)
                {
                    item.isOnModel = false;
                    tempId = -1;
                }
            }
            for (int i = 0; i < listItemShops.Count; i++)
            {
                if (listItemShops[i].id == id)
                {
                    EquipItem(listItemShops[i]);
                }
            }
            
            effectInShop.PlaySparkle();
        }

        private void HandleBuyItem(object obj)
        {
            AudioManager.instance.PlaySound("EquipClothe");
            Debug.Log("User bought item then hide hand hint buy and show hand hint return" );
            int id = (int)obj;
            for (int i = 0; i < listItemShops.Count; i++)
            {
                if (listItemShops[i].id == id)
                {
                    float coin = PlayerPrefs.GetFloat(PlayerPrefsManager.Coin);
                    if (listItemShops[i].price <= coin)
                    {
                       // model.idUsedGetted.Add(listItemShops[i].id);
                        Observer.Notify(EventAction.EVENT_UPDATE_COIN, listItemShops[i].price);
                        EquipItem(listItemShops[i]);
                        SaveGame.AddElementToArray(listItemShops[i].id);
                        listItemShops[i].ChangeState();
                        ShowElementsShop.instance.idItemsUserLoaded.Add(listItemShops[i].id);
                    }
                }
            }
            effectInShop.PlaySparkle();
            if (!tutorialShop.isDoneTutShop)
            {
                tutorialShop.handHintBuy.gameObject.SetActive(false);
                tutorialShop.EnableHandHintReturn();
            }
        }

        public void EquipItem(ItemShop Item)
        {
            for (int i = 0; i < model.listModelFashion.Count; i++)
            {
                if (model.listModelFashion[i].fashionType == Item.fashionType)
                {
                    if (model.listModelFashion[i].itemFashionInit != null)
                    {
                        UnequipFashion(model.listModelFashion[i].id);
                        model.listModelFashion[i].itemFashionInit.gameObject.SetActive(false);
                        Destroy(model.listModelFashion[i].itemFashionInit.gameObject);
                    }

                    ItemFashionInit itemLoaded = Resources.Load<ItemFashionInit>($"Data/fashion_{Item.id}");
                    ItemFashionInit itemInit =
                        Instantiate(itemLoaded, transform.position, Quaternion.identity);
                    itemInit.transform.SetParent(model.listModelFashion[i].transform);
                    itemInit.transform.localPosition = itemLoaded.positionOnModel;
                    itemInit.transform.localScale = Vector3.one;
                    model.listModelFashion[i].id = Item.id;
                    model.listModelFashion[i].itemFashionInit = itemInit;
                    model.listModelFashion[i].Collider.enabled = true;
                    if (model.listModelFashion[i].fashionType == FashionType.FASHION_BOT)
                    {
                        botDefaultFashion.gameObject.SetActive(false);
                        for (int j = 0; j < model.listModelFashion.Count; j++)
                        {
                            if (model.listModelFashion[j].id != -1 &&
                                model.listModelFashion[j].fashionType == FashionType.FASHION_OVERALL)
                            {
                                model.listModelFashion[j].itemFashionInit.gameObject.SetActive(false);
                                model.listModelFashion[j].Collider.enabled = false;
                                model.listModelFashion[j].itemFashionInit = null;
                                UnequipFashion(model.listModelFashion[j].id);
                                model.listModelFashion[j].id = -1;
                                topDefaultFashion.gameObject.SetActive(true);
                            }
                        }
                    }

                    if (model.listModelFashion[i].fashionType == FashionType.FASHION_TOP)
                    {
                        topDefaultFashion.gameObject.SetActive(false);
                        for (int j = 0; j < model.listModelFashion.Count; j++)
                        {
                            if (model.listModelFashion[j].id != -1 &&
                                model.listModelFashion[j].fashionType == FashionType.FASHION_OVERALL)
                            {
                                model.listModelFashion[j].itemFashionInit.gameObject.SetActive(false);
                                model.listModelFashion[j].Collider.enabled = false;
                                model.listModelFashion[j].itemFashionInit = null;
                                UnequipFashion(model.listModelFashion[j].id);
                                model.listModelFashion[j].id = -1;
                                botDefaultFashion.gameObject.SetActive(true);
                                Debug.Log("Unequip overall 1");
                            }
                        }
                    }

                    if (model.listModelFashion[i].fashionType == FashionType.FASHION_HAIR)
                    {
                        hairDefaultFashion.gameObject.SetActive(false);
                    }

                    if (model.listModelFashion[i].fashionType == FashionType.FASHION_OVERALL)
                    {
                        topDefaultFashion.gameObject.SetActive(false);
                        botDefaultFashion.gameObject.SetActive(false);
                        for (int j = 0; j < model.listModelFashion.Count; j++)
                        {
                            if (model.listModelFashion[j].id != -1 &&
                                model.listModelFashion[j].fashionType == FashionType.FASHION_TOP)
                            {
                                model.listModelFashion[j].itemFashionInit.gameObject.SetActive(false);
                                model.listModelFashion[j].Collider.enabled = false;
                                model.listModelFashion[j].itemFashionInit = null;
                                UnequipFashion(model.listModelFashion[j].id);
                                model.listModelFashion[j].id = -1;
                                //  botDefaultBotFashion.gameObject.SetActive(true);
                            }

                            if (model.listModelFashion[j].id != -1 &&
                                model.listModelFashion[j].fashionType == FashionType.FASHION_BOT)
                            {
                                model.listModelFashion[j].itemFashionInit.gameObject.SetActive(false);
                                model.listModelFashion[j].Collider.enabled = false;
                                model.listModelFashion[j].itemFashionInit = null;
                                UnequipFashion(model.listModelFashion[j].id);
                                model.listModelFashion[j].id = -1;
                                //   botDefaultBotFashion.gameObject.SetActive(true);
                            }
                        }
                    }
                }
            }
        }

        public void UnequipFashion(int id)
        {
            for (int i = 0; i < listItemShops.Count; i++)
            {
                if (id == listItemShops[i].id)
                {
                    listItemShops[i].OnItemReleased();
                }
            }
        }
    }
}