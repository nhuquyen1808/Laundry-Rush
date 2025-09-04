using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DevDuck
{
    [Serializable]
    public class DataModel
    {
        public int id;
        public FashionType type;

        public DataModel(int id, FashionType type)
        {
            this.id = id;
            this.type = type;
        }
    }

    [Serializable]
    public class DataModelWrapped
    {
        public List<DataModel> data = new List<DataModel>();
    }

    public class Model : MonoBehaviour
    {
        public DataModelWrapped dataModelWrapped;
        public List<ModelFashion> listModelFashion = new List<ModelFashion>();
        public Sprite defaultTopSprite, defaultBotSprite;
        [SerializeField] Vector3 defaultTopPos, defaultBotPos;
        public GameObject defaultTopObj, defaultBotObj, defaultHairObj;

        public void UnequipFashion(FashionType fashionType)
        {
            for (int i = 0; i < listModelFashion.Count; i++)
            {
                if (fashionType == FashionType.FASHION_BOT)
                {
                    listModelFashion[i].itemFashionInit.gameObject.SetActive(false);
                }
            }
        }

        public void SaveFashion()
        {
            dataModelWrapped.data.Clear();
            for (int i = 0; i < listModelFashion.Count; i++)
            {
                if (listModelFashion[i].id > 0)
                {
                    dataModelWrapped.data.Add(new DataModel(listModelFashion[i].id, listModelFashion[i].fashionType));
                }
            }

            string json = JsonUtility.ToJson(dataModelWrapped, true);
            PlayerPrefs.SetString("Fashion", json);
        }

        public void LoadAndInitFashion()
        {
            dataModelWrapped.data.Clear();
            string json = PlayerPrefs.GetString("Fashion", "");
            if (!string.IsNullOrEmpty(json))
            {
                dataModelWrapped = JsonUtility.FromJson<DataModelWrapped>(json);
            }

            InitItemSaved();
        }

        public void InitItemSaved()
        {
            for (int i = 0; i < dataModelWrapped.data.Count; i++)
            {
                for (int j = 0; j < listModelFashion.Count; j++)
                {
                    if (listModelFashion[j].fashionType == dataModelWrapped.data[i].type)
                    {
                        int id = dataModelWrapped.data[i].id;

                        ItemFashionInit fashionItemLoad = Resources.Load<ItemFashionInit>("Data/fashion_" + id);
                        ItemFashionInit fashionItem = Instantiate(fashionItemLoad,
                            listModelFashion[j].transform.position, Quaternion.identity, listModelFashion[j].transform);
                        fashionItem.transform.localPosition = fashionItem.positionOnModel;

                        fashionItem.id = id;
                        listModelFashion[j].itemFashionInit = fashionItem;
                        listModelFashion[j].id = id;
                        if (listModelFashion[j].fashionType == FashionType.FASHION_HAIR)
                        {
                            defaultHairObj.gameObject.SetActive(false);
                        }

                        if (listModelFashion[j].fashionType == FashionType.FASHION_TOP)
                        {
                            defaultTopObj.gameObject.SetActive(false);
                        }

                        if (listModelFashion[j].fashionType == FashionType.FASHION_BOT)
                        {
                            defaultBotObj.gameObject.SetActive(false);
                        }

                        DOVirtual.DelayedCall(.5F, (() =>
                        {
                            if (LogicFashion.instance != null)
                            {
                                LogicFashion.instance.CheckShowNextButton();
                                for (int K = 0; K < LogicFashion.instance.listFashionItemsGame.Count; K++)
                                {
                                    if (LogicFashion.instance.listFashionItemsGame[K].id == id)
                                    {
                                        LogicFashion.instance.listFashionItemsGame[K].adIcon.gameObject
                                            .SetActive(false);
                                        LogicFashion.instance.listFashionItemsGame[K].isAds = false;
                                        LogicFashion.instance.listFashionItemsGame[K].OnItemSelected(id);
                                        LogicFashion.instance.listFashionItemsGame[K].itemFashionGame.isOnModel = true;
                                    }
                                }
                            }
                        }));
                    }
                }
            }
        }

        public bool CheckFashion()
        {
            bool hasTop = false;
            bool hasBot = false;
            for (int i = 0; i < listModelFashion.Count; i++)
            {
                var item = listModelFashion[i];
                if (item.fashionType == FashionType.FASHION_OVERALL && item.id > 0)
                {
                    hasTop = true;
                    hasBot = true;
                }

                if (item.fashionType == FashionType.FASHION_TOP && item.id > 0)
                    hasTop = true;
                if (item.fashionType == FashionType.FASHION_BOT && item.id > 0)
                    hasBot = true;
            }

            if (hasTop && hasBot)
                return true;
            return false;
        }

        public int itemLacked()
        {
            int a = 0;
            List<int> idxItems = new List<int>();
            bool isTopHad = false;
            bool isBotHad = false;
            bool isOverall = false;
            if (listModelFashion.Count > 0)
            {
                for (int i = 0; i < listModelFashion.Count; i++)
                {
                    if (listModelFashion[i].fashionType == FashionType.FASHION_HAIR)
                    {
                        if (listModelFashion[i].id < 1)
                        {
                            idxItems.Add(2);
                        }
                    }
                    else if (listModelFashion[i].fashionType == FashionType.FASHION_TOP)
                    {
                        if (listModelFashion[i].id < 1)
                        {
                            idxItems.Add(7);
                            isTopHad = false;
                        }
                        else
                        {
                            isTopHad = true;
                        }
                    }
                    else if (listModelFashion[i].fashionType == FashionType.FASHION_BOT)
                    {
                        if (listModelFashion[i].id < 1)
                        {
                            idxItems.Add(11);
                            isBotHad = false;
                        }
                        else
                        {
                            isBotHad = true;
                        }
                    }
                    else if (listModelFashion[i].fashionType == FashionType.FASHION_SHOES)
                    {
                        if (listModelFashion[i].id < 1)
                        {
                            idxItems.Add(13);
                        }
                    }
                    else if (listModelFashion[i].fashionType == FashionType.FASHION_OVERALL)
                    {
                        if (listModelFashion[i].id < 1)
                        {
                            idxItems.Add(8);
                        }
                        else
                        {
                            isOverall = true;
                        }
                    }
                }

                if (isOverall)
                {
                    if (idxItems.Contains(7))
                    {
                        idxItems.Remove(7);
                    }

                    if (idxItems.Contains(11))
                    {
                        idxItems.Remove(11);
                    }
                }

                if (isTopHad && isBotHad)
                {
                    if (idxItems.Contains(8))
                    {
                        idxItems.Remove(8);
                    }
                }

                if (idxItems.Count > 0)
                    a = idxItems[Random.Range(0, idxItems.Count)];
            }

            idxItems.Clear();
            return a;
        }

        public int ItemOnModel()
        {
            int amount = 0;
            for (int i = 0; i < listModelFashion.Count; i++)
            {
                if (listModelFashion[i].fashionType == FashionType.FASHION_HAIR && listModelFashion[i].id > 0)
                    amount++;
                if (listModelFashion[i].fashionType == FashionType.FASHION_TOP && listModelFashion[i].id > 0)
                    amount++;
                if (listModelFashion[i].fashionType == FashionType.FASHION_BOT && listModelFashion[i].id > 0)
                    amount++;
                if (listModelFashion[i].fashionType == FashionType.FASHION_SHOES && listModelFashion[i].id > 0)
                    amount++;
                if (listModelFashion[i].fashionType == FashionType.FASHION_OVERALL && listModelFashion[i].id > 0)
                    amount += 2;
            }
            return amount;
        }
    }
}