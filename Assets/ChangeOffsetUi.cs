using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class ChangeOffsetUi : MonoBehaviour
    {
        float scrollSpeed = 0.2f;
        private Image bg;

        void Start()
        {
            bg  = GetComponent<Image>();

        }

        void Update()
        {
            float offset = Time.time * scrollSpeed;
            bg.material.SetTextureOffset("_BaseMap", new Vector2(offset, offset));
        }
    }
}
