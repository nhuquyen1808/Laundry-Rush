using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DevDuck
{
    public class HelicopterController : MonoBehaviour
    {
        public void MoveParabola(Vector3 pointA, Vector3 pointB, float height, float duration,GameObject something)
        {
            transform.position = pointA;
           transform.LookAt(pointB);
            
            Vector3 midPoint = (pointA + pointB) / 2 + Vector3.up * height + Vector3.right * height / 1.5f;
            Vector3[] path = new Vector3[] { pointA, midPoint, pointB };
           transform.DOPath(path, duration, PathType.CatmullRom)
                .SetEase(Ease.Linear).OnComplete((() =>
                {
                    something.transform.SetParent(transform);
                    something.transform.DOMove(transform.position, 0.3f).SetEase(Ease.InBack).OnComplete((() =>
                    {
                        MoveOutOfScene(something);
                    }));
                }));
        }

        public void MoveOutOfScene(GameObject something)
        {
            Vector3 endPos = (something.transform.position - transform.position).normalized;
            Vector3 realPos = new Vector3(endPos.x - 30, endPos.y + 15, endPos.z + 150);
            transform.DOMove(realPos, 4).SetDelay(0.3f);
        }
    }
}
