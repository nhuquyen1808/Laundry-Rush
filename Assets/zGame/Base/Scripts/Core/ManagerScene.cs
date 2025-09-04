using UnityEngine.SceneManagement;

// by nt.Dev93
namespace ntDev
{
    public static class ManagerScene
    {
        public const string SceneLoadScene = "SceneLoadScene";
        public const string SceneMenu = "SceneMenu";
        public const string SceneFashion = "SceneFashion";
        public const string SceneStory = "SceneStory";

        public static string nextScene = SceneMenu;
        public static void LoadScene(string name)
        {
            nextScene = name;
            ManagerGame.Clear();
            ManagerLoading.ShowLoadScene(() => SceneManager.LoadSceneAsync(SceneLoadScene),0.4f);
        }
    }
}