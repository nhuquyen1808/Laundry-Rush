using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public static class StoryPerforment
    {
        //  public static Tween shakeTween;
        public static void ModelMovement(GameObject model /*, Vector3 desiredPosition, Tween tween, float timer*/)
        {

            model.transform.DORotate(new Vector3(0, 0, -3), 0.3f).OnComplete((() =>
            {
                model.transform.DORotate(new Vector3(0, 0, 3), 0.3f).SetLoops(-1, LoopType.Yoyo);
            }));
        }

        public static void DefaultRotatationModel(GameObject model)
        {
            model.transform.DORotate(Vector3.zero, .2f);
            model.transform.DOKill();
        }

        public static void ModelTalking(GameObject model)
        {
            model.transform.DORotate(new Vector3(0, 0, -3), 0.2f).OnComplete(() =>
            {
                model.transform.DORotate(new Vector3(0, 0, 3), 0.2f).OnComplete(() =>
                {
                    model.transform.DORotate(new Vector3(0, 0, -3), 0.2f).OnComplete(() =>
                    {
                        model.transform.DORotate(new Vector3(0, 0, 3), 0.2f).OnComplete(() =>
                        {
                            model.transform.DORotate(new Vector3(0, 0, 0), 0.2f);
                        });
                    });
                });
            });
        }

        public static void ImageContentSetup(Image imgContent, bool isLeft,Sprite sprite)
        {
            imgContent.sprite = sprite;
            if (isLeft)
            {
                imgContent.rectTransform.pivot = new Vector2(0, 0);
                imgContent.rectTransform.anchoredPosition = new Vector3(-200, 60, 0);
            }
            else
            {
                imgContent.rectTransform.pivot = new Vector2(1, 0);
                imgContent.rectTransform.anchoredPosition = new Vector3(200, 60, 0);
            }
        }
}
}
