using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using ntDev;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DevDuck
{
    public enum AREA
    {
        NONE,
        STORE,
        BAR,
        SCHOOL,
        PARK,
        RESTAURANT,
        ARCADE
    }

    public class LogicMenu : MonoBehaviour
    {
        public List<AreaGame> areas = new List<AreaGame>();
        List<int> themeArcadeItems = new List<int>();
        List<int> themeShopItems = new List<int>();
        List<int> themeSchoolItems = new List<int>();
        List<int> themeRestaurantItems = new List<int>();
        List<int> themeBarItems = new List<int>();
        List<int> themeParkItems = new List<int>();
        List<DataFashion> dataFashion = new List<DataFashion>();
        List<int> idUserGetted = new List<int>();
        [SerializeField] private Button missionButton;
        public int amountItemArcade,
            amountItemShop,
            amountItemRestaurant,
            amountItemBar,
            amountItemPark,
            amountItemScool;



        public RectTransform nContent;
        
        public List<int> positionsLocation = new List<int>();
        void Start()
        {
            Application.targetFrameRate = 90;
            Addressables.InitializeAsync();
            ShowListArea();
            Observer.AddObserver(EventAction.EVENT_AREA_SELECTED, HandleAreaSelected);
            GlobalData.isFashion = true;
            GlobalData.isInGame = true;
            if (PlayerPrefs.GetInt("ISNEWUSER") == 0)
            {
                PlayerPrefs.SetFloat(PlayerPrefsManager.Coin, 10000);
                PlayerPrefs.SetInt("ISNEWUSER", 1);
            }
            GetDataArea();
            SaveGame.LoadArrayFromPlayerPrefs();
            idUserGetted = SaveGame.ArraySaved.ToList();

            int yAxis = positionsLocation[PlayerPrefs.GetInt(PlayerPrefsManager.AREA_UNLOCK)];
            nContent.anchoredPosition = new Vector2(0, yAxis);
        }

        private async void GetDataArea()
        {
            dataFashion = await DataFashion.GetListData();
            for (int i = 0; i < dataFashion.Count; i++)
            {
                if (dataFashion[i].Theme == THEME.ARCADE)
                {
                    themeArcadeItems.Add(dataFashion[i].ID);
                }

                if (dataFashion[i].Theme == THEME.FASHION)
                {
                    themeShopItems.Add(dataFashion[i].ID);
                }

                if (dataFashion[i].Theme == THEME.RESTAURANT)
                {
                    themeRestaurantItems.Add(dataFashion[i].ID);
                }

                if (dataFashion[i].Theme == THEME.SCHOOL)
                {
                    themeSchoolItems.Add(dataFashion[i].ID);
                }

                if (dataFashion[i].Theme == THEME.PARK)
                {
                    themeParkItems.Add(dataFashion[i].ID);
                }

                if (dataFashion[i].Theme == THEME.PARK)
                {
                    themeBarItems.Add(dataFashion[i].ID);
                }
            }
            GetAmountItemsGettedPerTheme();
        }
        private void OnDestroy()
        {
            Observer.RemoveObserver(EventAction.EVENT_AREA_SELECTED, HandleAreaSelected);
        }
        private void HandleAreaSelected(object obj)
        {
            int  area = (int)obj;
            string sceneName = "";
            switch (area)
            {
                case 0:
                case 6:
                    GlobalData.currentTheme = THEME.ARCADE;
                    GlobalData.currentSceneMiniGame = SceneGame.Game1;
                    GlobalData.currentStory = 1;
                    break;
                case 1:
                    GlobalData.currentTheme = THEME.FASHION;
                    GlobalData.currentSceneMiniGame = SceneGame.Game8;
                    GlobalData.currentStory = 2;
                    break;
                case 2:
               // case 8:
                    GlobalData.currentTheme = THEME.SCHOOL;
                    PlayerPrefs.SetInt("SCHOOLTheme", 0);
                    GlobalData.currentSceneMiniGame = SceneGame.Game10;
                    GlobalData.currentStory = 3;
                    break;
                case 3:
                case 9:
                    GlobalData.currentTheme = THEME.RESTAURANT;
                    PlayerPrefs.SetInt("RESTAURANTTheme", 0);
                    GlobalData.currentSceneMiniGame = SceneGame.Game6;
                    GlobalData.currentStory = 2;
                    break;
                case 4:
                case 10:
                    GlobalData.currentTheme = THEME.BAR;
                    GlobalData.currentSceneMiniGame = SceneGame.Game20;
                    GlobalData.currentStory = 4;
                    break;
                case 5:
                    GlobalData.currentTheme = THEME.PARK;
                    GlobalData.currentSceneMiniGame = SceneGame.Game2;
                    GlobalData.currentStory = 5;
                    break;
                case 7:
                    
                    GlobalData.currentTheme = THEME.FASHION;
                    GlobalData.currentSceneMiniGame = SceneGame.Game15;
                    GlobalData.currentStory = 2;
                    break;
                case 8:
                    GlobalData.currentTheme = THEME.SCHOOL;
                    PlayerPrefs.SetInt("SCHOOLTheme", 0);
                    GlobalData.currentSceneMiniGame = SceneGame.Game18;
                    GlobalData.currentStory = 3;
                    break;
                case 11:
                    GlobalData.currentTheme = THEME.PARK;
                    GlobalData.currentSceneMiniGame = SceneGame.Game16;
                    GlobalData.currentStory = 5;
                    break;
                
            }

            DOTween.KillAll();
            ManagerSceneDuck.ins.LoadScene(SceneGame.sceneFashion);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }
        public void ShowListArea()
        {
            for (int i = 0; i < areas.Count; i++)
            {
                var a = i;
                /*areas[i].SetUnlock();
                areas[i].notationIcon.gameObject.SetActive(false);
                areas[a].transform.DOScale(1, 0.4f).SetEase(Ease.OutBack).SetDelay(a * 0.3f);*/
                SetUpAreas(areas[a]);
            }
        }
        public void LoadSceneFashion(string sceneName)
        {
            ManagerSceneDuck.ins.LoadScene(sceneName);
        }

        public void SetUpAreas(AreaGame area)
        {
            int levelUnlock = PlayerPrefs.GetInt(PlayerPrefsManager.AREA_UNLOCK);
            /*if (levelUnlock >= 5)
            {
                area.SetUnlock();
                area.iconArea.GetComponent<Animator>().enabled = true;
                area.notificationIcon.gameObject.SetActive(false);
            }
            else*/
            {
                if (area.id < levelUnlock)
                {
                    area.SetUnlock();
                    area.iconArea.GetComponent<Animator>().enabled = false;
                    area.notificationIcon.gameObject.SetActive(false);
                    area.cloudsAnimator.gameObject.SetActive(false);
                }
                else if (area.id == levelUnlock)
                {
                    area.SetUnlock();
                    area.notificationIcon.gameObject.SetActive(true);
                }
                else
                {
                    area.SetLock();
                    area.iconArea.GetComponent<Animator>().enabled = false;
                    area.notificationIcon.gameObject.SetActive(false);
                    area.cloudsAnimator.gameObject.SetActive(true);
                }
            }
        }

        private void GetAmountItemsGettedPerTheme()
        {
            for (int i = 0; i < themeArcadeItems.Count; i++)
            {
                if (idUserGetted.Contains(themeArcadeItems[i]))
                {
                    amountItemArcade++;
                }
            }

            for (int i = 0; i < themeShopItems.Count; i++)
            {
                if (idUserGetted.Contains(themeShopItems[i]))
                {
                    amountItemShop++;
                }
            }

            for (int i = 0; i < themeSchoolItems.Count; i++)
            {
                if (idUserGetted.Contains(themeSchoolItems[i]))
                {
                    amountItemScool++;
                }
            }

            for (int i = 0; i < themeBarItems.Count; i++)
            {
                if (idUserGetted.Contains(themeBarItems[i]))
                {
                    amountItemBar++;
                }
            }

            for (int i = 0; i < themeRestaurantItems.Count; i++)
            {
                if (idUserGetted.Contains(themeRestaurantItems[i]))
                {
                    amountItemRestaurant++;
                }
            }

            for (int i = 0; i < themeParkItems.Count; i++)
            {
                if (idUserGetted.Contains(themeParkItems[i]))
                {
                    amountItemPark++;
                }
            }
        }
    }
}