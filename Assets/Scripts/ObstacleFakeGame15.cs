using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DevDuck
{
    public class ObstacleFakeGame15 : MonoBehaviour
    {
        private BoxCollider2D _boxCollider;

        public BoxCollider2D BoxCollider
        {
            get
            {
                if (_boxCollider == null)
                {
                    _boxCollider = GetComponent<BoxCollider2D>();
                }

                return _boxCollider;
            }
        }
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
        public GameObject childObject;
        public SpriteRenderer childSpriteRenderer;
        public Sprite childSprite;

        private void OnTriggerExit2D(Collider2D other)
        {
            Tag obstacleTag = other.gameObject.GetComponent<Tag>();
            if (obstacleTag != null)
            {
                if (obstacleTag.tagName == "Player")
                {
                    BoxCollider.isTrigger = false;
                    gameObject.layer = LayerMask.NameToLayer("Obstacle");
                    childObject.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
                    childSpriteRenderer.sprite = childSprite;
                    SpriteRenderer.sprite = null;
                }
            }
        }
    }
}