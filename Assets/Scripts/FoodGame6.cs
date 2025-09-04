using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DevDuck
{
    public class FoodGame6 : MonoBehaviour
    {
        private SpriteRenderer spr;

        public SpriteRenderer Spr
        {
            get
            {
                if (spr == null)
                {
                   // spr = GetComponent<SpriteRenderer>() ?? gameObject.AddComponent<SpriteRenderer>();
                   spr = GetComponent<SpriteRenderer>();
                   if (spr == null)
                   {
                       spr = gameObject.AddComponent<SpriteRenderer>();
                   }
                }
                return spr;
            }
           // set { spr = value; }
        }
        
        
        public int id;
        public TOPPING_TYPE type;

        public void SetFoodSprite(int toppingId)
        {
            switch (toppingId)
            {
                case 0:
                    Spr.sprite = Resources.Load<Sprite>("Art/game5/hard_mode/meat");
                    type = TOPPING_TYPE.MEAT;
                    break;
                case 1:
                    Spr.sprite = Resources.Load<Sprite>("Art/game5/hard_mode/tomato");
                    type = TOPPING_TYPE.TOMATO;
                    break;
                case 2:
                    Spr.sprite = Resources.Load<Sprite>("Art/game5/hard_mode/cheese");                    
                    type = TOPPING_TYPE.CHEESE;
                    break;
                case 3:
                    Spr.sprite = Resources.Load<Sprite>("Art/game5/hard_mode/harmburger");
                    type = TOPPING_TYPE.HARMBURGER;
                    break;
                default:
                    Spr.sprite = null;
                    break;
            }
        }
        public void ToppingClicked(GameObject des)
        {
            this.transform.DOJump(des.transform.position, 0.5f, 1, .5f).OnComplete(() =>
            {
                LogicGame5.ins.PlayParticleHitPlateEffect();
                this.gameObject.GetComponent<PooledObject>().ReturnToPool();
            });
        }
        public void ToppingAppear()
        {
           float yPos = this.gameObject.transform.position.y;
            gameObject.transform.localScale = Vector2.zero;
            gameObject.transform.DOScale(new Vector3(0.7f, 0.7f, 1), 0.3f);
            gameObject.transform.DOMoveY(yPos + .5f, 0.15f).OnComplete(() =>
            {
                gameObject.transform.DOMoveY(yPos , 0.15f);
            });
        }
    }
}