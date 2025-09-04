using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DevDuck
{
    public class TestHelicopTer : MonoBehaviour
    {
        public GameObject cube;
        public HelicopterController helicopter;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log((cube.transform.position - helicopter.transform.position).normalized);
                // StartCoroutine(ParabolaMotion(helicopter.transform.position, cube.transform.position, 15, 2));
                MoveParabola(helicopter.transform.position, cube.transform.position + new Vector3(0, 2, 0), 15, 2);
            }
        }

        IEnumerator ParabolaMotion(Vector3 start, Vector3 end, float height, float duration)
        {
            float elapsed = 0;
            helicopter.transform.LookAt(end);
            while (elapsed < duration)
            {
                float t = elapsed / duration;
                Vector3 horizontal = Vector3.Lerp(start, end, t);
                float y = Mathf.Lerp(start.y, end.y, t) + height * 4 * t * (1 - t);

                helicopter.transform.position = new Vector3(horizontal.x, y, horizontal.z);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = end;
        }

        public void MoveParabola(Vector3 pointA, Vector3 pointB, float height, float duration)
        {
            helicopter.transform.position = pointA;
            helicopter.transform.LookAt(pointB);
            
            Vector3 midPoint = (pointA + pointB) / 2 + Vector3.up * height + Vector3.right * height / 1.5f;
            Vector3[] path = new Vector3[] { pointA, midPoint, pointB };
            helicopter.transform.DOPath(path, duration, PathType.CatmullRom)
                .SetEase(Ease.Linear).OnComplete((() =>
                {
                    cube.transform.SetParent(helicopter.transform);
                    cube.transform.DOMove(helicopter.transform.position, 0.3f).SetEase(Ease.InBack).OnComplete((() =>
                    {
                        MoveOutOfScene();
                    }));
                }));
        }

        public void MoveOutOfScene()
        {
            Vector3 endPos = (cube.transform.position - helicopter.transform.position).normalized;
            Vector3 realPos = new Vector3(endPos.x - 30, endPos.y + 15, endPos.z + 150);
            helicopter.transform.DOMove(realPos, 4).SetDelay(0.3f);
        }
    }
}