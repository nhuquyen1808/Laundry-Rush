using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class PositionGame14 : MonoBehaviour
    {
        public bool isHasPiece;
        private RaycastHit2D hit;
        public int id;
        public bool HitSomething()
        {
            hit = Physics2D.Raycast(transform.position, -Vector3.forward*100);
            if (hit.collider != null)
            {
                isHasPiece = true;
                return true;
            }
            else
            {
                Debug.Log("id Null :  " +this.id);
                isHasPiece = false;
                return false;
            }
        }

        private void OnDrawGizmos()
        {
            Debug.DrawRay(transform.position, -Vector3.forward*100);
        }
    }
}
