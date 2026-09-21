using DevDuck;
using ntDev;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button playButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
    }

    // Update is called once per frame
    void OnPlayButtonClicked()
    {
        ManagerSceneDuck.ins.LoadScene("ClothesSort");
    }
}
