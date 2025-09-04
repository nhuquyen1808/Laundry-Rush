using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

namespace DevDuck
{
    public enum PETALTYPE
    {
        BLUE,
        PINK,
        DARKBLUE,
        DARKPINK
    }

    public enum DIRECTION
    {
        RIGHT,
        LEFT,
        UP,
        DOWN
    }
    public enum DISTANCE_HIT
    {
        LONG_HIT,
        SHORT_HIT
    }
    public class Petal : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        public PETALTYPE PetalType;
        public DIRECTION direction;
        public DISTANCE_HIT distanceHit;
        private RaycastHit2D[] hit;
        public Animator strink;
        public bool isDone;

        public SpriteRenderer SpriteRenderer
        {
            get
            {
                if (spriteRenderer == null)
                {
                    spriteRenderer = GetComponent<SpriteRenderer>();
                    if (spriteRenderer == null)
                    {
                        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                    }
                }

                return spriteRenderer;
            }
        }

        private void Start()
        {
            SetDarkSprite();
          
        }

        public void SetDarkSprite()
        {
            if (PetalType == PETALTYPE.BLUE)
            {
                SpriteRenderer.sprite = Resources.Load<Sprite>("Art/game12/petal_bluegrey");
            }
            else if (PetalType == PETALTYPE.PINK)
            {
                SpriteRenderer.sprite = Resources.Load<Sprite>("Art/game12/petal_pinkgrey");
            }
        }

        public void SetLightSprite()
        {
            if (PetalType == PETALTYPE.BLUE)
            {
                SpriteRenderer.sprite = Resources.Load<Sprite>("Art/game12/petal_blue");
            }
            else if (PetalType == PETALTYPE.PINK)
            {
                SpriteRenderer.sprite = Resources.Load<Sprite>("Art/game12/petal_pink");
            }
        }

        public int DistanceHit()
        {
            if (distanceHit == DISTANCE_HIT.LONG_HIT)
            {
                return 5;
            }
            else
            {
                return 3;
            }
        }

        public Vector2 Direction()
        {
            if (direction == DIRECTION.RIGHT) return Vector2.right;

            else if (direction == DIRECTION.LEFT) return Vector2.left;
            else if (direction == DIRECTION.UP) return Vector2.up;
            else return Vector2.down;
        }

        public void Hit()
        {
            hit = Physics2D.RaycastAll(transform.position, transform.up, DistanceHit(),
                Level_1_Game12.instance.nostrokes);
            if (hit.Length > 1)
            {
                //  Debug.Log(hit[1].collider.gameObject.name + "     " + hit[1].distance + "    " + hit[1].collider.gameObject.transform.parent.name);
                Petal o = hit[1].collider.gameObject.GetComponent<Petal>();
                if (o != null)
                {
                    EnableBoxCollider(o);
                    if (o.PetalType == PetalType)
                    {
                        Debug.Log(o.PetalType.ToString());
                        if (strink != null && o.strink == null)
                        {
                            ShowAndHide(true);
                            DOVirtual.DelayedCall(0.05f, SetLightSprite);
                            DOVirtual.DelayedCall(1f, o.SetLightSprite);
                            isDone = true;
                        }
                        if (strink == null && o.strink != null)
                        {
                            if (ManagerAnimator.HasAnimation(o.strink.GetComponent<Animator>(),
                                    "Short_pink_strink_show"))
                                o.strink.GetComponent<Animator>().Play("Short_pink_strink_show", 0, 0);
                            if (ManagerAnimator.HasAnimation(o.strink.GetComponent<Animator>(), "strink_show"))
                                o.strink.GetComponent<Animator>().Play("strink_show", 0, 0);
                            DOVirtual.DelayedCall(1f, SetLightSprite);
                            DOVirtual.DelayedCall(0.05f, o.SetLightSprite);
                            o.isDone = true;
                        }
                    }
                    else
                    {
                        SetDarkSprite();
                        o.SetDarkSprite();
                        if (strink == null) return;
                        if (strink != null && strink.GetComponent<SpriteRenderer>().size != new Vector2(SizeX(), 0f))
                        {
                            ShowAndHide(false);
                            isDone = true;
                        }
                        if (o.strink == null) return;
                        if (o.strink != null && o.strink.GetComponent<SpriteRenderer>().size != new Vector2(SizeX(), 0f))
                        {
                            if (ManagerAnimator.HasAnimation(o.strink, "Short_pink_strink_hide"))
                                o.strink.Play("Short_pink_strink_hide", 0, 0);
                            if (ManagerAnimator.HasAnimation(o.strink, "strink_hide"))
                                o.strink.Play("strink_hide", 0, 0);
                            o.isDone = true;
                        }
                    }
                }
                PetaFinalPiece finalPiece = hit[1].collider.GetComponent<PetaFinalPiece>();
                if (finalPiece != null)
                {
                    ShowAndHide(true);
                    finalPiece.isDone = true;
                }
            }
            else
            {
                isDone = false;
                ShowAndHide(false);
            }
        }

        public void ShowAndHide(bool isShow)
        {
            if (strink == null) return;
            if (isShow)
            {
                if (ManagerAnimator.HasAnimation(strink, "Short_pink_strink_show") && strink.GetComponent<SpriteRenderer>().size == new Vector2(SizeX(), 0f))
                {
                    strink.Play("Short_pink_strink_show", 0, 0);
                }

                if (ManagerAnimator.HasAnimation(strink, "strink_show")&& strink.GetComponent<SpriteRenderer>().size == new Vector2(SizeX(), 0f))
                {
                    strink.Play("strink_show", 0, 0);
                }
            }
            else
            {
                if (ManagerAnimator.HasAnimation(strink, "Short_pink_strink_hide")&& strink.GetComponent<SpriteRenderer>().size != new Vector2(SizeX(), 0f))
                {
                    strink.Play("Short_pink_strink_hide", 0, 0);
                }

                if (ManagerAnimator.HasAnimation(strink, "strink_hide")&& strink.GetComponent<SpriteRenderer>().size != new Vector2(SizeX(), 0f))
                {
                    strink.Play("strink_hide", 0, 0);
                }
            }
        }

        public void EnableBoxCollider(Petal petal)
        {
            if (this.transform.parent.GetComponent<BoxCollider2D>().enabled == true)
            {
                petal.gameObject.transform.parent.GetComponent<BoxCollider2D>().enabled = true;
            }
            else
            {
                petal.gameObject.transform.parent.GetComponent<BoxCollider2D>().enabled = false;
            }
        }

        public void ShowDone()
        {
            if (strink == null)
            {
                SetLightSprite();
            }
        }
        private void OnDrawGizmos()
        {
            Debug.DrawRay(transform.position, transform.up * 3f, Color.red);
           
        }
        public float SizeX()
        {
            if (distanceHit == DISTANCE_HIT.LONG_HIT)
            {
                return 0.8f;
            }
            else
            {
                return 0.58f;
            }
        }
    }
}