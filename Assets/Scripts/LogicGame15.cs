using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DevDuck
{
    public class LogicGame15 : MonoBehaviour
    {
        public Button leftButton, rightButton, upButton, downButton;
        public PlayerGame15 player;
        public bool isCanPlay, isInsDone;
        public UiWinLose uiWinLose;
        public GameObject fx;

        public ParticleSystem leftFx, rightFx;

        //  public List<LevelGame15> levels = new List<LevelGame15>();
        public static LogicGame15 instance;
        public GameObject settingButton, buttonBelow;

        private void Awake()
        {
            instance = this;
            leftButton.onClick.AddListener(OnClickLeftButton);
            rightButton.onClick.AddListener(OnClickRightButton);
            upButton.onClick.AddListener(OnClickUpButton);
            downButton.onClick.AddListener(OnClickDownButton);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Scene currentScene = SceneManager.GetActiveScene();
                DOTween.KillAll();
                SceneManager.LoadScene(currentScene.name);
            }
        }

        private void OnClickDownButton()
        {
            if (isCanPlay && isInsDone)
            {
                isCanPlay = false;
                player.MoveDown();
            }
        }

        private void OnClickUpButton()
        {
            if (isCanPlay && isInsDone)
            {
                isCanPlay = false;
                player.MoveUp();
            }
        }

        private void OnClickRightButton()
        {
            if (isCanPlay && isInsDone)
            {
                isCanPlay = false;
                player.MoveRight();
            }
        }

        private void OnClickLeftButton()
        {
            if (isCanPlay && isInsDone)
            {
                isCanPlay = false;
                player.MoveLeft();
            }
        }

        void Start()
        {
            isCanPlay = true;
            Observer.AddObserver(EventAction.EVENT_PLAYER_MOVE_DONE, CheckPlayerMoveDone);
            Observer.AddObserver(EventAction.EVENT_POPUP_SHOW_WIN_DONE, ShowWinPanel);
            SetData();
            /*levels[0].ShowObjectScene();
            player = levels[0].playerGame15;*/
        }

        private void OnDestroy()
        {
            Observer.RemoveObserver(EventAction.EVENT_PLAYER_MOVE_DONE, CheckPlayerMoveDone);
            Observer.RemoveObserver(EventAction.EVENT_POPUP_SHOW_WIN_DONE, ShowWinPanel);
        }

        private void ShowWinPanel(object obj)
        {
            fx.transform.position = player.transform.position;
            Duck.PlayParticle(leftFx);
            Duck.PlayParticle(rightFx);
            settingButton.SetActive(false);
            buttonBelow.SetActive(false);
            DOVirtual.DelayedCall(1f, uiWinLose.ShowWin3Star);
        }

        private void CheckPlayerMoveDone(object obj)
        {
            isCanPlay = (bool)obj;
        }

        public void SetData()
        {
            if (GlobalData.isReplay)
            {
                PlayerPrefs.SetInt("CURRENTLEVElGAME15", PlayerPrefs.GetInt("CURRENTLEVElGAME15"));
            }
            else
            {
                PlayerPrefs.SetInt("CURRENTLEVElGAME15", PlayerPrefs.GetInt("CURRENTLEVElGAME15") + 1);
            }

            GlobalData.isReplay = false;
            int index = PlayerPrefs.GetInt("CURRENTLEVElGAME15");
            LevelGame15 level = Resources.Load<LevelGame15>($"Data/Game15/Level{index}");
            LevelGame15 levelGame15 = Instantiate(level, transform.position, Quaternion.identity);
            levelGame15.ShowObjectScene();
            player = levelGame15.playerGame15;
        }
    }
}