using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DevDuck
{
    public class GiftLevel6 : MonoBehaviour
    {
        public TYPEITEM8 type;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.GetComponent<Tag>().tagName == "RedContainer" && type == TYPEITEM8.RED)
            {
                Observer.Notify(EventAction.EVENT_GET_RED, 1);
                this.gameObject.GetComponent<PooledObject>().ReturnToPool();
                this.gameObject.transform.position = Vector3.zero;

            }
            if (collision.gameObject.GetComponent<Tag>().tagName == "GreenContainer" && type == TYPEITEM8.GREEN)
            {
                Observer.Notify(EventAction.EVENT_GET_GREEN, 1);
                this.gameObject.GetComponent<PooledObject>().ReturnToPool();
                this.gameObject.transform.position = Vector3.zero;

            }
            if (collision.gameObject.GetComponent<Tag>().tagName == "ReturnPool" )
            {
                this.gameObject.GetComponent<PooledObject>().ReturnToPool();  
                this.gameObject.transform.position = Vector3.zero;

            }
          //  this.gameObject.GetComponent<PooledObject>().ReturnToPool();
        }
    }
}
