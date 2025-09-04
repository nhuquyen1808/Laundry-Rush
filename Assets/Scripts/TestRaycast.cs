using System;
using System.Collections;
using System.Collections.Generic;
using DevDuck;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int tileType;
}
public class TestRaycast : MonoBehaviour
{
    [FormerlySerializedAs("testButton")] public Button GetCoin;
    
    public EffectGetCoin effectGetCoin;
    private void Awake()
    {
        GetCoin.onClick.AddListener(GetCoinButtonOnClick);
    }

    private void GetCoinButtonOnClick()
    {
        effectGetCoin.GetCoin(15,10,ShowLog);
    }

    public void ShowLog()
    {
        Debug.Log("ShowLog");
    }
}
