using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DevDuck
{
    public class DownButtonGame1 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        bool isPressed;
        [FormerlySerializedAs("shadowObj")] [FormerlySerializedAs("grabObj")] public ShadowControl shadowControlObj;
        [SerializeField] Sprite pressedUi, normalUi;
        Image img;

        private void Start()
        {
            img = GetComponent<Image>();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            isPressed = true;
            LogicGame1.instance.clawMachineHandle.AnimationState.SetAnimation(0, "bot", false);
            ShadowControl.ins.ShakeGrabObject();
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
                shadowControlObj.MoveDown();
            }
        }
    }
}
