using System.Collections.Generic;
using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    public class ControlClimb : ControlBase
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
            vMove = vMove.normalized;
        }

        [SerializeField] LayerMask layerClimb;
        [SerializeField] Transform climbTarget;
        [SerializeField] float climbSpeed = 10f;
        Vector3 o, d, n;
        RaycastHit raycastHit;
        public bool isClimbing;
        public float timer;
        public bool CheckClimb()
        {
            if (!isClimbing)
            {
                if (vMove != Vector3.zero)
                {
                    o = transform.position + new Vector3(0, climbTarget.localPosition.y, 0);
                    d = climbTarget.position - o;
                    if (Physics.Raycast(o, d, out raycastHit, d.z, layerClimb))
                    {
                        timer += Ez.FixedTimeMod;
                        if (timer > 0.5f)
                        {
                            isClimbing = true;
                            n = raycastHit.normal;
                            transform.RotateToY(transform.position, transform.position + raycastHit.normal);
                        }
                    }
                    else timer = 0;
                }
                else timer = 0;
            }
            else
            {
                o = transform.position + new Vector3(0, climbTarget.localPosition.y, 0);
                d = climbTarget.position - o;
                if (!Physics.Raycast(transform.position, d, out raycastHit, d.z, layerClimb) && !Physics.Raycast(o, d, out raycastHit, d.z, layerClimb))
                {
                    Rigidbody.MovePosition(transform.position - n * 0.1f);
                    isClimbing = false;
                }
            }
            if (isClimbing) Rigidbody.AddForce(vMove * climbSpeed * Ez.FixedTimeMod, ForceMode.VelocityChange);
            return isClimbing;
        }
    }
}