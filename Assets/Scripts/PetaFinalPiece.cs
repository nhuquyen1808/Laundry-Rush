using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.Rendering;
using UnityEngine;

namespace DevDuck
{
    public enum State
    {
        NONE,
        CHECK
    }

    public class PetaFinalPiece : MonoBehaviour
    {
        public PETALTYPE petalTypeFinal;
        public State state;
        public SpriteRenderer smallHeart;
        public bool isDone;
        private SpriteRenderer _Sprite;
        [SerializeField] private GameObject strink;

        public SpriteRenderer SpriteRenderer
        {
            get
            {
                if (_Sprite == null)
                {
                    _Sprite = GetComponent<SpriteRenderer>();
                }

                return _Sprite;
            }
        }

        RaycastHit2D[] hits;

        private void OnDrawGizmos()
        {
            Debug.DrawRay(transform.position, transform.up * 5f, Color.green);
        }

        public void Check()
        {
            hits = Physics2D.RaycastAll(transform.position, transform.up, 3f, Level_1_Game12.instance.nostrokes);
            if (hits.Length > 1)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[1].transform.GetComponent<Petal>().PetalType == petalTypeFinal)
                    {
                        if (hits[1].transform.GetComponent<Petal>().strink != null)
                        {
                            hits[1].transform.GetComponent<Petal>().isDone = true;
                            hits[1].transform.GetComponent<Petal>().SetLightSprite();
                        }

                        DOVirtual.DelayedCall(1, SetLightSprite);
                    }
                    else
                    {
                        SetGreySprite();
                        if (state == State.CHECK) isDone = false;
                    }
                }
            }
            else
            {
                SetGreySprite();
                if (state == State.CHECK) isDone = false;
            }
        }

        public void SetLightSprite()
        {
            //   if (isDone == false)
            {
                smallHeart.sprite = Resources.Load<Sprite>("Art/game12/petal_heart_green");
                if (petalTypeFinal == PETALTYPE.BLUE)
                {
                    SpriteRenderer.sprite = Resources.Load<Sprite>("Art/game12/petal_heart_blue");
                }
                else
                {
                    SpriteRenderer.sprite = Resources.Load<Sprite>("Art/game12/petal_heart_pink");
                }
            }
        }

        private void SetGreySprite()
        {
            //  if (isDone == false)
            {
                smallHeart.sprite = Resources.Load<Sprite>("Art/game12/petal_heart_grey");
                if (petalTypeFinal == PETALTYPE.BLUE)
                {
                    SpriteRenderer.sprite = Resources.Load<Sprite>("Art/game12/petal_heart_bluegrey");
                }
                else
                {
                    SpriteRenderer.sprite = Resources.Load<Sprite>("Art/game12/petal_heart_pinkgrey");
                }
            }
        }
    }
}