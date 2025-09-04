using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class NpcGame6Tut : NpcGame6
    {
        public override void GetOrders()
        {
            orders.Add(FOODTYPE.BURGER_TYPE_3);
            orders.Add(FOODTYPE.BURGER_TYPE_1);
            orders.Add(FOODTYPE.BURGER_TYPE_2);
        }

        public override void SetPositionOrder()
        {
            sprFood1.transform.localPosition = new Vector3(0.3f, 0, 0);
            sprFood2.transform.localPosition = new Vector3(1.15f, 0, 0);
            sprFood3.transform.localPosition = new Vector3(2, 0, 0);
            sprFood1.sprite = Resources.Load<Sprite>($"Art/game5/hard_mode/Harmburger_{FoodSpriteShowed(orders[0])}");
            sprFood2.sprite = Resources.Load<Sprite>($"Art/game5/hard_mode/Harmburger_{FoodSpriteShowed(orders[1])}");
            sprFood3.sprite = Resources.Load<Sprite>($"Art/game5/hard_mode/Harmburger_{FoodSpriteShowed(orders[2])}");
        }
    }
}