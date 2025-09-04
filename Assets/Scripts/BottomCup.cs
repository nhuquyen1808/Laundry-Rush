using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class BottomCup : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            Tag tag = other.gameObject.GetComponent<Tag>();
            if (tag != null && tag.tagName == "ICE")
            {
               // tag.GetComponent<Rigidbody2D>().isKinematic = true;
            }
        }
    }
}
