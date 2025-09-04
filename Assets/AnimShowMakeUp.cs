using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class AnimShowMakeUp : MonoBehaviour
    {
        public GameObject blush;
        public List<Image> eyeLenItems =  new List<Image>();
        public List<Image> eyeLineItems =  new List<Image>();
        public List<Image> lipstickItems =  new List<Image>();

        public void ShowMakeUp()
        {
            blush.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
            for (int i = 0; i < eyeLenItems.Count; i++)
            {
                var a = i;
                eyeLenItems[a].DOFade(1, 0.3f).From(0).OnComplete((() =>
                {
                    eyeLenItems[a].raycastTarget = true;
                }));
            }
            for (int i = 0; i < eyeLineItems.Count; i++)
            {
                var a = i;
                eyeLineItems[a].DOFade(1, 0.3f).From(0).OnComplete((() =>
                {
                    eyeLineItems[a].raycastTarget = true;
                }));
            }
            for (int i = 0; i < lipstickItems.Count; i++)
            {
                var a = i;
                lipstickItems[a].DOFade(1, 0.3f).From(0).OnComplete((() =>
                {
                    lipstickItems[a].raycastTarget = true;
                }));
            }
        }
    }
}
