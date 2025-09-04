using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class TweenHeartBar : MonoBehaviour
    {
        [SerializeField] GameObject leftWing, rightWing, heartImg, heartBg;
        public void StopAnimationHeartBar()
        {
            leftWing.transform.DOKill();
            rightWing.transform.DOKill();
            heartImg.transform.DOKill();
            heartBg.transform.DOKill();
        }
        public void PlayAnimationHeartBar( int loops)
        {
            leftWing.transform.DOKill();
            rightWing.transform.DOKill();
            heartImg.transform.DOKill();
            heartBg.transform.DOKill();
            leftWing.transform.DORotate(new Vector3(0, 0, 10), 0.2f).OnComplete(() =>
            {
                leftWing.transform.DORotate(new Vector3(0, 0, -10), 0.2f).SetLoops(loops, LoopType.Yoyo);
            });
           rightWing.transform.DORotate(new Vector3(0, 0, -10), 0.2f).OnComplete(() =>
            {
                rightWing.transform.DORotate(new Vector3(0, 0, 10), 0.2f).SetLoops(loops, LoopType.Yoyo);
            });
             heartImg.transform.DOScale(1.1f, 0.2f).OnComplete(() =>
            {
                heartImg.transform.DOScale(0.95f, 0.2f).SetLoops(loops, LoopType.Yoyo);
            });
           heartBg.transform.DOScale(1.1f, 0.2f).OnComplete(() =>
            {
                heartBg.transform.DOScale(0.95f, 0.2f).SetLoops(loops, LoopType.Yoyo);
            });
        }

    }
}
