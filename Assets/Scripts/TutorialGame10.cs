using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DevDuck
{
    public class TutorialGame10 : MonoBehaviour
    {
        public static TutorialGame10 Instance;
        public GameObject boxHint;
        public GameObject boxOrder;

        private void Awake()
        {
            Instance = this;
        }

        public GameObject customer, handHint, handHintPressOreder;

        public void CustomerMoveToTable()
        {
            customer.transform.DOMove(new Vector2(0, 7.6f), 0.5f).OnComplete(() =>
            {
                customer.transform.DOScale(0.9f, 0.5f);
                customer.transform.DOMove(new Vector2(0, -1), 0.8f).OnComplete((() =>
                {
                    LogicGame10EasyMode.instance.boxOrder.gameObject.SetActive(true);
                    LogicGame10EasyMode.instance.boxOrder.transform.DOScale(0.6f, 0.5f).SetEase(Ease.OutBack)
                        .OnComplete(() =>
                        {
                            LogicGame10EasyMode.instance.bellAnim.enabled = true;
                            LogicGame10EasyMode.instance.bellAnim.gameObject.GetComponent<BoxCollider2D>().enabled =
                                true;
                            LogicGame10EasyMode.instance.bellAnim.Play("Bell", 0, 0);
                            handHint.SetActive(true);
                        });
                }));
            });
        }

        public void CustomerGoOut()
        {
            customer.transform.DOMoveX(15, 0.5f).SetEase(Ease.Linear).SetDelay(2f).OnComplete((() =>
            {
                
            }));
        }


        public void HandDisable()
        {
            boxOrder.transform.localScale = new Vector3(0, 0, 0);
            if (handHint.activeSelf)
            {
                handHint.SetActive(false);
            }
            if (!LogicGame10EasyMode.instance.isDoneTut10EasyMode)
            {
                boxHint.gameObject.SetActive(true);
                boxHint.transform.DOScale(1.5f, 0.3f).SetEase(Ease.OutBack).SetDelay(1).From(0);
                boxOrder.transform.DOScale(0.8f, 0.3f).SetEase(Ease.OutBack).SetDelay(2f).From(0);
            }
        }

        public void TurnOfHandAndBox()
        {
            
            boxHint.SetActive(false);
            handHintPressOreder.SetActive(false);
            PlayerPrefs.SetInt("isDoneTut10EasyMode", 1);
            
            CustomerGoOut();
        }
    }
}