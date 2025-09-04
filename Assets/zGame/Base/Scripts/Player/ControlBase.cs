using UnityEngine;
using ntDev;

// by nt.Dev93
namespace ntDev
{
    public enum MoveType
    {
        GROUND,
        AIR,
    }

    public class ControlBase : MonoBehaviour
    {
        protected Rigidbody Rigidbody;

        protected Animator Animator;
        protected int animIDSpeed;
        protected int animIDGrounded;
        protected int animIDJump;

        public virtual void Init(Rigidbody rigidbody, Animator animator)
        {
            Rigidbody = rigidbody;
            Animator = animator;

            animIDSpeed = Animator.StringToHash("Speed");
            animIDGrounded = Animator.StringToHash("Grounded");
            animIDJump = Animator.StringToHash("Jump");
        }

        public virtual void OnMove(Vector3 v)
        {

        }
    }
}