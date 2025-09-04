using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class TestInScene : MonoBehaviour
    {
        public BoxCollider boxCollider;

        private bool canInsCoin;
        // Update is called once per frame
        void Update()
        {
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(boxCollider.transform.position);
            if ((viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1) && !canInsCoin)
            {
                Debug.Log("Object đã ra khỏi màn hình");
            }

        }
    }
}
