using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Data
{
    public int timer;
    public int target;
}
public class LevelDataGame : MonoBehaviour
{
    public List<Data> listLevelData = new List<Data> ();
}
