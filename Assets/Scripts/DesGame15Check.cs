using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class DesGame15Check : MonoBehaviour
    {
        private BoxCollider2D boxCollider;

        public BoxCollider2D BoxCollider2d
        {
            get
            {
                if (boxCollider == null)
                {
                    boxCollider = GetComponent<BoxCollider2D>();
                }
                return boxCollider;
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            Tag obstacleTag = other.gameObject.GetComponent<Tag>();
            if (obstacleTag != null)
            {
                if (obstacleTag.tagName == "Player")
                {
                    Observer.Notify(EventAction.EVENT_POPUP_SHOW_WIN_DONE,null);
                    Observer.Notify(EventAction.EVENT_PLAYER_MOVE_DONE,false);
                    Observer.Notify(EventAction.EVENT_GET_COINS, 30);
                }
            }
        }
    }
}
