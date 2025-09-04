using System.Collections.Generic;
using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    public class PlayerControl : MonoBehaviour
    {
        LogicControl logicControl;
        LogicControl LogicControl
        {
            get
            {
                if (logicControl == null) logicControl = FindObjectOfType<LogicControl>();
                return logicControl;
            }
        }

        void Awake()
        {
            InitControl();
        }

        ControlClimb ControlClimb;
        ControlJump ControlJump;
        ControlAim ControlAim;
        ControlMove ControlMove;
        ControlMoveCam ControlMoveCam;
        void InitControl()
        {
            ControlMove = GetComponent<ControlMove>();
            ControlBase[] arrControl = GetComponents<ControlBase>();
            foreach (ControlBase control in arrControl)
            {
                if (control is ControlClimb) ControlClimb = control as ControlClimb;
                else if (control is ControlJump) ControlJump = control as ControlJump;
                else if (control is ControlAim) ControlAim = control as ControlAim;
                else if (control is ControlMove) ControlMove = control as ControlMove;
                else if (control is ControlMoveCam) ControlMoveCam = control as ControlMoveCam;
                control.Init(GetComponent<Rigidbody>(), GetComponentInChildren<Animator>());
            }

            if (LogicControl == null) return;
            if (LogicControl.ControllerMove != null)
            {
                LogicControl.ControllerMove.OnEventDragV = OnMove;
                LogicControl.ControllerMove.OnEventRelease = (f) => OnMove(Vector3.zero);
            }
            if (LogicControl.ControllerAim != null)
            {
                LogicControl.ControllerAim.OnEventTouch = (v) => ControlAim?.OnAim();
                LogicControl.ControllerAim.OnEventDragD = (v) => ControlAim?.OnMoveCam(v);
                LogicControl.ControllerAim.OnEventRelease = (f) => ControlAim?.OffAim();
            }
            if (LogicControl.ControllerCam != null)
            {
                // LogicControl.ControllerCam.OnEventDragD = (v) => ControlMoveCam?.OnMoveCam(LogicControl.CinemachineFreeLook, v);
                // LogicControl.ControllerCam.OnEventRelease = (f) => ControlMoveCam?.OnMoveCam(LogicControl.CinemachineFreeLook, Vector3.zero);
            }
            if (LogicControl.Btn != null) LogicControl.Btn.OnEventTouch = (v) => ControlJump.OnJump();
        }

        public void OnMove(Vector3 v)
        {
            ControlClimb?.OnMove(v);
            ControlMove?.OnMove(v);
        }

        bool isClimbing;
        void FixedUpdate()
        {
            isClimbing = ControlClimb ? ControlClimb.CheckClimb() : false;
            if (!isClimbing)
            {
                ControlMove?.Move();
                ControlJump?.CheckGround();
                ControlJump?.Jump();
            }
        }
    }
}
