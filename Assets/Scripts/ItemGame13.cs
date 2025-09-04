using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public enum SHAPE
    {
        SHAPE1,SHAPE2,SHAPE3,SHAPE4,SHAPE5
    }
    public class ItemGame13 : MonoBehaviour
    {
        public SHAPE Shape;
        public int id;
        PolygonCollider2D _collider;
        public bool isDone = false;
        public Vector3 startPosition;

        private void Start()
        {
            startPosition = this.transform.position;
        }

        public PolygonCollider2D Collider2d
        {
            get
            {
                if (_collider == null)
                {
                    _collider = GetComponent<PolygonCollider2D>();
                }
                return _collider;
            }
        }

        public void DisableCollider()
        {
            Collider2d.enabled = false;
            isDone = true;
        }
    }
}
