using UnityEngine;
using System;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using UnityEditor;


public class EasyBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public Action PointerDownAction, PointerUpAction, PointerClickAction;
    public Image img;
    public void OnPointerClick(PointerEventData eventDataT)
    {
        img.raycastTarget = false;
        transform.DOScale(0.9F, 0.2F).OnComplete(() =>
      {
          transform.DOScale(1f, 0.15F).OnComplete(() =>
          {
              img.raycastTarget = true;
              PointerClickAction?.Invoke();
          });
      });
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(0.9F, 0.3F).OnComplete(() =>
        {
            transform.DOScale(1f, 0.3F).OnComplete(() =>
            {
                PointerDownAction?.Invoke();
                Debug.Log("action invoked");
            });
        });
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PointerUpAction?.Invoke();
    }


}
