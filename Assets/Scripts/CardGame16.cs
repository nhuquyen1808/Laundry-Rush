using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class CardGame16 : MonoBehaviour
    {
        public int id;
        private SpriteRenderer spriteRenderer;

        public SpriteRenderer SpriteRenderer
        {
            get
            {
                if (spriteRenderer == null)
                {
                    spriteRenderer = GetComponent<SpriteRenderer>();
                }
                return spriteRenderer;
            }
        }

        private BoxCollider2D box;

        public BoxCollider2D BoxCollider2D
        {
            get
            {
                if (box == null)
                {
                    box = GetComponent<BoxCollider2D>();
                }
                return box;
            }
        }

        public void SetSpriteAndId(int id)
        {
            SpriteRenderer.sprite = Resources.Load<Sprite>($"Art/game16/Cards_{id}"  );
            this.id = id;
        }

        public void EnableBoxCollider()
        {
            BoxCollider2D.enabled = true;
        }

        public void DisableBoxCollider()
        {
            BoxCollider2D.enabled = false;
        }
    }
}
