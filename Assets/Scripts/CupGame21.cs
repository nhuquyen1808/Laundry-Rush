using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DevDuck
{
    public enum CUPTYPE
    {
        NORMAL,WHEELCUP,SPRINGCUP,COCTAIL
    }
    public class CupGame21 : MonoBehaviour
    {
        public BoxCollider2D leftCollider, rightCollider, bottomCollider;
        public IceGame21 ice;
        public float force,iceForce;
        private void OnTriggerExit2D(Collider2D other)
        {
            Tag objectTag = other.gameObject.GetComponent<Tag>();
            if (objectTag != null && objectTag.tag == "ICE")
            {
                ChangeState();
                Debug.Log("Change State");
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Tag objectTag = other.gameObject.GetComponent<Tag>();
            if (tag != null && objectTag.tag == "ICE")
            {
                Debug.Log("Change State ?????????" );
            }
    }

        public void ChangeState()
        {
            if (bottomCollider.isTrigger)
            {
                bottomCollider.isTrigger = false;
                leftCollider.isTrigger = false;
                rightCollider.isTrigger = false;
            }
        }
    }
}