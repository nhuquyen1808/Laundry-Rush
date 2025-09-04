using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DevDuck
{
    public class RightButtonGame1 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        bool isPressed;
        [FormerlySerializedAs("shadowObj")] public ShadowControl shadowControlObj;
        [SerializeField] Sprite pressedUi, normalUi;
        Image img;

        private void Start()
        {
            img = GetComponent<Image>();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            isPressed = true;
            LogicGame1.instance.clawMachineHandle.AnimationState.SetAnimation(0, "right", false);
            
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            shadowControlObj.StandStill();
            isPressed = false;
            img.sprite = normalUi;

        }

        private void Update()
        {
            if (isPressed)
            {
                img.sprite = pressedUi;
                shadowControlObj.MoveRight();
            }
        }

    }
}
