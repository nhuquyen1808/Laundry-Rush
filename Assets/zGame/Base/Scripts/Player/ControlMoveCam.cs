using System.Collections.Generic;
using UnityEngine;
// using Cinemachine;

// by nt.Dev93
namespace ntDev
{
    public class ControlMoveCam : ControlBase
    {
        protected float SpeedCamX = 15f;
        protected float SpeedCamY = 0.2f;
        // public void OnMoveCam(CinemachineFreeLook cinemachineFreeLook, Vector3 d)
        // {
        //     cinemachineFreeLook.m_XAxis.Value += d.x * SpeedCamX * Ez.TimeMod;
        //     cinemachineFreeLook.m_YAxis.Value -= d.y * SpeedCamY * Ez.TimeMod;
        // }
    }
}
