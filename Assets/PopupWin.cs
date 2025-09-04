using ntDev;
using System;
using System.Collections;
using System.Collections.Generic;
using DevDuck;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public  class PopupWin : MonoBehaviour
{
    public static PopupWin ins;
    public Image shadow;
    public GameObject popup;
    public Button nextButton;

    private void Awake()
    {
        ins = this;
        nextButton.onClick.AddListener(OnClickNextButton);
    }

    private void OnClickNextButton()
    {
        int currentlevel = PlayerPrefs.GetInt("CurrentLevel");
        currentlevel++;
        PlayerPrefs.SetInt("CurrentLevel",currentlevel);
        Scene scene = SceneManager.GetActiveScene();
        ManagerSceneDuck.ins.LoadScene(scene.name);
    }

    [SerializeField] List<GameObject> list = new List<GameObject>();

    public void Show()
    {
        shadow.enabled = true;
        popup.gameObject.SetActive(true);

        for (int i = 0; i < list.Count; i++)
        {
            list[i].gameObject.transform.localScale =  Vector3.zero;
            list[i].SetActive(false);
        }
        for (int i = 0; i < list.Count; i++)
        {
            var a = i;
            list[a].SetActive(true);
            list[a].transform.DOScale(1, 0.3f).SetDelay(a * 0.1f ).SetEase(Ease.OutBack);
        }
    }
}
