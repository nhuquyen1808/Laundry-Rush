using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class ModelFashion : MonoBehaviour
    {
        public FashionType fashionType;
        public int id;
        private SpriteRenderer spriteRendererFront;
        public ItemFashionInit itemFashionInit;
        public SpriteRenderer SpriteRendererFront
        {
            get
            {
                if (spriteRendererFront == null)
                {
                    for (int i = 0; i < gameObject.transform.childCount; i++)
                    {
                        if (gameObject.transform.GetChild(0).transform.name == "Back")
                        {
                            if(transform.GetChild(i).name == "Front") spriteRendererFront = gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>();
                            else if(transform.GetChild(i).name == "Back")  spriteRendererBack = gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>();
                        }
                    }
                }

                return spriteRendererFront;
            }
        }
        private SpriteRenderer spriteRendererBack;
        public SpriteRenderer SpriteRendererBack
        {
            get
            {
                if (spriteRendererFront == null)
                {
                    for (int i = 0; i < gameObject.transform.childCount; i++)
                    {
                        if (gameObject.transform.GetChild(0).transform.name == "Back")
                        {
                            if(transform.GetChild(i).name == "Front") spriteRendererFront = gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>();
                            else if(transform.GetChild(i).name == "Back")  spriteRendererBack = gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>();
                        }
                    }
                }

                return spriteRendererBack;
            }
        }
        public void Init(int id)
        {
            Sprite spriteFront = Resources.Load<Sprite>($"Art/_FashionItems/{id}");
            if(spriteFront != null) spriteRendererFront.sprite = spriteFront;
            else
            {
                Debug.LogError("Sprite front not found");
            }
            
        }

        private BoxCollider2D collider;

        public BoxCollider2D Collider
        {
            get
            {
                if (collider == null)
                {
                    collider = GetComponent<BoxCollider2D>();
                }

                return collider;
            }
        }
    }
}