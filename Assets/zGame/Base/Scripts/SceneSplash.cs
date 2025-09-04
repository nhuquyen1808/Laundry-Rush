using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// by nt.Dev93
namespace ntDev
{
    public class SceneSplash : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI txtLoading;

        void Start()
        {
            txtLoading.text = "Loading.";
            timer = 0.5f;
            StartCoroutine(Load());
        }

        IEnumerator Load()
        {
            ManagerLoading.HideLoadScene();
            yield return new WaitForSeconds(3f);
            ManagerScene.LoadScene(ManagerScene.SceneMenu);
        }

        float timer = 0;
        int t = 1;
        void Update()
        {
            if (timer > 0)
            {
                timer -= Ez.TimeMod;
                if (timer <= 0)
                {
                    timer = 0.5f;
                    ++t;
                    if (t > 3) t = 1;

                    string str = "Loading";
                    if (t == 1) str = "Loading.";
                    else if (t == 2) str = "Loading..";
                    else if (t == 3) str = "Loading...";
                    txtLoading.text = str;
                }
            }
        }
    }
}