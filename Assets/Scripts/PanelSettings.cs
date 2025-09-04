using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DevDuck
{
    public class PanelSettings : MonoBehaviour
    {
        public GameObject settingsPanel, npopup, nShadow;
        public Button settingsButton, closeButton, homeButton, soundButton, musicButton,replayButton;
        public CanvasGroup soundGreenImage, soundBlueImage;
        public CanvasGroup musicGreenImage, musicBlueImage;
        private void Awake()
        {
            if (PlayerPrefs.GetInt(PlayerPrefsManager.FIRST_TIME_DOWNLOAD) == 0)
            {
                PlayerPrefs.SetInt("MUSIC", 1);
                PlayerPrefs.SetInt("SOUND", 1);
                PlayerPrefs.SetInt(PlayerPrefsManager.FIRST_TIME_DOWNLOAD, 1);
            }

            settingsButton.onClick.AddListener(OnClickSettingsButton);
            closeButton.onClick.AddListener(OnClickCloseButton);
            homeButton.onClick.AddListener(OnClickHomeButton);
            soundButton.onClick.AddListener(OnClickSoundButton);
            musicButton.onClick.AddListener(OnClickMusicButton);
            replayButton.onClick.AddListener(OnClickReplayButton);
        }

        private void OnClickReplayButton()
        {
            GlobalData.isReplay = true;
            Scene currentScene = SceneManager.GetActiveScene();
            DOTween.KillAll();
          //  SceneManager.LoadScene(currentScene.name);
            ManagerSceneDuck.ins.LoadScene(currentScene.name);
        }

        private void Start()
        {
            SetUpButtonOnstart();
           // GlobalData.isReplay = false;
        }

        public void SetUpButtonOnstart()
        {
            int isMusic = PlayerPrefs.GetInt("MUSIC");
            int isSound = PlayerPrefs.GetInt("SOUND");
            if (isMusic == 1)
            {
                musicBlueImage.alpha = 0;
                musicGreenImage.alpha = 1;
            }
            else
            {
                musicGreenImage.alpha = 0;
                musicBlueImage.alpha = 1;
            }

            if (isSound == 1)
            {
                soundBlueImage.alpha = 0;
                soundGreenImage.alpha = 1;
            }
            else
            {
                soundGreenImage.alpha = 0;
                soundBlueImage.alpha = 1;
            }
        }

        private void OnClickMusicButton()
        {
            int isMusic = PlayerPrefs.GetInt("MUSIC");
            if (isMusic == 1)
            {
                AudioManager.instance.StopPlayMusic();
                PlayerPrefs.SetInt("MUSIC", 0);
                musicBlueImage.DOFade(1, 0.3f).From(0);
                musicGreenImage.DOFade(0, 0.3f).From(1);
            }
            else
            {
                AudioManager.instance.ContinuePlayMusic();
                PlayerPrefs.SetInt("MUSIC", 1);
                musicGreenImage.DOFade(1, 0.3f).From(0);
                musicBlueImage.DOFade(0, 0.3f).From(1);
            }
        }

        private void OnClickSoundButton()
        {
            int isMusic = PlayerPrefs.GetInt("SOUND");
            if (isMusic == 1)
            {
                AudioManager.instance.StopPlaySound();
                PlayerPrefs.SetInt("SOUND", 0);
                soundBlueImage.DOFade(1, 0.5f).From(0);
                soundGreenImage.DOFade(0, 0.5f).From(1);
            }
            else
            {
                AudioManager.instance.ContinuePlaySound();

                PlayerPrefs.SetInt("SOUND", 1);
                soundGreenImage.DOFade(1, 0.5f).From(0);
                soundBlueImage.DOFade(0, 0.5f).From(1);
            }
        }

        private void OnClickHomeButton()
        {
            homeButton.transform.DOScale(0.95f, 0.2f).OnComplete(() =>
            {
                homeButton.transform.DOScale(1, 0.2f).OnComplete(() =>
                {
                    GlobalData.isInGame = true;
                    ManagerGame.TIME_SCALE = 1;
                    npopup.gameObject.SetActive(false);
                    nShadow.gameObject.SetActive(false);
                    DOTween.KillAll();
                    ManagerSceneDuck.ins.LoadScene("SceneMenu");
                });
            });
        }

        private void OnClickCloseButton()
        {
            npopup.transform.localScale = Vector3.one;
            npopup.transform.DOScale(0, 0.3f).SetEase(Ease.InBack);
            npopup.GetComponent<CanvasGroup>().DOFade(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                nShadow.SetActive(false);
                ManagerGame.TIME_SCALE = 1;
                GlobalData.isInGame = true;
                DOTween.PlayAll();
            });
        }

        private void OnClickSettingsButton()
        {
            DOTween.PauseAll();
            GlobalData.isInGame = false;
            ManagerGame.TIME_SCALE = 0;
            settingsButton.transform.DOScale(0.95f, 0.1f).OnComplete((() =>
            {
                settingsButton.transform.DOScale(1, 0.1f).OnComplete(() =>
                {
                    nShadow.SetActive(true);
                    npopup.SetActive(true);
                    npopup.transform.localScale = Vector3.zero;
                    npopup.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
                    npopup.GetComponent<CanvasGroup>().DOFade(1, 0.3f).SetEase(Ease.OutBack);
                });
            }));
        }
    }
}