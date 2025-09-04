using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DevDuck
{
    public class ShufferCard : MonoBehaviour
    {
        public List<Transform> cards;
        public Transform centerPoint;
        public float radius = 2f;

        public float duration = 2f;

        // Start is called before the first frame update
        void Start()
        {
        }

        void ShuffleCards()
        {
            for (int i = 0; i < cards.Count; i++) 
            {
                if (cards[i] == null)
                {
                    Debug.LogError($"⚠ Thẻ bài tại index {i} bị null!");
                    continue;
                }

                float angle = (i * (360 / cards.Count)) * Mathf.Deg2Rad;
                Vector3 targetPos = centerPoint.position + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
                cards[i].transform.position = targetPos;
                cards[i].DOMove(centerPoint.position, duration / 2).SetEase(Ease.InOutSine);
                /*cards[i].DOMove(targetPos, duration / 2).SetEase(Ease.InOutSine)
                    .OnComplete(() =>
                    {
                        if (cards[i] != null) // Kiểm tra trước khi truy cập
                            cards[i].DOMove(centerPoint.position, duration / 2).SetEase(Ease.InOutSine);
                    });*/
            }

        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ShuffleCards();
            }
        }
    }
}