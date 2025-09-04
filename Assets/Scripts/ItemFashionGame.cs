 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DevDuck
{
    public class ItemFashionGame : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
    {

        public int id;
        public bool isOnModel;
        
        public void OnPointerDown(PointerEventData eventData)
        {
          //  Debug.Log("OnPointerDown :   " + id );
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (isOnModel)
            {
                isOnModel = false;
                Observer.Notify(EventAction.EVENT_TRY_RELEASE_ITEM, id);
            }
            else
            {
                isOnModel = true;
                Observer.Notify(EventAction.EVENT_ITEM_SELECTED, id);
            }
        }
    }
}
