using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DevDuck
{
    public class TableOrderGame10 : MonoBehaviour
    {
        [SerializeField] List<FoodGame10> foods = new List<FoodGame10>();
        public SpriteRenderer avatar;
        public GameObject boxOrder;
        public SpriteRenderer order1, order2, order3;
        public GameObject clock, board;
        [SerializeField] private TextMeshPro timeText;
        private SpriteRenderer sprite;
        public SpriteRenderer markSprite1, markSprite2, markSprite3;
        [SerializeField] private Sprite tickSpr, xSprite;

        public List<Sprite> avatarSprites = new List<Sprite>();

        public SpriteRenderer SpriteRenderer
        {
            get
            {
                if (sprite == null)
                {
                    sprite = GetComponent<SpriteRenderer>();
                }

                return sprite;
            }
        }

        public void ShowTableOrder()
        {

            SetAvatarSprite();
            order3.transform.localScale = Vector3.zero;
            order2.transform.localScale = Vector3.zero;
            order1.transform.localScale = Vector3.zero;
            SpriteRenderer.DOColor(new Color32(255, 255, 255, 255), .2f);
            avatar.transform.DOMove(new Vector3(-3.3f, 6.5f, 0), 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                boxOrder.transform.DOScale(0.8f, 0.3f).SetEase(Ease.OutQuad);
            });
            clock.transform.DOMove(new Vector3(2.8f, 4.3f, 0), 0.5f).SetEase(Ease.OutQuad).SetDelay(0.8f);
            board.transform.DOMove(new Vector3(0f, -3f, 0), 0.5f).SetEase(Ease.OutQuad).SetDelay(0.8f)
                .OnComplete((() => ShowListOrder()));
        }

        private void ShowListOrder()
        {
            /*order1.sprite = LogicGame10.Instance.order1.sprite;
            order2.sprite = LogicGame10.Instance.order2.sprite;
            order3.sprite = LogicGame10.Instance.order3.sprite;*/

            for (int i = 0; i < foods.Count; i++)
            {
                foods[i].spriteSelected.color = new Color32(255, 255, 255, 0);
                foods[i].transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack).From(0).SetDelay(0.1f * i);
            }
        }

        public void SetTimer(float time)
        {
            if (time > 20)
            {
                timeText.text = "20s";
            }
            else
            {
                timeText.text = Mathf.RoundToInt(time).ToString() + "s";
                if (time <= 5.1f)
                {
                    timeText.color = Color.red;
                    if (time < 1.1f)
                    {
                        ShakeClock();
                    }
                }
                else if (time <= 10.1f)
                {
                    timeText.color = Color.yellow;
                }
                else
                {
                    timeText.color = Color.white;
                }
            }
        }

        private void ShakeClock()
        {
            bool isShake = false;
            if (!isShake)
            {
                isShake = true;
                clock.transform.DOShakeRotation(0.3f, new Vector3(0, 0, 3), 1).OnComplete((() =>
                {
                    clock.transform.rotation = Quaternion.identity;
                }));
            }
        }

        public void HideTableOrder()
        {
            board.transform.DOMoveX(-20, 0.5f).SetEase(Ease.InBack);
            clock.transform.DOMoveX(15, 0.5f).SetEase(Ease.Linear).SetDelay(0.35f);
            avatar.transform.DOMoveX(-20, 0.5f).SetEase(Ease.Linear).SetDelay(0.7f);
            boxOrder.transform.DOScale(0, 0.4f).SetEase(Ease.InBack).SetDelay(0.8f);
            SpriteRenderer.DOFade(0, 0.5f).SetEase(Ease.InBack).SetDelay(1f).OnComplete(() =>
            {
                for (int i = 0; i < foods.Count; i++)
                {
                    foods[i].transform.localScale = Vector3.zero;
                }
            });
        }

        public void SetMarkSprite(int count, bool isCorrect)
        {
            if (isCorrect)
            {
                if (count == 1)
                {
                    markSprite1.sprite = tickSpr;
                    markSprite1.transform.localPosition = new Vector3(0, -0.68f, 0);
                }

                if (count == 2)
                {
                    markSprite2.sprite = tickSpr;
                    markSprite2.transform.localPosition = new Vector3(0, -0.68f, 0);
                }

                if (count == 3)
                {
                    markSprite3.sprite = tickSpr;
                    markSprite3.transform.localPosition = new Vector3(0, -0.68f, 0);
                }
            }
            else
            {
                if (count == 1)
                {
                    markSprite1.sprite = xSprite;
                    markSprite1.transform.localPosition = new Vector3(0, 0f, 0);
                }
                if (count == 2)
                {
                    markSprite2.sprite = xSprite;
                    markSprite2.transform.localPosition = new Vector3(0, 0f, 0);
                }
                if (count == 3)
                {
                    markSprite3.sprite = xSprite;
                    markSprite3.transform.localPosition = new Vector3(0, 0f, 0);
                }
            }
        }

        public void SetAvatarSprite()
        {
            int id = Random.Range(0, avatarSprites.Count);
            avatar.sprite = avatarSprites[id];
            avatarSprites.Remove(avatarSprites[id]);
        }
    }
}