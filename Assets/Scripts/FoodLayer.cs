using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public enum TOPPING_TYPE
    {
        TOMATO,CHEESE,HARMBURGER,MEAT
    }
    public class FoodLayer : MonoBehaviour
    {
        public int layer;
        public TOPPING_TYPE type;
        /*[SerializeField]*/ private SpriteRenderer spr;

        private void Start()
        {
            spr = GetComponent<SpriteRenderer>();
        }

        public void SetSpriteFoodLayer(int index)
        {
            spr.sprite = Resources.Load<Sprite>($"Art/game5/hard_mode/topping_{index+2}");
                /*switch (index)
                {
                    case 0:
                        spr.sprite = Resources.Load<Sprite>($"Art/game5/hard_mode/meat");
                        type = TOPPING_TYPE.MEAT;
                        break;
                    case 1:
                        spr.sprite = Resources.Load<Sprite>($"Art/game5/hard_mode/tomato");
                        type = TOPPING_TYPE.TOMATO;
                        break;
                    case 2:
                        spr.sprite = Resources.Load<Sprite>($"Art/game5/hard_mode/cheese");                    
                        type = TOPPING_TYPE.CHEESE;
                        break;
                    case 3:
                        spr.sprite = Resources.Load<Sprite>($"Art/game5/hard_mode/harmburger");
                        type = TOPPING_TYPE.HARMBURGER;
                        break;
                    default:
                        spr.sprite = null;
                        break;
            }*/
        }
    }
}
