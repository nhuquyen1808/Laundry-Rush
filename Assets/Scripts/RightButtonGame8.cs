using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DevDuck
{
    public class RightButtonGame8 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        bool pointerDown;
        public void OnPointerDown(PointerEventData eventData)
        {
            pointerDown = true;
            for (int i = 0; i < LogicGame8.ins.standsObjRight.Count; i++)
            {
                LogicGame8.ins.standsObjRight[i].transform.DOKill();
                LogicGame8.ins.standsObjRight[i].transform.rotation = Quaternion.identity;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            pointerDown = false;
            LogicGame8.ins.Reset(true);


        }
        void Update()
        {
            if (pointerDown)
            {
               // arrowDown.transform.Rotate(-Vector3.forward,50*Time.deltaTime);
                LogicGame8.ins.RotateLeft();
            }
        }
    }
}
