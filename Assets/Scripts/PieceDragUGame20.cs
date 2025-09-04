using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DevDuck
{
    public class PieceDragUGame20 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public int id;
        public Image pieceImage,ShadowImage;
        public Button rotateButton;
        public GameObject pieceRotate;
        public bool isRotated;
        
        private void Start()
        {
            rotateButton.onClick.AddListener(OnClickRotateButton);
        }

        private void OnClickRotateButton()
        {
            isRotated = true;
            pieceRotate.transform.DORotate(Vector3.zero, 0.3f, RotateMode.FastBeyond360).SetEase(Ease.InBack).OnComplete((() => rotateButton.gameObject.SetActive(false)));
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            pieceImage.color = new Color32(255, 255, 255, 0);
            Observer.Notify(EventAction.EVENT_PLAYER_BEGINSELECT,id);
            rotateButton.gameObject.SetActive(false);
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Observer.Notify(EventAction.EVENT_PLAYER_ENDSELECT,-1);
            
        }

        public void ShowPieceImage()
        {
            pieceImage.color = new Color32(255, 255, 255, 255);
        }

        public void HidePieceImage()
        {
            this.gameObject.GetComponent<Image>().raycastTarget = false;
            ShadowImage.raycastTarget = false;
            pieceImage.color = new Color32(255, 255, 255, 0);
        }
    }
}