using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


namespace DevDuck
{
    public class IceGame21 : MonoBehaviour
    {
        public Rigidbody2D rb;
        public float iceForce;
        bool isJumping = false;
        private RaycastHit2D hit;
        [SerializeField] private LayerMask bottomGlass;
        public CircleCollider2D collider;

        private void Update()
        {
            if (isJumping)
            {
                CheckGlass();
            }
        }
        public void CheckGlass()
        {
            hit = Physics2D.Raycast(transform.position, Vector2.down,Mathf.Infinity,bottomGlass);
            if (hit.collider != null)
            {
                float distance = Vector3.Distance(transform.position, hit.point);
                if (distance < 0.55f && rb.velocity.y < 0.2f)
                {
                    isJumping = false;
                    this.transform.SetParent(hit.collider.transform.parent);
                }
            }
        }
        public void IceJump()
        {
            rb.isKinematic = false;
            isJumping = true;
            rb.AddForce(Vector2.up * iceForce, ForceMode2D.Impulse);
            rb.AddForce(IceDirection(), ForceMode2D.Impulse);
        }
        public Vector2 IceDirection()
        {
            int rand = Random.Range(0, 3);
            if (rand == 0)
            {
                return Vector2.right * 0.1f;
            }
            else
            {
                return Vector2.left * -0.1f;
            }
        }
        /*private void OnTriggerExit2D(Collider2D other)
        {
            CupGame21 cup = other.GetComponent<CupGame21>();
            if (cup != null)
            {
                transform.SetParent(cup.transform);
                cup.ice = this.GetComponent<IceGame21>();
                cup.ChangeState();
                cup.rb.isKinematic = false;
                Observer.Notify(EventAction.EVENT_GET_SCORE, true);
            }
        }*/

        private void OnCollisionEnter2D(Collision2D other)
        {
            Tag tag = other.gameObject.GetComponent<Tag>();
            if (tag != null && tag.tagName == "Tray")
            {
                Debug.Log("Tray ________");
                collider.isTrigger = true;
            }
            if (tag != null && tag.tagName == "Cup")
            {
                Debug.Log("Cup ________");
               rb.isKinematic = true;
            }
        }
    }
}