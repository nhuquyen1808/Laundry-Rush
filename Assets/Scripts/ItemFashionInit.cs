using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public enum FashionType
    {
        FASHION_HAT,
        FASHION_HAIR,
        FASHION_EARS,
        FASHION_FACE,
        FASHION_NECK,
        FASHION_TOP,
        FASHION_OVERALL,
        FASHION_HAND,
        FASHION_BOT,
        FASHION_SHOES,
        FASHION_ACCESSORY,

    }
        
    public class ItemFashionInit : MonoBehaviour
    {
        public int id;
       // public string name;
        public FashionType Type;
        public Vector3 positionOnModel;
        private SpriteRenderer spriteRenderer;
        public LogicShop LogicShop;
        public SpriteRenderer SpriteRenderer
        {
            get
            {
                if (spriteRenderer == null)
                {
                    spriteRenderer = GetComponent<SpriteRenderer>();
                    if (spriteRenderer == null)
                    {
                        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                    }
                }
                return spriteRenderer;
            }
        }

        public void ClearAllItemSprites()
        {

            for (int i = 0; i <  LogicShop.modelFashions.Count; i++)
            {
                LogicShop.modelFashions[i].sprite = null;
            }
        }
    }
}
