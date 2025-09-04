using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DevDuck
{
    public class Food : MonoBehaviour
    {
        public int id;
        public SpriteRenderer spr;

        private BoxCollider2D _box;

        public BoxCollider2D BoxCollider2D
        {
            get
            {
                if (_box == null)
                {
                    _box = GetComponent<BoxCollider2D>();
                    
                }
                return _box;
            }
        }

        private void Start()
        {
            BoxCollider2D.enabled = false;
        }

        public void SetFoodSprite(int foodId)
        {
            spr.sprite = Resources.Load<Sprite>($"Art/game5/easy_mode/food_{foodId}");
            // Debug.Log($"Set food sprite to {foodId}");
        }

        public void ShowFailFood()
        {
            Vector3 pos = this.transform.position;
            spr.DOColor(Color.red, 0.2f).OnComplete(() => { spr.DOColor(Color.white, 0.2f); });
            transform.DOMove(pos + new Vector3(0, 0.4f, 0), .2f).OnComplete(() =>
            {
                LogicUiGame6.instance.PlaySmokeEffect();
                transform.DOMove(pos, 0.2f).OnComplete(() => { Destroy(gameObject); });
            });
        }
    }
}