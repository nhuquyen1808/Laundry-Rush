using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DevDuck
{
    public enum TypeBot
    {
        NONE,
        YELLOW,
        RED
    }

    public class BotGame4 : MonoBehaviour
    {
        public float moveSpeed = 2f; // Speed of the bot
        public Vector2 areaSize = new Vector2(10, 10); // Define area boundaries
        public Vector2 targetPosition;
        public TypeBot botType;
        [SerializeField] List<GameObject> posItems = new List<GameObject>();
        [SerializeField] PoolSmokeGetPoint poolSmokeGetPoint;
        private int countItemOnTray;
        void Start()
        {
            SetRandomTargetPosition();
        }

        void Update()
        {
            MoveTowardsTarget();
        }

        void SetRandomTargetPosition()
        {
            float randomX = Random.Range(-areaSize.x / 2, areaSize.x / 2);
            float randomY = Random.Range(-areaSize.y / 2, areaSize.y / 2);
            float z = 0;
            if (this.botType == TypeBot.YELLOW)
            {
                z = 2;
            }
            else if (this.botType == TypeBot.RED)
            {
                z = 3;
            }
            targetPosition = new Vector3(randomX, randomY,z);
           
        }

        void MoveTowardsTarget()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Duck.TimeMod);
            if ((Vector2)transform.position == targetPosition ||
                Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                SetRandomTargetPosition();
            }
        }
        private void PlayerGetPoint(int _score, Sprite _spr)
        {
            for (int i = 0; i < posItems.Count; i++)
            {
                if (i + 1 == _score)
                {
                    
                    posItems[i].GetComponent<SpriteRenderer>().sprite = _spr;
                    posItems[i].transform.DOScale(1, 0.25f).SetEase(Ease.OutBack);
                    
                    PooledObject o =  poolSmokeGetPoint.GetPooledObject( posItems[i].transform.position, Quaternion.Euler(90,0,0));
                    o.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    o.transform.position += new Vector3(0.3f, 0.3f, 0.3f);
                    o.transform.SetParent(this.transform);
                    Duck.PlayParticle(o.GetComponent<ParticleSystem>());
                    DOVirtual.DelayedCall(1, o.ReturnToPool);
                }
            }
        }

        void OnDrawGizmos()
        {
            // Draw the movement area in the Scene view
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Vector2.zero, areaSize);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Tag>().tagName == "ItemGame4")
            {
                if (botType == TypeBot.YELLOW)
                {
                    Observer.Notify(EventAction.EVENT_BOT_YELLOW_GET_SCORE, 1);
                }
                else if (botType == TypeBot.RED)
                {
                    Observer.Notify(EventAction.EVENT_BOT_RED_GET_SCORE, 1);
                }
                countItemOnTray += 1;
                TypeItem type = other.GetComponent<ItemGame4>().TYPE;
                if (type == TypeItem.SHRIMP)
                {
                    PlayerGetPoint(countItemOnTray, LogicGame4.instance.item1);
                }
                else if (type == TypeItem.SALMON)
                {
                    PlayerGetPoint(countItemOnTray, LogicGame4.instance.item2);
                }
                else
                {
                    PlayerGetPoint(countItemOnTray, LogicGame4.instance.item3);
                }
                // LogicGame4.instance.GetPoint(T.GetComponent<ItemGame4>().id);
               // PooledPlusPoint pooledPlusPoint = poolPoint.GetPooledObject(transform.position + new Vector3(0, 1, 0), Quaternion.identity);
              //  pooledPlusPoint.SetText(1, transform.position + new Vector3(0, 1, 0));
                if (countItemOnTray == 15)
                {
                    countItemOnTray = 0;
                    ResetTray();
                }
            }
        }
        public void ResetTray()
        {
            moveSpeed = 0;
            Vector2 pos = this.transform.position;
            transform.DOMove(pos + new Vector2(10, 0), 1f).OnComplete(() =>
            {
                for (int i = 0; i < posItems.Count; i++)
                {
                    posItems[i].transform.localScale = Vector2.zero;
                    posItems[i].GetComponent<SpriteRenderer>().sprite = null;
                }

                transform.DOMove(pos, 1f).OnComplete(() =>
                {
                    moveSpeed = 2;
                });
            });

        }
    }
}