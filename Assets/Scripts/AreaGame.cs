
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DevDuck
{
    public enum LevelArea
    {
        LEVEL_1,LEVEL_2,LEVEL_3,LEVEL_4
    }
    public class AreaGame : MonoBehaviour
    {
        public int id;
        public LevelArea levelArea;
        public AREA area;
        public Image lockIcon;
        public Image iconArea; 
        public GameObject notificationIcon;
        public Button areaButton;
        public int anmountToUnlock;
        public Animator cloudsAnimator;
        public Animator roadAnimator;
        private void Awake()
        {
            areaButton.onClick.AddListener(OnClickAreaSelected);
        }

        private void OnClickAreaSelected()
        {
            Observer.Notify(EventAction.EVENT_AREA_SELECTED,id);
        }

        public void SetUnlock()
        {
            areaButton.enabled = true;
            lockIcon.enabled = false;
            iconArea.color = new Color32(255,255,255,255);
            cloudsAnimator.gameObject.SetActive(true);
            cloudsAnimator.Play("CloudsAnim",0,0);
            roadAnimator.gameObject.SetActive(true);
            roadAnimator.Play("road",0,0);
        }

        public void SetLock()
        {
            areaButton.enabled = false;
            lockIcon.enabled = true;
            iconArea.color = new Color32(100,100,100,255);
            
        }

        private void SetUnLockLevelArea(LevelArea levelArea)
        {
            switch (levelArea)
            {
                case LevelArea.LEVEL_1:
                    Debug.Log("LEVEL 1");
                    break;
                case LevelArea.LEVEL_2:
                    Debug.Log("LEVEL 2"); 
                    break;
                case LevelArea.LEVEL_3:
                    Debug.Log("LEVEL 3");
                    break;
            }
        }
    }
}
