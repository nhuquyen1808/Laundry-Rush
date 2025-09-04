using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DevDuck
{
    [CreateAssetMenu(fileName = "DataGame1", menuName = "ScriptableObjects/SpawnListItemGame1", order = 1)]

   
    public class SpawnListItemGame1 : ScriptableObject
    {
       public  List<ItemGame1List> itemGame1List = new List<ItemGame1List>();
    }
    [Serializable]
    public class ItemGame1List
    {
        public List<itemGame1> items = new List<itemGame1>();
    }
}
