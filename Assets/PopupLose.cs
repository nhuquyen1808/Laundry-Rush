using DevDuck;
using DG.Tweening;
using ntDev;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupLose : MonoBehaviour
{

    public Image shadowImg;
    public static PopupLose instance;
    public GameObject nPopup;
    public Button rePlayButton;


    private void Awake()
    {
        instance = this;
        rePlayButton.onClick.AddListener(OnClickRePlayBtn);
    }

    private void OnClickRePlayBtn()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        ManagerSceneDuck.ins.LoadScene(currentScene.name);
    }

    [SerializeField] List<GameObject> list = new List<GameObject>();

    public void Show()
    {
        shadowImg.enabled = true;
        nPopup.gameObject.SetActive(true);

        for (int i = 0; i < list.Count; i++)
        {
            list[i].gameObject.transform.localScale = Vector3.zero;
            list[i].SetActive(false);
        }
        for (int i = 0; i < list.Count; i++)
        {
            var a = i;
            list[a].SetActive(true);
            list[a].transform.DOScale(1, 0.3f).SetDelay(a * 0.1f).SetEase(Ease.OutBack);
        }
    }
}
