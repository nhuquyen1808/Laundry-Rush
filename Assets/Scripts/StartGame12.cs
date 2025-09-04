using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DevDuck
{
    public class StartGame12 : MonoBehaviour
    {
        public PETALTYPE startType;
        public DIRECTION dir;
        RaycastHit2D hit;
        public LayerMask mask;
        public Animator strinkAnimator;
        public DISTANCE_HIT DistanceHit;
        public void CheckType()
        {
            hit = Physics2D.Raycast(transform.position, GetDirection(), 3, mask);
            if (hit.collider != null)
            {
                Petal petal = hit.collider.gameObject.GetComponent<Petal>();
                if (petal != null)
                {
                    if (petal.PetalType == startType)
                    {
                        petal.isDone = true;
                        DOVirtual.DelayedCall(1, petal.SetLightSprite);
                        ShowEffectGetPetal();
                        ShowAndHide(true);

                    }
                    else
                    {
                        if (petal.strink != null)
                        {
                        petal.isDone = false;
                            
                        }
                        petal.SetDarkSprite();

                        ShowAndHide(false);
                    }
                }
            }
            else
            {
                ShowAndHide(false);

                /*if (ManagerAnimator.HasAnimation(strinkAnimator, "Short_pink_strink_hide"))
                {
                    strinkAnimator.Play("Short_pink_strink_hide", 0, 0);
                }*/
            }
        }

        private Vector2 GetDirection()
        {
            switch (dir)
            {
                case DIRECTION.UP:
                    return Vector2.up;
                    break;
                case DIRECTION.DOWN:
                    return Vector2.down;
                    break;
                case DIRECTION.LEFT:
                    return Vector2.left;
                    break;
                case DIRECTION.RIGHT:
                    return Vector2.right;
                    break;
                default:
                    return Vector2.zero;
            }
        }

        private void OnDrawGizmos()
        {
            Debug.DrawRay(transform.position, transform.up * 3f, Color.red);
            Debug.DrawRay(transform.position, -transform.up * 3f, Color.red);
            Debug.DrawRay(transform.position, -transform.right * 3f, Color.red);
            Debug.DrawRay(transform.position, transform.right * 3f, Color.red);
        }

        public void ShowEffectGetPetal()
        {
            transform.DOScale(1.1f, 0.2f).OnComplete(() =>
            {
                transform.DOScale(1f, 0.2f);
            });
        }
        public void ShowAndHide(bool isShow)
        {
            if (strinkAnimator == null) return;
            if (isShow)
            {
                if (ManagerAnimator.HasAnimation(strinkAnimator, "Short_pink_strink_show") && strinkAnimator.GetComponent<SpriteRenderer>().size == new Vector2(SizeX(), 0f))
                {
                    strinkAnimator.Play("Short_pink_strink_show", 0, 0);
                }

                if (ManagerAnimator.HasAnimation(strinkAnimator, "strink_show")&& strinkAnimator.GetComponent<SpriteRenderer>().size == new Vector2(SizeX(), 0f))
                {
                    strinkAnimator.Play("strink_show", 0, 0);
                }
            }
            else
            {
                if (ManagerAnimator.HasAnimation(strinkAnimator, "Short_pink_strink_hide")&& strinkAnimator.GetComponent<SpriteRenderer>().size != new Vector2(SizeX(), 0f))
                {
                    strinkAnimator.Play("Short_pink_strink_hide", 0, 0);
                }

                if (ManagerAnimator.HasAnimation(strinkAnimator, "strink_hide")&& strinkAnimator.GetComponent<SpriteRenderer>().size != new Vector2(SizeX(), 0f))
                {
                    strinkAnimator.Play("strink_hide", 0, 0);
                }
            }
        }
        public float SizeX()
        {
            if (DistanceHit == DISTANCE_HIT.LONG_HIT)
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