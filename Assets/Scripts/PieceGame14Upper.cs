using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace DevDuck
{
    public class PieceGame14Upper : MonoBehaviour
    {
        public int id;
        public bool isDone;
        BoxCollider2D _boxCollider;
        
        SpriteRenderer _spriteRenderer;

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
        public BoxCollider2D BoxCollider2D
        {
            get
            {
                if (_boxCollider == null)
                {
                    _boxCollider = GetComponent<BoxCollider2D>();
                    if (_boxCollider == null)
                    {
                        _boxCollider = gameObject.AddComponent<BoxCollider2D>();
                    }
                }
                return _boxCollider;
            }
        }
        
        public void HidePiece()
        {
            BoxCollider2D.enabled = false;
            gameObject.transform.DOScale(1.05f, 0.15f).OnComplete(() =>
            {
                gameObject.transform.DOScale(0.98f, 0.15f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    SpriteRenderer.DOFade(0,0.2f);
                    isDone = true;

                  //  Debug.Log("?????????");
                });
            });
        }

        public void EnableBoxCollider()
        {
            BoxCollider2D.enabled = true;
            SpriteRenderer.DOColor(Color.white, 1f).From(0);
        }

        public void DisableBoxCollider()
        {
            BoxCollider2D.enabled = false;
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

        public void ShowFaild()
        {
            SpriteRenderer.DOColor(new Color32(255, 122, 122, 255), 0.1f).OnComplete(() =>
            {
                SpriteRenderer.DOColor(new Color32(255, 255, 255, 255), 0.1f);
            });
        }
    }
}
