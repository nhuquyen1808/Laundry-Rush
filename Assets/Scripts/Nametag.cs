using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DevDuck
{
    public class Nametag : MonoBehaviour
    {
        // Start is called before the first frame update
        Vector3 startPosition;
        private float timer;
        
        void Start()
        {
            timer = Random.Range(0.5f, 1f);
            startPosition = this.GetComponent<RectTransform>().anchoredPosition;
          this.GetComponent<RectTransform>().DOAnchorPos(startPosition + new Vector3(0, 50f, 0), timer).OnComplete((() =>
          {
              this.GetComponent<RectTransform>().DOAnchorPos(startPosition , timer).SetLoops(-1, LoopType.Yoyo);
          }));
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}
