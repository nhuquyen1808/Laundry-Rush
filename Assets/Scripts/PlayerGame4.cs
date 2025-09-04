using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class PlayerGame4 : MonoBehaviour
    {
        [SerializeField] FixedJoystick _joystick;
        [SerializeField] Rigidbody2D _rb;
        [Header("Attribute : ")]
        public float _moveSpeed;
        public int countItemOnTray;

        [Header("Game")]
        [SerializeField] List<GameObject> posItems = new List<GameObject>();
        [SerializeField] Sprite shrimpSpr;
        [SerializeField] PoolPlusPoint poolPoint;
        [SerializeField] PoolSmokeGetPoint poolSmokeGetPoint;
        


        private void FixedUpdate()
        {
            if (_joystick.Direction.y != 0)
            {
                _rb.velocity = new Vector2(_joystick.Direction.x * _moveSpeed * Duck.TimeMod, _joystick.Direction.y * _moveSpeed * Duck.TimeMod);
            }
            else
            {
                _rb.velocity = Vector2.zero;
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

        public void ResetTray()
        {
            _moveSpeed = 0;
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
                    _moveSpeed = 150;
                });
            });

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Tag T = collision.gameObject.GetComponent<Tag>();

            if (T != null)
            {
                if (T.tagName == "ItemGame4")
                {
                    countItemOnTray += 1;
                    Observer.Notify(EventAction.EVENT_GET_SCORE,1);
                    TypeItem type = T.GetComponent<ItemGame4>().TYPE;
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
                    PooledPlusPoint pooledPlusPoint = poolPoint.GetPooledObject(transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                    pooledPlusPoint.SetText(1, transform.position + new Vector3(0, 1, 0));
                    if (countItemOnTray == 15)
                    {
                        countItemOnTray = 0;
                        ResetTray();
                    }
                }
            }
        }

        private void ReturnPoolObjects(PooledObject o)
        {
            o.ReturnToPool();
            o.transform.SetParent(poolSmokeGetPoint.transform);
        }
    }
}
