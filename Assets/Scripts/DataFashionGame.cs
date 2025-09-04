    using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DevDuck
{
   

    public class DataFashionGame : MonoBehaviour
    {
        /*public List<DataItems> DataItemsGame = new List<DataItems>();
        public List<FashionItem> listFashionItems = new List<FashionItem>();
        public static DataFashionGame Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void SaveJsonDataFashionGame()
        {
            for (int i = 0; i < listFashionItems.Count; i++)
            {
                DataItemsGame.Add(new DataItems(listFashionItems[i].id, listFashionItems[i].fashionType,listFashionItems[i].theme,
                    listFashionItems[i].rate,listFashionItems[i].price,listFashionItems[i].isHasIcon));
            }
            ListDataItem data = new ListDataItem();
            data.items = DataItemsGame;
            Debug.Log("DataFashionGame saved  " + DataItemsGame.Count);
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(GameLocalFile.FILE_DATAITEM, json);
        }

        public void UpdateJsonDataFashionGame()
        {
            DataItemsGame.Clear();
            /*for (int i = 0; i < listFashionItems.Count; i++)
            {
                DataItemsGame.Add(new DataItems(listFashionItems[i].id, listFashionItems[i].fashionType,
                    listFashionItems[i].transform.localPosition, listFashionItems[i].theme));
            }#1#
        }

        public void LoadJsonDataFashionGame()
        {
            ListDataItem result = new ListDataItem();
            string data = "";
#if UNITY_EDITOR
            data = File.ReadAllText(GameLocalFile.FILE_DATAITEM);
#else
            data = Resources.Load<TextAsset>(GameLocalFile.FILE_DATAITEM).ToString();
#endif
            result = JsonUtility.FromJson<ListDataItem>(data);
            DataItemsGame = result.items;
        }*/
    }
}