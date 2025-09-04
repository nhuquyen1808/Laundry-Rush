using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class BoxChatOrderFood : MonoBehaviour
    {
        private int idFood1 = -1, idFood2 = -1;
        [SerializeField] private SpriteRenderer _spriteRendererFood1, _spriteRendererFood2;
        /*private void Start()
        {
            SetIdFood(2,-1);
            SetSpriteFood();
        }*/
        public void SetIdFood(int idFood1, int idFood2)
        {
            this.idFood1 = idFood1;
            this.idFood2 = idFood2;
        }
        public void SetSpriteFood()
        {
            if (idFood1 != -1 && _spriteRendererFood1 != null)
            {
                _spriteRendererFood1.sprite = Resources.Load<Sprite>($"Art/game5/easy_mode/food_{idFood1}");
            }
            if (idFood2 != -1 && _spriteRendererFood2 != null)
            {
                _spriteRendererFood2.sprite = Resources.Load<Sprite>($"Art/game5/easy_mode/food_{idFood2}");
            }
        }
    }
}
