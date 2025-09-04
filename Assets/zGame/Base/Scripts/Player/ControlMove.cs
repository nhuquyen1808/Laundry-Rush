using System.Collections.Generic;
using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    public class ControlMove : ControlBase
    {
        public Vector3 vMove;
        public override void OnMove(Vector3 v)
        {
            vMove = v;
            if (vMove == Vector3.zero)
            {
                Rigidbody.AddForce(Vector3.zero, ForceMode.VelocityChange);
                return;
            }
            vMove.z = vMove.y;
            vMove.y = 0;
            vMove = vMove.normalized;
            vMove = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up) * vMove;
        }

        [SerializeField] float walkSpeed = 50f;
        [SerializeField] float runSpeed = 50f;
        bool isRunning;
        float vMoveAnimation;
        public void Move()
        {
            if (vMove != Vector3.zero)
            {
                Rigidbody.AddForce(vMove * (isRunning ? runSpeed : walkSpeed) * Ez.FixedTimeMod, ForceMode.VelocityChange);
                float _targetRotation = Mathf.Atan2(vMove.x, vMove.z) * Mathf.Rad2Deg + Camera.main.transform.localEulerAngles.y;
                transform.rotation = Quaternion.Euler(0.0f, _targetRotation, 0.0f);
                vMoveAnimation = Mathf.Lerp(vMoveAnimation, isRunning ? runSpeed : walkSpeed, 0.5f);
            }
            else vMoveAnimation = Mathf.Lerp(vMoveAnimation, 0, 0.5f);
            if (vMoveAnimation < 0.1f) vMoveAnimation = 0;
            Animator.SetFloat(animIDSpeed, vMoveAnimation);
        }
    }
}
