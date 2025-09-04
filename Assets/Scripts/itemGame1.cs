using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
namespace DevDuck
{
    public enum Type
    {
        HAT,FACE,NECK,TOP,BOT,OVERALL,SHOES,HAND
        
    }
    public class itemGame1 : MonoBehaviour
    {
        public Type type;
        public SpriteRenderer spr;
        public bool isOverSize;
        private void Start()
        {
            spr = GetComponent<SpriteRenderer>();
        }

        public Sprite GetSprite()
        {
            return spr.sprite;
        }
    }
}
