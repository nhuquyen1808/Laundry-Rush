using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DevDuck
{
    public class LimitPlayerMoveGame4 : MonoBehaviour
    {
        public EdgeCollider2D edge;
        public Vector2[] edgePoints;
        public List<GameObject> posLimit = new List<GameObject>();
        public List<Vector2> pos  = new List<Vector2>();
        // Start is called before the first frame update
        void Start()
        {
            edge = GetComponent<EdgeCollider2D>() == null ? gameObject.AddComponent<EdgeCollider2D>() : GetComponent<EdgeCollider2D>();
            edgePoints = new Vector2 [6];
            for (int i = 0; i < posLimit.Count; i++)
            {
                pos.Add(posLimit[i].transform.position);
            }

            edgePoints = pos.ToArray();
            edge.points = edgePoints;
        }

    }
}
