using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DevDuck
{
    public class CupPapa : MonoBehaviour
    {
        public CUPTYPE type;
        public Tween tween;
        public CupGame21 cupChild;
        public IceGame21 iceChild;
        public Rigidbody2D rb;
        public float force;
        public float direction,speed;
        private Vector2 startPos;
        public float moveDistance = 3f;
        void Start()
        {
            MoveCup();
            startPos = rb.position;
        }

        private void FixedUpdate()
        {
            /*if (Mathf.Abs(rb.position.x - startPos.x) >= moveDistance)
            {
                direction *= -1;
            }
            if (rb.position.x <= startPos.x)
            {
                direction = 1;
            }
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);*/
        }

        public void Jump()
        {
            rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            //  cupChild.rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            iceChild.transform.SetParent(transform.parent);
            iceChild.IceJump();
        }

        private void ShakeCup()
        {
            int rand = Random.Range(0, 2);
            if (rand == 0)
            {
                transform.Rotate(0, 0, 5, Space.Self);
            }
            else
            {
                transform.Rotate(0, 0, -5, Space.Self);
            }
        }

        private void MoveCup()
        {
            if (type == CUPTYPE.NORMAL)
            {
                float leftX = transform.position.x - 2;
                float rightX = transform.position.x + 5;
                //rb.AddForce(Vector2.right * force, ForceMode2D.Impulse); 
                transform.DOMoveX(leftX, 0.5f).OnComplete((() =>
                {
                    transform.DOMoveX(rightX, 1f).SetLoops(-1, LoopType.Yoyo);
                }));
            }
        }
    }
}