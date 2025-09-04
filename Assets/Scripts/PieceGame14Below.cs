using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DevDuck
{
    public class PieceGame14Below : MonoBehaviour
    {
        public int id;
        private SpriteRenderer _spriteRenderer;
        public bool isDone = false;
        public SpriteRenderer SpriteRenderer
        {
            get
            {
                if (_spriteRenderer == null)
                {
                    _spriteRenderer = GetComponent<SpriteRenderer>();
                    if (_spriteRenderer == null)
                    {
                        _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                    }
                }
                return _spriteRenderer;
            }
        }
        
        private SpriteRenderer _spriteShadowRenderer;
        
        public SpriteRenderer ShadowSpriteRenderer
        {
            get
            {
                if (_spriteShadowRenderer == null)
                {
                    _spriteShadowRenderer = gameObject.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
                    if (_spriteShadowRenderer == null)
                    {
                        _spriteShadowRenderer = gameObject.transform.GetChild(0).gameObject.AddComponent<SpriteRenderer>();
                    }
                }
                return _spriteShadowRenderer;
            }
        }
        public void TurnOffShadow()
        {
           // ShadowSpriteRenderer.gameObject.SetActive(false);
            ShadowSpriteRenderer.DOFade(0, 0.5f);
            isDone = true;
        }

        public void SetSprite(int level)
        {
            switch (level)
            {
                case 1:
                    SpriteRenderer.sprite = Resources.Load<Sprite>($"Art/game14/magazine_1_{id}");
                    break;
                case 2:
                    SpriteRenderer.sprite = Resources.Load<Sprite>($"Art/game14/magazine_2_{id}");
                    break;
                case 3:
                    SpriteRenderer.sprite = Resources.Load<Sprite>($"Art/game14/magazine_3_{id}");
                    break;
                default:
                    SpriteRenderer.sprite = null;
                    break;
            }
        }
        
    }
}
