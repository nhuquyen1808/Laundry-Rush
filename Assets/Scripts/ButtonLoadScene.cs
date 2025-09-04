using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DevDuck
{
    public class ButtonLoadScene : MonoBehaviour
    {
        private Button _button;
        public string sceneName;
        public Button Button
        {
            get
            {
                if (_button == null)
                {
                    _button = GetComponent<Button>();
                }
                return _button;
            }
        }
        void Start()
        {
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(OnClickButtonLoadScene);
        }
        private void OnClickButtonLoadScene()
        {
          //  ManagerScene.ins.LoadScene(sceneName);//
            SceneManager.LoadSceneAsync(sceneName);
            UiGame.Instance. levelBoard.gameObject.SetActive(false);

        }
    }
}
