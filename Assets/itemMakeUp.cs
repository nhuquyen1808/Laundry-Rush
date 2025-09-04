using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public enum MakeupType
    {
        EYES_LINE, EYES_LEN,LIP, BLUSH
    }
    public class itemMakeUp : MonoBehaviour
    
    {
        public int id;
        public MakeupType makeupType;
        private BoxCollider2D _boxCollider2D;

        public BoxCollider2D BoxCollider2D
        {
            get
            {
                if (_boxCollider2D == null)
                {
                    _boxCollider2D = GetComponent<BoxCollider2D>();
                }
                return _boxCollider2D;
            }
        }


        public void OnItemSelected()
        {
            BoxCollider2D.enabled = false;
        }

        public void OnItemReleased()
        {
            BoxCollider2D.enabled = true;
        }
    }
}
