using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DevDuck
{
    public class PooledPlusPoint : MonoBehaviour
    {
        private PoolPlusPoint pool;
        public PoolPlusPoint Pool { get => pool; set => pool = value; }
        public void ReturnToPool()
        {
            pool.ReturnToPool(this);
        }

        public void SetText(int score, Vector3 pos)
        {
            this.gameObject.GetComponent<TextMeshPro>().text ="+"+ score.ToString();
            transform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
            transform.DOMove(pos+ new Vector3(0,1,0),0.5f).OnComplete(()=> ReturnToPool());
        }
    }
}
