using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public static class UiDuck
{
    public static void ShowPopup(GameObject shadow, CanvasGroup nPopup)
    {
        shadow.gameObject.SetActive(true);
        nPopup.transform.localScale = Vector3.zero;
        nPopup.gameObject.SetActive(true);
        nPopup.alpha = 0;
        nPopup.DOFade(1, 0.3f).SetEase(Ease.OutBack);
        nPopup.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
    }

    public static void HidePopup(GameObject shadow, CanvasGroup nPopup)
    {
        shadow.gameObject.SetActive(false);
        nPopup.transform.localScale = Vector3.one;
        nPopup.alpha = 1;
       // nPopup.gameObject.SetActive(false);
        nPopup.DOFade(0, 0.3f).SetEase(Ease.InBack);
        nPopup.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(()=>nPopup.gameObject.SetActive(false));
    }

    public static void ShowElementsPopup(List<GameObject> elements, float timeWaitingBetweenElements)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            elements[i].transform.localScale = Vector3.zero;
        }
        for (int i = 0; i < elements.Count; i++)
        {
            var a = i;
            elements[a].transform.DOScale(1,0.3f).SetEase(Ease.OutBack).SetDelay(a*timeWaitingBetweenElements);
        }
    }

    public static void PinScrollRect(ScrollRect scrollRect)
    {
        scrollRect.verticalNormalizedPosition = 0.5f;
    }
    
    
}
