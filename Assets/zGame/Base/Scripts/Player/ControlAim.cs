using System.Collections.Generic;
using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    public class ControlAim : ControlBase
    {
        [SerializeField] Transform nCam;
        [SerializeField] GameObject imgAim;

        public bool IsAiming;

        public void OnAim()
        {
            // nAim.localPosition = new Vector3(0, 2, 1000);
            imgAim.gameObject.SetActive(true);
            Camera.main.transform.localPosition = new Vector3(0.4f, 2, -2);
            IsAiming = true;
        }

        public float SpeedCamX = 50f;
        public float SpeedCamY = 50f;
        public void OnMoveCam(Vector3 d)
        {
            nCam.eulerAngles += new Vector3(-d.y * SpeedCamY * Ez.TimeMod % 360, +d.x * SpeedCamX * Ez.TimeMod % 360, 0);
            // cinemachineFreeLook.m_XAxis.Value += d.x * SpeedCamX * Ez.TimeMod;
            // cinemachineFreeLook.m_YAxis.Value -= d.y * SpeedCamY * Ez.TimeMod;
        }

        public void OffAim()
        {
            imgAim.gameObject.SetActive(false);
            Camera.main.transform.localPosition = new Vector3(0, 2, -7);
            IsAiming = false;
        }
    }
}
