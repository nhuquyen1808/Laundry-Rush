using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    [Serializable]
    public class foodData
    {
        public int id;
        public string name;
        public string description;
    }
    public class DataFoodGame10 : MonoBehaviour
    {
        public List<foodData> dataGame10 = new List<foodData>();
    }
}
