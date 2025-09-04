using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// by nt.Dev93
namespace ntDev
{
    public class SceneLoadScene : MonoBehaviour
    {
        void Start()
        {
            SceneManager.LoadSceneAsync(ManagerScene.nextScene);
        }
    }
}
