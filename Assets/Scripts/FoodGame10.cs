using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace DevDuck
{
    public class FoodGame10 : MonoBehaviour
    {
        public int id;
        public TextMeshPro name;
        public TextMeshPro description;
        public SpriteRenderer spriteSelected;
        public SpriteRenderer foodSpr;
        private SpriteRenderer _spriteRenderer;
        public SpriteRenderer SpriteRenderer
        {
            get
            {
                if (_spriteRenderer == null)
                {
                    _spriteRenderer = GetComponent<SpriteRenderer>();
                }
                    return _spriteRenderer;
            }
        }

        private BoxCollider2D box;

        public BoxCollider2D BoxCollider2D
        {
            get
            {
                if (box == null)
                {
                    box = GetComponent<BoxCollider2D>();
                }
                return box;
            }
        }

        public void ShowFoodSelected()
        {
            spriteSelected.DOFade(1, 0.2f).SetEase(Ease.OutBack).From(0);
        }
        
    }
}
