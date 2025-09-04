using System;
using System.Collections.Generic;

// by nt.Dev93
namespace ntDev
{
    public class RatioObject
    {
        public int ID;
        public float Ratio;
        public int Win;

        public bool Calculate()
        {
            bool b = Ez.Random(0, 1f) < Ratio;
            if (b) ++Win;
            return b;
        }
    }

    [Serializable]
    public class BalanceListRatio
    {
        // public List<int> ListResult = new List<int>();

        // public void Init(float[] arr)
        // {
        //     for (int i = 0; i < arr.Length; ++i)
        //     {
        //         for (int j = 0; j < (int)(arr[i] * 1000); ++j)
        //             ListResult.Add(i);
        //     }
        // }

        // public int Calculate()
        // {
        //     int n = Ez.Random(0, ListResult.Count);
        //     int result = ListResult[n];
        //     ListResult.RemoveAt(n);
        //     return result;
        // }

        public List<RatioObject> ListRatioObject = new List<RatioObject>();
        public List<float> ListRatio = new List<float>();
        public int Total;

        public void Init(float[] arr)
        {
            List<float> list = new List<float>();
            list.AddRange(arr);
            Init(list);
        }

        public void Init(List<float> list)
        {
            ListRatio.Clear();
            ListRatio.AddRange(list);
            ListRatioObject.Clear();
            for (int i = 0; i < list.Count; ++i)
                ListRatioObject.Add(new RatioObject { ID = i, Ratio = list[i], Win = 0 });
            Total = 0;
        }

        public int Calculate()
        {
            for (int i = 0; i < ListRatioObject.Count - 1; ++i)
            {
                if (Total == 0 || ListRatioObject[i].Win / (float)Total < ListRatioObject[i].Ratio)
                {
                    if (ListRatioObject[i].Calculate())
                    {
                        ++Total;
                        return ListRatioObject[i].ID;
                    }
                }
            }

            int t = ListRatio.GetRandomWithRatio();
            ++ListRatioObject[t].Win;
            ++Total;
            return t;
        }
    }
}
