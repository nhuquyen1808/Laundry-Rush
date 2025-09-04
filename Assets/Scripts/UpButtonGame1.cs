using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DevDuck
{
    public class UpButtonGame1 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
            LogicGame1.instance.clawMachineHandle.AnimationState.SetAnimation(0, "top", false);
            ShadowControl.ins.ShakeGrabObject();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            img.sprite = normalUi;
            shadowControlObj.StandStill();
            isPressed = false;
        }

        private void Update()
        {
            if (isPressed)
            {
                img.sprite = pressedUi;
                shadowControlObj.MoveUp();
            }
        }

    }
}
