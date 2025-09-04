using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DevDuck
{
    public class BotMoveMentGame2 : MonoBehaviour
    {
        public float moveSpeed = 2f; // Speed of the bot
        public Vector2 areaSize = new Vector2(10, 10); // Define area boundaries
        private Vector2 targetPosition;
        public bool isTrigger;
        public Rigidbody2D rb;
        public bool isSpawned = false;
        public bool moveOnTutorial = false;
    
        void Start()
        {
            SetRandomTargetPosition();
            rb = GetComponent<Rigidbody2D>();
            if (LogicGame2.instance.isDoneTutorial)
            {
                transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).SetDelay(1.7f).From(0)
                    .OnComplete((() => { isSpawned = true; }));
            }
        }

        public void SpawnBot()
        {
            transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).SetDelay(1.7f).From(0);
        }

        void Update()
        {
            if (!isTrigger && isSpawned)
            {
                MoveTowardsTarget();
            }

            if (moveOnTutorial)
            {
                MoveOnTutorial(PosTutorial);
                if (Vector2.Distance(transform.position, PosTutorial) < 0.1f)
                {
                    moveOnTutorial = false;
                }
            }
        }

        public Vector3 PosTutorial;

        void SetRandomTargetPosition()
        {
            float randomX = Random.Range(-areaSize.x / 2, areaSize.x / 2);
            float randomY = Random.Range(-areaSize.y / 2, areaSize.y / 2);
            targetPosition = new Vector2(randomX, randomY);
        }

        void MoveTowardsTarget()
        {
            // Move the bot towards the target position
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Duck.TimeMod);
            // Check if bot has reached the target position
            if ((Vector2)transform.position == targetPosition)
            {
                SetRandomTargetPosition();
            }
        }

        void OnDrawGizmos()
        {
            // Draw the movement area in the Scene view
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Vector2.zero, areaSize);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Tag tag = other.gameObject.GetComponent<Tag>();
            if (tag != null && tag.tagName == "CHAIR")
            {
                Vector2 dir = (other.transform.position - transform.position).normalized;
                rb.AddForce(dir * 2f, ForceMode2D.Impulse);
                SetRandomTargetPosition();
            }

            if (tag != null && tag.tagName == "Opponent")
            {
                Debug.Log(other.gameObject.name + "   ????");
                Vector2 dir = (tag.transform.position - transform.position).normalized;
                rb.AddForce(-dir * 3.5f, ForceMode2D.Impulse);
                isTrigger = true;
                DOVirtual.DelayedCall(0.5f, () =>
                {
                    isTrigger = false;
                    SetRandomTargetPosition();
                });
               
            }
        }

        public void MoveOnTutorial(Vector3 pos)
        {
            transform.position = Vector2.MoveTowards(transform.position, pos, moveSpeed * Duck.TimeMod);
        }

       
    }
}