using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dinhvt.Level18
{

    public class Block : MonoBehaviour, ISelectableObject
    {
        public int id;
        public bool isMoving = false;
        [SerializeField] AnimationCurve curveAnim;

        public Color visibleColor;
        public Color invisibleColor;

        public List<Block> upperBlocks = new List<Block>();
        public int row, col;
        public bool canSelect;
        [HideInInspector] public bool isSelected = false;

        private bool canDisableBlock = false;
        private Vector3 startPos;
        private int sortingOrder;

        [HideInInspector] public SpriteRenderer spriteRenderer;
        [HideInInspector] public Collider2D col2D;

        Tween addToSlotTween;
        bool canCall = true;
        Action checkMatch;

        private void Awake()
        {   
            spriteRenderer = GetComponent<SpriteRenderer>();
            col2D = GetComponent<Collider2D>();
        }

        private void Start()
        {
            
            startPos = transform.position;
        }

        public void Init(int id, Sprite sprite)
        {
            this.id = id;
            this.spriteRenderer.sprite = sprite;
        }
        
        //public void CheckOverlap()
        //{
        //    RaycastHit2D raycastHit2D = Physics2D.BoxCast(transform.position, col2D.bounds.size, 0f, Vector3.back);

        //    isVisible = raycastHit2D.collider == null;
        //    spriteRenderer.color = isVisible ? visibleColor : invisibleColor;
        //}

        public void OnSelect()
        {
            if (GameManager.instance.IsFullSlot() ||!canSelect) return;

            spriteRenderer.sortingOrder++;
            transform.localScale = Vector3.one * 0.8f;
        }

        public void OnDeselect()
        {
            if (GameManager.instance.IsFullSlot() || !canSelect) return;

            canSelect = false;
            isSelected = true;
            GridBuilder.instance.UpdateBlockOverlap();

            transform.localScale = Vector3.one;
            GameManager.instance.SelectBlock(this);
        }

        public void AddToSlot(Slot slot, int sortingOrder, Action action)
        {
            checkMatch = action;
            Move(slot.transform.position, sortingOrder, 0.5f);
        }

        
        public void MoveToSlot(Slot slot, int sortingOrder)
        {           
            bool isKillTween = false;
            if (addToSlotTween != null && addToSlotTween.IsActive())
            {
                addToSlotTween.Kill();
                isKillTween = true;
            }

            spriteRenderer.sortingOrder = sortingOrder;

            transform.DOMove(slot.transform.position, isKillTween ? 0.35f : 0.2f).OnComplete(() => {
                PlayAnimation();
            });
        }

        public void Move(Vector3 targetPos, int sortingOrder, float moveTime)
        {
            Vector3 distance = targetPos - startPos;

            spriteRenderer.sortingOrder = sortingOrder;

            transform.DOScale(GameManager.instance.dataLoader.scaleValue, 0.3f);

            isMoving = true;
            addToSlotTween = DOTween.To(() => 0f, val =>
            {
                float eval = curveAnim.Evaluate(val);
                transform.position = startPos + new Vector3(distance.x * val, distance.y * eval, 0f);
            }, 1f, moveTime)
                .OnComplete(() =>
                {
                    PlayAnimation();
                });
        }

        private void PlayAnimation()
        {
            if (!canCall) return;
            canCall = false;
            canDisableBlock = true;
            isMoving = false;
            checkMatch?.Invoke();
        }

        public void DisableAnim(Block middleBlock, Action callback = null)
        {   
            StartCoroutine(DisableAnimation(middleBlock, callback));
        }

        //public IEnumerator MoveToPosition(Vector3 targetPos, float duration)
        //{
        //    Vector3 distance = targetPos - startPos;
        //    float elapsed = 0f;

        //    while (elapsed < duration)
        //    {
        //        elapsed += Time.deltaTime;
        //        float val = elapsed / duration;
        //        float eval = curveAnim.Evaluate(val);
        //        transform.position = startPos + new Vector3(distance.x * val, distance.y * eval, 0f);
        //        yield return null;
        //    }

        //    transform.position = targetPos;
        //}


        IEnumerator DisableAnimation(Block middleBlock, Action callback)
        {
            while (!canDisableBlock)
            {
                yield return null;
            }

            Sequence sequence = DOTween.Sequence();

            //float targetPosX = transform.position.x;

            //if (middleBlock != this)
            //{
            //    float middleBlockPosX = middleBlock.transform.position.x;
            //    targetPosX += (transform.position.x > middleBlockPosX) ? 0.2f : -0.2f;
            //}
            //sequence.Append(transform.DOMove(new Vector3(targetPosX, -1.9f), 0.2f));

            //if (middleBlock == this)
            //{
            //    spriteRenderer.sortingOrder++;
            //    //yield return new WaitForSeconds(0.3f);
            //    sequence.Append(transform.DOScale(Vector3.one * 1.2f, 0.2f));
            //    sequence.Append(transform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
            //    {
            //        spriteRenderer.enabled = false;
            //    }));
            //}
            //else
            //{
            //    sequence.Append(transform.DOMoveX(middleBlock.transform.position.x, 0.2f).SetEase(Ease.InBack).OnComplete(() =>
            //    {
            //        callback?.Invoke();
            //        gameObject.SetActive(false);
            //    }));
            //}

            sequence.Append(transform.DOScale(Vector3.zero, 0.2f)).OnComplete(() =>
            {
                if (middleBlock != this)
                {
                    callback?.Invoke();
                }
                gameObject.SetActive(false);
            });
        }

        public void SetCanSelect(bool isVisible)
        {
            canSelect = isVisible;
            spriteRenderer.color = isVisible ? visibleColor : invisibleColor;
        }

        public void SetSortingOrder(int sortingOrder)
        {   
            this.sortingOrder = sortingOrder;
            spriteRenderer.sortingOrder = this.sortingOrder;
        }

        public void ResetTransform(Action action)
        {
            isSelected = false;
            canCall = true;
            canDisableBlock = false;
            transform.DOScale(Vector3.one, 0.2f);
            transform.DOMove(startPos, 0.2f).OnComplete(() =>
            {
                spriteRenderer.sortingOrder = this.sortingOrder;
                action?.Invoke();
                canSelect = true;
            });
            
        }
    }
}