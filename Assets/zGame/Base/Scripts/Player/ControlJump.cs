using System.Collections.Generic;
using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    public class ControlJump : ControlBase
    {
        [SerializeField] Vector3 vGravity = new Vector3(0, -50f, 0);
        [SerializeField] LayerMask layerGround;
        bool isGrounded;
        public void CheckGround()
        {
            isGrounded = Physics.CheckSphere(transform.position, 0.05f, layerGround, QueryTriggerInteraction.Ignore);
            if (isGrounded) jumpTimer = 0;
            else Rigidbody.AddForce(vGravity, ForceMode.Acceleration);
            Animator.SetBool(animIDGrounded, isGrounded);
        }

        public void OnJump()
        {
            goJumping = true;
        }

        bool goJumping;
        [SerializeField] float jumpPower;
        [SerializeField] float jumpTime;
        float jumpTimer;
        public void Jump()
        {
            if (isGrounded && goJumping)
            {
                jumpTimer = jumpTime;
                Animator.Play("JumpStart");
            }
            if (jumpTimer > 0)
            {
                if (Physics.CheckSphere(transform.position + new Vector3(0, 2, 0), 0.02f, layerGround, QueryTriggerInteraction.Ignore))
                    jumpTimer = 0;
                Rigidbody.AddForce(new Vector3(0, jumpPower * jumpTimer / jumpTime, 0), ForceMode.VelocityChange);
                jumpTimer -= Ez.FixedTimeMod;
            }
            goJumping = false;
        }
    }
}
