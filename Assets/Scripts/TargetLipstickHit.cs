using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace DevDuck
{
    public class TargetLipstickHit : MonoBehaviour
    {

        [SerializeField] float timer = 0f;
        SpriteRenderer spr;

        public bool isLeft;
        public static TargetLipstickHit instance;

        private void Awake()
        {
            if(instance != null)
            {
                Destroy(gameObject);
            }
            else instance = this;
        }
        private void Start()
        {
            spr = GetComponent<SpriteRenderer>();
            Observer.AddObserver(EventAction.EVENT_HITTARGET_MINIGAMEHITLIPSTICK, HitTheTarget);
        }

        private void HitTheTarget(object obj)
        {
            this.transform.DOShakePosition(0.1f, 0.3f, 1);
            spr.DOColor(new Color32(219, 219, 219, 255), 0.07f).OnComplete(() => spr.DOColor(new Color32(255, 255, 255, 255), 0.07f));
        }

        private void Update()
        {
            timer += 1 * Duck.TimeMod;

            if (timer > 10)
            {
                transform.Rotate(0, 0, 50 * Duck.TimeMod);
                isLeft = true;
            }
            else
            {
                isLeft = false;
                transform.Rotate(0, 0, -50 * Duck.TimeMod);

            }
        }
    }
}
