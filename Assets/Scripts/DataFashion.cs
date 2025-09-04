using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ntDev;
using UnityEngine;

namespace DevDuck
{
    [Serializable]
    public class DataFashion
    {
        public string Theme;
        public int ID;
        public int Price;
        public FashionType Type;
        public SHADOW_TYPE Shadow;
        public string isAds;
        static List<DataFashion> listData;

        public async static Task<List<DataFashion>> GetListData()
        {
            
            if (listData == null || listData.Count == 0)
            {
                listData = new List<DataFashion>();
                List<DataFashion> list =
                    JsonHelper.GetJsonList<DataFashion>((await ntDev.ManagerAsset.LoadAssetAsync<TextAsset>("DataFashion")).text);
                listData.AddRange(list);
            }

            return listData;
        }

        public async static Task<DataFashion> GetData(int id)
        {
            List<DataFashion> list = await GetListData();
            foreach (DataFashion dataFashion in list)
            {
                if(dataFashion.ID == id)
                    return dataFashion;
            }
            return null;
        }
    }
}