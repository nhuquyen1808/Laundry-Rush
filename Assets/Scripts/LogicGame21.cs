using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DevDuck
{
    public class LogicGame21 : MonoBehaviour
    {
        [SerializeField] List<CupPapa> cups = new List<CupPapa>();
        [SerializeField] List<GameObject> shelfs = new List<GameObject>();
        public CupPapa currentCup;
        public GameObject hand;
        int currentCupIndex = 0;
        private float yHandAppear;
        public SkeletonAnimation impactEffect;
        private void Start()
        {
            currentCup = cups[currentCupIndex];
            Observer.AddObserver(EventAction.EVENT_GET_SCORE, PlayerGetPoint);
            Observer.AddObserver(EventAction.EVENT_LOSE_SCORE, PlayerLosePoint);
        }

        private void PlayerLosePoint(object obj)
        {
            Vector2 pos = (Vector2)obj;
            /*if (shelfs[currentCupIndex] != null)
                redObject.transform.position = new Vector2(pos.x, shelfs[currentCupIndex].transform.position.y);
            redObject.SetActive(true);*/
            if (cups[currentCupIndex] != null)
            {
                cups[currentCupIndex].tween.Kill();
            }
            //   Debug.Log("Lose and Load Scene");
            //  SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void PlayerGetPoint(object obj)
        {
            cups[currentCupIndex].tween.Kill();
            /*cups[currentCupIndex].bottomCollider.isTrigger = true;
            cups[currentCupIndex].leftCollider.isTrigger = true;
            cups[currentCupIndex].rightCollider.isTrigger = true;*/
            currentCupIndex++;
            currentCup = cups[currentCupIndex];
            // shelfs[currentCupIndex].layer = LayerMask.NameToLayer("Default");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Scene currentScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(currentScene.name);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentCup.Jump();
                BangTheTable();
            }
        }

        public void BangTheTable()
        {
            GetYAxisHandAppear();
            hand.SetActive(true);
            hand.transform.rotation = Quaternion.Euler(0, 0, -15);
            hand.transform.localPosition = new Vector3(7.5F, yHandAppear, 0);
            hand.transform.DORotate(new Vector3(0, 0, 0), 0.05f).SetEase(Ease.InBack).OnComplete(() =>
            {
                impactEffect.gameObject.SetActive(true);
                ManagerSpine.PlaySpineAnimation(impactEffect, "impact", false);
                hand.transform.DOLocalMoveX(20.0F, 0.2f).SetEase(Ease.InBack).SetDelay(0.5f)
                    .OnComplete((() => impactEffect.gameObject.SetActive(false)));
            });
        }
        private void GetYAxisHandAppear()
        {
            switch (currentCupIndex)
            {
                case 0:
                    yHandAppear = -5.5f;
                    break;
                case 1:
                    yHandAppear = -0.28f;
                    break;
                case 2:
                    yHandAppear = 5.15f;
                    break;
            }
        }
    }
}