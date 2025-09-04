using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace dinhvt.Level18
{
    public class LogicDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public bool canDrag = true;

        [SerializeField] protected LayerMask dragLayer;
        [SerializeField] protected float radius = 0.3f;
        protected Camera mainCamera;
        protected Vector3 touchPosition;
        protected Vector3 offset;
        protected float angleOffset;
        protected RaycastHit2D[] hitInfo;
        protected Transform hitTransform;
        protected ISelectableObject selectableObject;


        protected virtual void Start()
        {
            //Application.targetFrameRate = 60;
            mainCamera = Camera.main;
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (!canDrag) return;
            UpdateTouchPosition(ref touchPosition);
            SelectObject();
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (!canDrag) return;
            UpdateTouchPosition(ref touchPosition);
        }
        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (hitTransform != null) ReleaseObject();
            if (!canDrag) return;
            UpdateTouchPosition(ref touchPosition);
        }

        protected virtual void SelectObject()
        {
            UpdateTouchPosition(ref touchPosition);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            hitInfo = Physics2D.CircleCastAll(ray.origin, radius, ray.direction, Mathf.Infinity, dragLayer);

            //hitInfo = Physics2D.CircleCastAll(touchPosition, radius, ray.direction, Mathf.Infinity, dragLayer);

            int hitSortingOrder, maxSortingOrder = 0;
            SpriteRenderer spriteRenderer;
            if (hitInfo.Length != 0) hitTransform = hitInfo[0].transform;

            foreach (var hit in hitInfo)
            {
                if (hit.transform.TryGetComponent<SpriteRenderer>(out spriteRenderer))
                {
                    hitSortingOrder = spriteRenderer.sortingOrder;
                    if (hitSortingOrder >= maxSortingOrder)
                    {
                        hitTransform = hit.transform;
                        maxSortingOrder = hitSortingOrder;
                    }
                }
            }

            if (hitTransform != null)
            {
                if (hitTransform.TryGetComponent<ISelectableObject>(out selectableObject))
                {
                    selectableObject.OnSelect();
                }
            }
        }

        


        protected virtual void ReleaseObject()
        {
            hitTransform = null;

            if (selectableObject != null)
            {
                UpdateTouchPosition(ref touchPosition);
                selectableObject?.OnDeselect();
                selectableObject = null;
            }
        }

        protected virtual void ToggleDragAbility(object sender, object result)
        {
            canDrag = (bool)result;
        }

        protected void UpdateTouchPosition(ref Vector3 touchPosition)
        {
            if (
#if UNITY_EDITOR
                true
#else
                Input.touchCount >= 1
#endif
                )
            {
#if UNITY_EDITOR
                touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
#else                  
                touchPosition =  Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
#endif
            }

            touchPosition.z = 0f;
        }
    }
}

