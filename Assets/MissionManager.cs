using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class MissionManager : MonoBehaviour
    {
        [SerializeField] List<Mission> missions = new List<Mission>();
        [SerializeField] private Button closeMission, missionButton;
        [SerializeField] private GameObject backgroundImageMission;
        [SerializeField] private CanvasGroup nPopup;
        [SerializeField] List<GameObject> elements = new List<GameObject>();
        public GameObject noticeImage;
        
        void Start()
        {
            missionButton.onClick.AddListener(OnClickMissionButton);
            closeMission.onClick.AddListener(OnClickCloseMission);
            GetDay();

            foreach (Mission mission in missions)
            {
                if (mission.claimButton.interactable)
                {
                    noticeImage.SetActive(true);
                    break;
                }
                else
                {
                    noticeImage.SetActive(false);
                }
            }
        }
        private void OnClickCloseMission()
        {
            DOTween.KillAll();
            UiDuck.HidePopup(backgroundImageMission, nPopup);
        }
        private void OnClickMissionButton()
        {
            foreach (GameObject mission in elements)
            {
                mission.transform.localScale = Vector3.zero;
            }

            UiDuck.ShowPopup(backgroundImageMission, nPopup);
            UiDuck.ShowElementsPopup(elements, 0.1f);
        }

        public void GetDay()
        {
            int currentDay = DateTime.Now.Day;

            int daySaved = PlayerPrefs.GetInt(PlayerPrefsManager.THE_LAST_DAY_SAVED);
            if (currentDay != daySaved)
            {
                PlayerPrefs.SetInt(PlayerPrefsManager.THE_LAST_DAY_SAVED, currentDay);
                PlayerPrefs.SetInt(PlayerPrefsManager.A_GAME_COMEPLETED, 0);
                PlayerPrefs.SetInt(PlayerPrefsManager.PREVIEW_A_DRESS, 0);
                PlayerPrefs.SetInt(PlayerPrefsManager.BUY_A_DRESS, 0);
                PlayerPrefs.SetInt(PlayerPrefsManager.GET_3_STARS_A_GAME, 0);
                PlayerPrefs.SetInt(PlayerPrefsManager.UNLOCK_NEW_THEME, 0);
                PlayerPrefs.SetInt(PlayerPrefsManager.COMPLETE_3_TASK, 0);
                PlayerPrefs.SetInt(PlayerPrefsManager.COMPLETE_ALL_THEME, 0);
                for (int i = 0; i < missions.Count; i++)
                {
                    PlayerPrefs.SetInt($"MISSION_{missions[i].id}_CLAIMED", 0);
                    SetDataMission(missions[i].id);
                }
            }
            else
            {
                for (int i = 0; i < missions.Count; i++)
                {
                    SetDataMission(missions[i].id);
                }
            }
        }

        private void SetStateMission()
        {
            for (int i = 0; i < missions.Count; i++)
            {
                SetDataMission(missions[i].id);
            }
        }

        public void SetDataMission(int idMission)
        {
            switch (idMission)
            {
                case 0:
                    missions[0].currentState = PlayerPrefs.GetInt(PlayerPrefsManager.TUTORIAL_COMPLETED);
                    missions[0].UpdateMission();
                    break;
                case 1:
                    missions[1].currentState = PlayerPrefs.GetInt(PlayerPrefsManager.A_GAME_COMEPLETED);
                    missions[1].UpdateMission();
                    break;
                case 2:
                    missions[2].currentState = PlayerPrefs.GetInt(PlayerPrefsManager.PREVIEW_A_DRESS);
                    missions[2].UpdateMission();
                    break;
                case 3:
                    missions[3].currentState = PlayerPrefs.GetInt(PlayerPrefsManager.BUY_A_DRESS);
                    missions[3].UpdateMission();
                    break;
                case 4:
                    missions[4].currentState = PlayerPrefs.GetInt(PlayerPrefsManager.GET_3_STARS_A_GAME);
                    missions[4].UpdateMission();
                    break;
                case 5:
                    missions[5].currentState = PlayerPrefs.GetInt(PlayerPrefsManager.UNLOCK_NEW_THEME);
                    missions[5].UpdateMission();
                    break;
                case 6:
                    missions[6].currentState = PlayerPrefs.GetInt(PlayerPrefsManager.COMPLETE_3_TASK);
                    missions[6].UpdateMission();
                    break;
                case 7:
                    missions[7].currentState = PlayerPrefs.GetInt(PlayerPrefsManager.COMPLETE_ALL_THEME);
                    missions[7].UpdateMission();
                    break;
            }
        }
    }
}