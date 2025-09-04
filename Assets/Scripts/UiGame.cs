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
    public class UiGame : MonoBehaviour
    {
        public static UiGame Instance;
        public Button homeButton;
        public Button reloadButton;
        public Button ClearDataButton;
        public GameObject levelBoard;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            homeButton.onClick.AddListener(OnClickHomeButton);
            reloadButton.onClick.AddListener(OnClickReloadSceneButton);
            //  ClearDataButton.onClick.AddListener(OnClickClearDataButton);
        }

        private void OnClickClearDataButton()
        {
            PlayerPrefs.SetInt("currentLevel", 0);
        }

        private void OnClickReloadSceneButton()
        {
            Scene scene = SceneManager.GetActiveScene();
            DOTween.KillAll();
            SceneManager.LoadScene(scene.name);
            ManagerSceneDuck.ins.LoadScene(scene.name);
            levelBoard.gameObject.SetActive(false);

        }

        private void OnClickHomeButton()
        {
            Scene scene = SceneManager.GetActiveScene();
            DOTween.KillAll();
            ManagerSceneDuck.ins.LoadScene("Home");
            SceneManager.LoadScene(scene.name);
            levelBoard.gameObject.SetActive(true);

        }
    }
}
