using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DevDuck
{
    public class NpcGame6 : MonoBehaviour
    {
        public List<FOODTYPE> orders = new List<FOODTYPE>();
        [SerializeField] private int amountOrder;
        public GameObject box1Order, box2Order, box3Order,tempOrder;
        public SpriteRenderer sprFood1, sprFood2, sprFood3;
        private void Start()
        {
            GetOrders();
        }
        public void ShowOrder()
        {
            if (amountOrder == 1)
            {
                sprFood1.sprite = Resources.Load<Sprite>($"Art/game5/hard_mode/Harmburger_{FoodSpriteShowed(orders[0])}");
                sprFood1.transform.SetParent(box1Order.transform);
                sprFood1.gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                box1Order.SetActive(true);
                box1Order.transform.DOScale(0.7f, 0.3f).From(0);
                tempOrder = box1Order;
            }
            else if (amountOrder == 2)
            {
                sprFood1.sprite = Resources.Load<Sprite>($"Art/game5/hard_mode/Harmburger_{FoodSpriteShowed(orders[0])}");
                sprFood2.sprite = Resources.Load<Sprite>($"Art/game5/hard_mode/Harmburger_{FoodSpriteShowed(orders[1])}");
                sprFood1.transform.SetParent(box2Order.transform);
                sprFood2.transform.SetParent(box2Order.transform);
                sprFood1.gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                sprFood2.gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                box2Order.SetActive(true);
                box2Order.transform.DOScale(0.7f, 0.3f).From(0);
                tempOrder = box2Order;

            }
            else
            {
                sprFood1.sprite = Resources.Load<Sprite>($"Art/game5/hard_mode/Harmburger_{FoodSpriteShowed(orders[0])}");
                sprFood2.sprite = Resources.Load<Sprite>($"Art/game5/hard_mode/Harmburger_{FoodSpriteShowed(orders[1])}");
                sprFood3.sprite = Resources.Load<Sprite>($"Art/game5/hard_mode/Harmburger_{FoodSpriteShowed(orders[2])}");
                sprFood1.transform.SetParent(box3Order.transform);
                sprFood2.transform.SetParent(box3Order.transform);
                sprFood3.transform.SetParent(box3Order.transform);
                sprFood1.gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                sprFood2.gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                sprFood3.gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                box3Order.SetActive(true);
                box3Order.transform.DOScale(0.7f, 0.3f).From(0);
                tempOrder = box3Order;

            }
            SetPositionOrder();
        }
        public virtual void GetOrders()
        { 
            amountOrder = Duck.GetRandom(1, 4);
            if (amountOrder == 1)
            {
                orders.Add(AddOrder());
            }
            else if (amountOrder == 2)
            {
                orders.Add(AddOrder());
                orders.Add(AddOrder());
            }
            else
            {
                orders.Add(AddOrder());
                orders.Add(AddOrder()); 
                orders.Add(AddOrder());
            }
        }
        public FOODTYPE AddOrder()
        {
            int burger = Duck.GetRandom(1, 5);
            switch (burger)
            {
                case 1:
                    return FOODTYPE.BURGER_TYPE_1;
                    break;
                case 2:
                    return FOODTYPE.BURGER_TYPE_2;
                    break;
                case 3:
                    return FOODTYPE.BURGER_TYPE_3;
                    break;
                case 4:
                    return FOODTYPE.BURGER_TYPE_4;
                    break;
                default:
                    return FOODTYPE.NONE;
                    break;
            }
        }
        public int FoodSpriteShowed( FOODTYPE type)
        {
            switch (type)
            {
                case FOODTYPE.BURGER_TYPE_1:
                    return 1;
                case FOODTYPE.BURGER_TYPE_2:
                    return 2;
                case FOODTYPE.BURGER_TYPE_3:
                    return 3;
                case FOODTYPE.BURGER_TYPE_4:
                    return 4;
                default:
                    return -1;
            }
        }
        public virtual void SetPositionOrder()
        {
            if (amountOrder == 1)
            {
                sprFood1.transform.localPosition = new Vector3(.25f, 0, 0);
            }
            else if (amountOrder == 2)
            {
                sprFood1.transform.localPosition = new Vector3(0.3f, 0, 0);
                sprFood2.transform.localPosition = new Vector3(1, 0, 0);
            }
            else
            {
                sprFood1.transform.localPosition = new Vector3(0.3f, 0, 0);
                sprFood2.transform.localPosition = new Vector3(1.15f, 0, 0);
                sprFood3.transform.localPosition = new Vector3(2, 0, 0);
            }
        }

        public void HideOrder()
        {
            tempOrder.SetActive(false);
        }
        public void ShowTick(FOODTYPE type)
        {
            int a = FoodSpriteShowed(type);
            if (sprFood1.sprite.name == $"Harmburger_{a}" && sprFood1.transform.childCount == 0)
            {
                DOVirtual.DelayedCall(0.8f, () => InitTick(sprFood1.gameObject));
             //   LogicUiGame5.Instance.ShowTickImage(sprFood1.gameObject);
            }
            else if (sprFood2.sprite.name == $"Harmburger_{a}" && sprFood2.transform.childCount == 0)
            {
                DOVirtual.DelayedCall(0.8f, () => InitTick(sprFood2.gameObject));
              //  LogicUiGame5.Instance.ShowTickImage(sprFood2.gameObject);
            }
            else if (sprFood3.sprite.name == $"Harmburger_{a}" && sprFood3.transform.childCount == 0)
            {
                DOVirtual.DelayedCall(0.8f, () => InitTick(sprFood3.gameObject));
               // LogicUiGame5.Instance.ShowTickImage(sprFood3.gameObject);
            }
        }

        public void InitTick(GameObject o)
        {
            GameObject a = Instantiate(LogicUiGame5.Instance.tickObject, o.transform.position,Quaternion.identity); 
            a.transform.SetParent(o.transform);
            a.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
            a.transform.localPosition = new Vector3(-.5f, 0f, 0f);
            a.GetComponent<SpriteRenderer>().sortingOrder = 3;
            a.GetComponent<Animator>().Play("tickAnimation");

        }
    }
}