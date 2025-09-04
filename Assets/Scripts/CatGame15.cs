using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

namespace DevDuck
{
    public class CatGame15 : MonoBehaviour
    {
        private Vector3 startPos;
        [SerializeField] SkeletonAnimation _animation;
        public ParticleSystem hitSmoke;
        
        void Start()
        {
            startPos = this.transform.position;
            CatAppear();
        }
        public void CatAppear()
        {
            this.transform.position = startPos + new Vector3(0, 15, 0);
            this.transform.DOMove(startPos, 1f).SetEase(Ease.Linear).SetDelay(0.8f).OnComplete(() =>
            {
                ManagerSpine.PlaySpineAnimation(_animation, "idle", true);
                Duck.PlayParticle(hitSmoke);
            });
        }
    }
}