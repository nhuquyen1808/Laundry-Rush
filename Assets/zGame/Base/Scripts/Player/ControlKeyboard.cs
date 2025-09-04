using System.Collections.Generic;
using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    public class ControlKeyboard : ControlBase
    {
        PlayerControl playerControl;
        PlayerControl PlayerControl
        {
            get
            {
                if (playerControl == null) playerControl = GetComponent<PlayerControl>();
                return playerControl;
            }
        }

        ControlJump controlJump;
        ControlJump ControlJump
        {
            get
            {
                if (controlJump == null) controlJump = GetComponent<ControlJump>();
                return controlJump;
            }
        }

        Vector3 vMove;
        bool bW, bS, bA, bD, bMove;
        void Update()
        {
            vMove = Vector3.zero;

            if (Input.GetKeyDown(KeyCode.W)) bW = true;
            else if (Input.GetKeyUp(KeyCode.W)) bW = false;
            if (Input.GetKeyDown(KeyCode.A)) bA = true;
            else if (Input.GetKeyUp(KeyCode.A)) bA = false;
            if (Input.GetKeyDown(KeyCode.S)) bS = true;
            else if (Input.GetKeyUp(KeyCode.S)) bS = false;
            if (Input.GetKeyDown(KeyCode.D)) bD = true;
            else if (Input.GetKeyUp(KeyCode.D)) bD = false;

            if (Input.GetKeyUp(KeyCode.Space)) ControlJump.OnJump();

            if (bW) vMove.y = 1;
            if (bS) vMove.y = -1;
            if (bA) vMove.x = -1;
            if (bD) vMove.x = 1;

            if (vMove != Vector3.zero)
            {
                bMove = true;
                PlayerControl.OnMove(vMove);
            }
            else
            {
                if (bMove) PlayerControl.OnMove(vMove);
                bMove = false;
            }
        }
    }
}
