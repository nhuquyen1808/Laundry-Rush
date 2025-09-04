using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DevDuck
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
    public class playerGame2 : MonoBehaviour
    {
        [SerializeField] FixedJoystick _joystick;
        [SerializeField] Rigidbody2D _rb;
        public float _moveSpeed;
        private bool isTrigger;
        private bool isSpawned;
        public GameObject handHintMove;

        private void Start()
        {
            if (!LogicGame2.instance.isDoneTutorial)
            {
                transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).SetDelay(1.7f).From(0)
                    .OnComplete((() =>
                    {
                        isSpawned = true;
                        handHintMove.SetActive(true);
                    }));
            }
            else
            {
                transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).SetDelay(1.7f).From(0)
                    .OnComplete((() =>
                    {
                        isSpawned = true;
                    }));
            }
        }
        bool isActiveHand = true;
        private void FixedUpdate()
        {
            if (_joystick.Direction.y != 0)
            {
                if (isSpawned)
                {
                    _rb.velocity = new Vector2(_joystick.Direction.x * _moveSpeed * Duck.TimeMod,
                        _joystick.Direction.y * _moveSpeed * Duck.TimeMod);

                    if (handHintMove.activeSelf && isActiveHand)
                    {
                        isActiveHand = false;
                        StartCoroutine( LogicGame2.instance.CameraMoveTutorial()) ;
                        /*handHintMove.SetActive(false);
                        LogicGame2.instance.focusTutorial.gameObject.SetActive(false);*/
                        StartCoroutine(DelayHide());
                        Debug.Log("Something happened there");
                    }

                    if (!LogicGame2.instance.isDoneTutorial)
                    {
                        if (LogicGame2.instance.distanceTutPlayer())
                        {
                            _rb.velocity = Vector2.zero;
                        }
                    }
                }
            }
            else
            {
                if (!isTrigger)
                {
                    _rb.velocity = Vector2.zero;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            BotMoveMentGame2 bot = collision.gameObject.GetComponent<BotMoveMentGame2>();
            if (bot != null)
            {
                Vector2 dir = (transform.position - bot.transform.position).normalized;
                bot.gameObject.GetComponent<Rigidbody2D>().AddForce(-dir * 4.5f, ForceMode2D.Impulse);
                bot.isTrigger = true;
                _rb.AddForce(dir * 2f, ForceMode2D.Impulse);
                DOVirtual.DelayedCall(1, (() => bot.isTrigger = false));
                DOVirtual.DelayedCall(1, (() => isTrigger = false));
                isTrigger = true;
            }
        }

        IEnumerator DelayHide()
        {
            yield return new WaitForSeconds(1.5f);
            handHintMove.SetActive(false);
            LogicGame2.instance.focusTutorial.gameObject.SetActive(false);
        }
    }
}