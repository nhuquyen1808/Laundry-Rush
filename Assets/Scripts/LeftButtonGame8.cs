using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DevDuck
{
    public class LeftButtonGame8 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        bool pointerDown;

        public void OnPointerDown(PointerEventData eventData)
        {
            for (int i = 0; i < LogicGame8.ins.standsObjLeft.Count; i++)
            {
                LogicGame8.ins.standsObjLeft[i].transform.DOKill();
                LogicGame8.ins.standsObjLeft[i].transform.rotation = Quaternion.identity;
            }
            pointerDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            pointerDown = false;
            LogicGame8.ins.Reset(false);
        }
        void Update()
        {
            if (pointerDown)
            {
                //arrowDown.transform.Rotate(Vector3.forward, 50 * Time.deltaTime);
                LogicGame8.ins.RotateRight();
            }
        }
    }
}
