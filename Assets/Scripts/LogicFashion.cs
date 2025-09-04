using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DG.Tweening;
using ntDev;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DevDuck
{
    public class LogicFashion : MonoBehaviour
    {
        [HideInInspector] public List<FashionItem> listFashionItemsGame = new List<FashionItem>();
        [SerializeField] public List<FashionItem> listFashionItemsGot = new List<FashionItem>();
        public Model model;
        private RaycastHit2D hit;
        private Camera _camera;
        private ItemFashionInit cureentItem;
        private bool isDragging;
        public FashionItem fashionItemPrefab;
        public static LogicFashion instance;
        public LogicUIFashion UIFashion;
        [SerializeField] FashionTab currentFashionTab;
        [SerializeField] List<FashionTab> fashionTabs = new List<FashionTab>();
        [SerializeField] List<TabFashionButton> fashionTabFashionButtons = new List<TabFashionButton>();
        public GameObject topDefaultFashion;
        public GameObject botDefaultFashion;
        public GameObject hairDefaultFashion;
        [HideInInspector] public List<int> idUserGetted = new List<int>();
        [HideInInspector] public List<DataFashion> dataFashion = new List<DataFashion>();
        public Sprite arcadeSpr, restaurantSpr, schoolSpr, parkSpr, fashionSpr, barSpr;

        public int totalIndext;
        public LogicUIFashion logicUIFashion;
        public Image iconClothe;

        public ParticleSystem sparkleBody, starEffect;
        public TutorialFashion tutorialFashion;

        public FocusTUTFashion focusTutorial;
        public Sprite defaultBgItem, greenBgItem;
        public int indexFillBar = 0;

        private void Awake()
        {
            instance = this;
            Addressables.InitializeAsync();
        }

        private async void Start()
        {
            _camera = Camera.main;
            Observer.AddObserver(EventAction.EVENT_ITEM_SELECTED, ItemSelected);
            dataFashion = await DataFashion.GetListData();
            SaveGame.LoadArrayFromPlayerPrefs();
            idUserGetted = SaveGame.ArraySaved.ToList();
            DOVirtual.DelayedCall(0.5f, () => InitItemOnBoard());
            Observer.AddObserver(EventAction.EVENT_BUY_ITEM, HandleBuyItem);
            Observer.AddObserver(EventAction.EVENT_TRY_RELEASE_ITEM, HandleReleaseItem);

            model.LoadAndInitFashion();
        }

        private void HandleReleaseItem(object obj)
        {
            int idRelease = (int)obj;
            for (int i = 0; i < model.listModelFashion.Count; i++)
            {
                if (idRelease == model.listModelFashion[i].id)
                {
                    if (model.listModelFashion[i].fashionType == FashionType.FASHION_HAIR)
                    {
                        hairDefaultFashion.SetActive(true);
                        indexFillBar -= 1;
                    }

                    if (model.listModelFashion[i].fashionType == FashionType.FASHION_TOP)
                    {
                        topDefaultFashion.SetActive(true);
                        indexFillBar -= 1;
                    }

                    if (model.listModelFashion[i].fashionType == FashionType.FASHION_BOT)
                    {
                        botDefaultFashion.SetActive(true);
                        indexFillBar -= 1;
                    }

                    if (model.listModelFashion[i].fashionType == FashionType.FASHION_OVERALL)
                    {
                        topDefaultFashion.SetActive(true);
                        botDefaultFashion.SetActive(true);
                        indexFillBar -= 2;
                    }

                    model.listModelFashion[i].id = -1;
                    Destroy(model.listModelFashion[i].itemFashionInit.gameObject);
                }
            }

            for (int i = 0; i < listFashionItemsGame.Count; i++)
            {
                if (listFashionItemsGame[i].id == idRelease)
                {
                    listFashionItemsGame[i].imageSelected.enabled = false;
                    listFashionItemsGame[i].sprBG.enabled = true;
                }
            }

            CheckShowNextButton();
            logicUIFashion.UpdateFillImage(indexFillBar);
            //UpdateIndex();
        }

        private void HandleBuyItem(object obj)
        {
            // AudioManager.instance.PlaySound("EquipClothe");
            PlayerPrefs.SetInt(PlayerPrefsManager.BUY_A_DRESS, 1);
            int id = (int)obj;
            for (int i = 0; i < listFashionItemsGame.Count; i++)
            {
                if (listFashionItemsGame[i].id == id)
                {
                    listFashionItemsGame[i].adIcon.gameObject.SetActive(false);
                    listFashionItemsGame[i].isAds = false;
                    listFashionItemsGame[i].OnItemSelected(id);
                    listFashionItemsGame[i].coinText.gameObject.SetActive(false);
                    listFashionItemsGame[i].sprBG.gameObject.SetActive(true);
                    listFashionItemsGame[i].buyButtonImage.gameObject.SetActive(false);
                    listFashionItemsGame[i].OwnerText.gameObject.SetActive(true);
                    EquipItem(listFashionItemsGame[i]);
                    SaveGame.AddElementToArray(listFashionItemsGame[i].id);
                    //  UpdateIndex();
                    CheckShowNextButton();
                }
            }

            logicUIFashion.UpdateFillImage(indexFillBar);
        }

        private void OnDestroy()
        {
            Observer.RemoveObserver(EventAction.EVENT_ITEM_SELECTED, ItemSelected);
            Observer.RemoveObserver(EventAction.EVENT_BUY_ITEM, HandleBuyItem);
            Observer.RemoveObserver(EventAction.EVENT_TRY_RELEASE_ITEM, HandleReleaseItem);
        }

        public float timer = 5;
        private bool isHint;

        private void Update()
        {
            timer -= Duck.TimeMod;
            if (timer <= 0 && !isHint)
            {
                timer = 5;
                isHint = true;
                ShowHint();
            }
            /*if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
                hit = Physics2D.Raycast(mousePos, Vector3.forward, Mathf.Infinity);
                if (hit.collider != null)
                {
                    ModelFashion modelFashion = hit.collider.GetComponent<ModelFashion>();
                    if (modelFashion != null)
                    {
                        if (modelFashion.fashionType == FashionType.FASHION_HAIR)
                        {
                            hairDefaultFashion.SetActive(true);
                        }

                        if (modelFashion.fashionType == FashionType.FASHION_TOP)
                        {
                            topDefaultFashion.SetActive(true);
                        }

                        if (modelFashion.fashionType == FashionType.FASHION_BOT)
                        {
                            botDefaultFashion.SetActive(true);
                        }

                        if (modelFashion.fashionType == FashionType.FASHION_OVERALL)
                        {
                            topDefaultFashion.SetActive(true);
                            botDefaultFashion.SetActive(true);
                        }

                        modelFashion.Collider.enabled = false;
                        UnequipFashion(modelFashion.id);
                        modelFashion.id = -1;
                        DOVirtual.DelayedCall(0.1f, (() => UpdateIndex()));
                        Destroy(modelFashion.itemFashionInit.gameObject);
                        CheckShowNextButton();
                    }
                }
            }*/
        }

        private int countItemTutUsed;
        public GameObject scaleFocus;
        
        public void ItemSelected(object obj)
        {
            // AudioManager.instance.PlaySound("EquipClothe");
            int idSelected = (int)obj;
            int tempId = -1;
            foreach (FashionItem item in listFashionItemsGame)
            {
                if (item.id == idSelected)
                {
                    for (int j = 0; j < model.listModelFashion.Count; j++)
                    {
                        if (item.fashionType == model.listModelFashion[j].fashionType && !item.isAds)
                        {
                            tempId = model.listModelFashion[j].id;
                            PlayStarEffect(model.listModelFashion[j].transform.position);
                        }
                    }
                }
            }

            foreach (FashionItem item in listFashionItemsGame)
            {
                if (item.id == tempId)
                {
                    item.itemFashionGame.isOnModel = false;
                    tempId = -1;
                }
            }

            for (int i = 0; i < listFashionItemsGame.Count; i++)
            {
                if (listFashionItemsGame[i].id == idSelected)
                {
                    if (listFashionItemsGame[i].isAds)
                    {
                        logicUIFashion.ShowPanelGetClothesPanel(listFashionItemsGame[i].icon.sprite,
                            listFashionItemsGame[i].price, idSelected);

                        /*if (tutorialFashion.isDoneTutFashion && !tutorialFashion.isDoneTutBuyInFashion)
                        {
                            Debug.Log("Start hint buy ???");
                            tutorialFashion.handHintBuy.SetActive(true);
                            tutorialFashion.handHint.SetActive(false);
                        }*/
                    }
                    else
                    {
                        EquipItem(listFashionItemsGame[i]);
                    }
                }
            }

            Duck.PlayParticle(sparkleBody);
            if (!tutorialFashion.isDoneTutFashion)
            {
                countItemTutUsed++;
                if (countItemTutUsed < 2)
                {
                    Debug.Log("??????");
                    scaleFocus.transform.position = listFashionItemsGot[0].transform.position;
                   // StartCoroutine(FocusTUTFashion.instance.ScaleFocus(8));
                    FocusTUTFashion.instance.hand.transform.position = 
                        listFashionItemsGot[0].transform.position + new Vector3(0.5f, 0.5f, 0);
                }
                else
                {
                    Debug.Log("Disable hand");
                    scaleFocus.SetActive(false);
                    tutorialFashion.isDoneTutFashion = true;
                    Destroy(tutorialFashion.handHint);
                    //tutorialFashion.handHint.SetActive(false); 
                    PlayerPrefs.SetInt(PlayerPrefsManager.TUT_FASHION, 1);
                }
                
                CheckShowNextButton();
            }
            logicUIFashion.UpdateFillImage(indexFillBar);
        }

        public void EquipItem(FashionItem fashionItem)
        {
            for (int i = 0; i < model.listModelFashion.Count; i++)
            {
                if (model.listModelFashion[i].fashionType == fashionItem.fashionType)
                {
                    if (model.listModelFashion[i].itemFashionInit != null)
                    {
                        UnequipFashion(model.listModelFashion[i].id);
                        model.listModelFashion[i].itemFashionInit.gameObject.SetActive(false);
                        Destroy(model.listModelFashion[i].itemFashionInit.gameObject);
                        if (model.listModelFashion[i].fashionType == FashionType.FASHION_OVERALL)
                        {
                            indexFillBar -= 2;
                        }
                        else
                        {
                            indexFillBar -= 1;
                        }
                    }

                    ItemFashionInit itemLoaded = Resources.Load<ItemFashionInit>($"Data/fashion_{fashionItem.id}");
                    ItemFashionInit itemInit =
                        Instantiate(itemLoaded, transform.position, Quaternion.identity);
                    itemInit.transform.SetParent(model.listModelFashion[i].transform);
                    itemInit.transform.localPosition = itemLoaded.positionOnModel;
                    itemInit.transform.localScale = Vector3.one;
                    model.listModelFashion[i].id = fashionItem.id;
                    model.listModelFashion[i].itemFashionInit = itemInit;
                    model.listModelFashion[i].Collider.enabled = true;

                    if (model.listModelFashion[i].fashionType == FashionType.FASHION_BOT)
                    {
                        botDefaultFashion.gameObject.SetActive(false);
                        for (int j = 0; j < model.listModelFashion.Count; j++)
                        {
                            if (model.listModelFashion[j].id != -1 &&
                                model.listModelFashion[j].fashionType == FashionType.FASHION_OVERALL)
                            {
                                model.listModelFashion[j].itemFashionInit.gameObject.SetActive(false);
                                model.listModelFashion[j].Collider.enabled = false;
                                model.listModelFashion[j].itemFashionInit = null;
                                UnequipFashion(model.listModelFashion[j].id);
                                model.listModelFashion[j].id = -1;
                                topDefaultFashion.gameObject.SetActive(true);
                            }
                        }

                        indexFillBar += 1;
                    }

                    if (model.listModelFashion[i].fashionType == FashionType.FASHION_TOP)
                    {
                        topDefaultFashion.gameObject.SetActive(false);
                        for (int j = 0; j < model.listModelFashion.Count; j++)
                        {
                            if (model.listModelFashion[j].id != -1 &&
                                model.listModelFashion[j].fashionType == FashionType.FASHION_OVERALL)
                            {
                                model.listModelFashion[j].itemFashionInit.gameObject.SetActive(false);
                                model.listModelFashion[j].Collider.enabled = false;
                                model.listModelFashion[j].itemFashionInit = null;
                                UnequipFashion(model.listModelFashion[j].id);
                                model.listModelFashion[j].id = -1;
                                botDefaultFashion.gameObject.SetActive(true);
                            }
                        }

                        indexFillBar += 1;
                    }

                    if (model.listModelFashion[i].fashionType == FashionType.FASHION_HAIR)
                    {
                        hairDefaultFashion.gameObject.SetActive(false);
                        indexFillBar += 1;
                    }

                    if (model.listModelFashion[i].fashionType == FashionType.FASHION_SHOES)
                    {
                        indexFillBar += 1;
                    }

                    if (model.listModelFashion[i].fashionType == FashionType.FASHION_OVERALL)
                    {
                        topDefaultFashion.gameObject.SetActive(false);
                        botDefaultFashion.gameObject.SetActive(false);
                        for (int j = 0; j < model.listModelFashion.Count; j++)
                        {
                            if (model.listModelFashion[j].id != -1 &&
                                model.listModelFashion[j].fashionType == FashionType.FASHION_TOP)
                            {
                                model.listModelFashion[j].itemFashionInit.gameObject.SetActive(false);
                                model.listModelFashion[j].Collider.enabled = false;
                                model.listModelFashion[j].itemFashionInit = null;
                                UnequipFashion(model.listModelFashion[j].id);
                                model.listModelFashion[j].id = -1;

                                indexFillBar -= 1;

                                //  botDefaultBotFashion.gameObject.SetActive(true);
                            }

                            if (model.listModelFashion[j].id != -1 &&
                                model.listModelFashion[j].fashionType == FashionType.FASHION_BOT)
                            {
                                model.listModelFashion[j].itemFashionInit.gameObject.SetActive(false);
                                model.listModelFashion[j].Collider.enabled = false;
                                model.listModelFashion[j].itemFashionInit = null;
                                UnequipFashion(model.listModelFashion[j].id);
                                model.listModelFashion[j].id = -1;

                                indexFillBar -= 1;

                                //   botDefaultBotFashion.gameObject.SetActive(true);
                            }
                        }

                        indexFillBar += 2;
                    }
                }
            }

            CheckShowNextButton();
            //UpdateIndex();
            Duck.PlayParticle(logicUIFashion.heartExplosion);
        }

        public FashionTab GetFashionTab(string theme)
        {
            switch (theme)
            {
                case THEME.ARCADE:
                case THEME.DEFAULT:
                    currentFashionTab = fashionTabs[0];
                    break;
                case THEME.SCHOOL:
                    currentFashionTab = fashionTabs[1];
                    break;
                case THEME.FASHION:
                    currentFashionTab = fashionTabs[2];
                    break;
                case THEME.BAR:
                    currentFashionTab = fashionTabs[3];
                    break;
                case THEME.RESTAURANT:
                    currentFashionTab = fashionTabs[4];
                    break;
                case THEME.PARK:
                    currentFashionTab = fashionTabs[5];
                    break;
                default:
                    currentFashionTab = null;
                    break;
            }

            return currentFashionTab;
        }

        [HideInInspector] public List<FashionItem> ItemsHaveSameTheme = new List<FashionItem>();

        public async void InitItemOnBoard()
        {
            for (int i = 0; i < dataFashion.Count; i++)
            {
                FashionItem fashionItem =
                    Instantiate(fashionItemPrefab, transform.position, Quaternion.identity);
                fashionItem.transform.SetParent(GetFashionTab(dataFashion[i].Theme).transform);
                fashionItem.transform.localScale = Vector3.zero;
                fashionItem.SetData(dataFashion[i].ID, dataFashion[i].Type, dataFashion[i].Price, dataFashion[i].Theme,
                    dataFashion[i].isAds, SetThemeSpr(dataFashion[i].Theme));

                listFashionItemsGame.Add(fashionItem);
                for (int k = 0; k < idUserGetted.Count; k++)
                {
                    if (fashionItem.id == idUserGetted[k])
                    {
                        listFashionItemsGot.Add(fashionItem);
                    }
                }

                if (fashionItem.theme == "DEFAULT")
                    listFashionItemsGot.Add(fashionItem);
                if (fashionItem.theme == GlobalData.currentTheme)
                    ItemsHaveSameTheme.Add(fashionItem);
            }

            StartCoroutine(GenerateFashionItems());
        }

        IEnumerator GenerateFashionItems()
        {
            for (int j = 0; j < listFashionItemsGame.Count; j++)
            {
                var a = j;
                listFashionItemsGame[a].adIcon.gameObject.SetActive(true);
                listFashionItemsGame[a].isAds = true;
                listFashionItemsGame[a].transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).SetDelay(0.2f);
            }

            yield return new WaitForSeconds(0.2f);
            for (int j = 0; j < listFashionItemsGot.Count; j++)
            {
                /*
                    if (!tutorialFashion.isDoneTutFashion)
                    {
                        tutorialFashion.handHint.transform.position = listFashionItemsGot[0].transform.position;
                        tutorialFashion.handHint.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).SetDelay(0.5f);
                        Debug.Log("Hand hint buy top on fist play game -");
                    }*/

                var a = j;
                listFashionItemsGot[j].transform.SetAsFirstSibling();
                listFashionItemsGot[a].adIcon.gameObject.SetActive(false);
                listFashionItemsGot[a].coinText.gameObject.SetActive(false);
                listFashionItemsGot[a].OwnerText.gameObject.SetActive(true);
                listFashionItemsGot[a].buyButtonImage.gameObject.SetActive(false);
                listFashionItemsGot[a].isAds = false;
                listFashionItemsGot[a].transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).SetDelay(0.2f);
                //UpdateIndex();
            }

            /*for (int k = 0; k < ItemsHaveSameTheme.Count; k++)
            {
                ItemsHaveSameTheme[k].transform.SetAsFirstSibling();
            }*/
        }

        public void UnequipFashion(int id)
        {
            for (int i = 0; i < listFashionItemsGame.Count; i++)
            {
                if (id == listFashionItemsGame[i].id)
                {
                    listFashionItemsGame[i].OnItemReleased();
                }
            }
        }

        public Sprite SetThemeSpr(string theme)
        {
            switch (theme)
            {
                case "ARCADE":
                    return arcadeSpr;
                    break;
                case "RESTAURANT":
                    return restaurantSpr;
                    break;
                case "FASHION":
                    return fashionSpr;
                    break;
                case "PARK":
                    return parkSpr;
                    break;
                case "SCHOOL":
                    return schoolSpr;
                    break;
                case "BAR":
                    return barSpr;
                    break;
                default:
                    return null;
            }
        }

        public void CheckShowNextButton()
        {
            if (model.CheckFashion())
            {
                logicUIFashion.nextButton.gameObject.SetActive(true);
            }
            else
            {
                logicUIFashion.nextButton.gameObject.SetActive(false);
            }

            indexFillBar = model.ItemOnModel();
            logicUIFashion.UpdateFillImage(model.ItemOnModel());
        }

        public void PlayStarEffect(Vector3 position)
        {
            starEffect.transform.position = position;
            Duck.PlayParticle(starEffect);
            ShowUserGetScore();
        }

        public TextMeshPro emotonText;
        public GameObject emotionBoxChat;

        private List<string> emtionsText = new List<string>()
            { "Wow", "Incredible", "Awesome", "Perfect", "Unbelievable","Excellent!","Amazing","Fantastic!","Good job!","Well done!"};

        public void ShowUserGetScore()
        {
            emotonText.text = emtionsText[Random.Range(0, emtionsText.Count)];
            emotionBoxChat.transform.DOKill();
            emotionBoxChat.transform.localScale = Vector3.zero;
            emotionBoxChat.transform.DOScale(0.75f, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                emotionBoxChat.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).SetDelay(1f);
            });
            model.GetComponent<Animator>().Play("GetScore", 0, 0);
        }


        TabFashionButton currentTabActive;
        public Image iconHintUser;
        public GameObject bubbleHint;

        void ShowHint()
        {
            isHint = false;
            int b = model.itemLacked();
            if (b == 0) return;
            for (int i = 0; i < fashionTabFashionButtons.Count; i++)
            {
                if (fashionTabFashionButtons[i].selected)
                {
                    currentTabActive = fashionTabFashionButtons[i];
                }
            }

            // get fashion type lacked
            int count = 0;
            switch (b)
            {
                case 2:
                    //hair lacked
                    for (int i = 0; i < fashionTabs[currentTabActive.id].transform.childCount; i++)
                    {
                        FashionItem item = fashionTabs[currentTabActive.id].transform.GetChild(i)
                            .GetComponent<FashionItem>();
                        if (item != null && item.fashionType == FashionType.FASHION_HAIR)
                        {
                            iconHintUser.sprite = item.itemFashionGame.GetComponent<Image>().sprite;
                            break;
                        }
                    }

                    break;
                case 7:
                    //top lacked
                    for (int i = 0; i < fashionTabs[currentTabActive.id].transform.childCount; i++)
                    {
                        FashionItem item = fashionTabs[currentTabActive.id].transform.GetChild(i)
                            .GetComponent<FashionItem>();
                        if (item != null && item.fashionType == FashionType.FASHION_TOP)
                        {
                            iconHintUser.sprite = item.itemFashionGame.GetComponent<Image>().sprite;
                            break;
                        }
                    }

                    break;
                case 8:
                    //overall lacked
                    for (int i = 0; i < fashionTabs[currentTabActive.id].transform.childCount; i++)
                    {
                        FashionItem item = fashionTabs[currentTabActive.id].transform.GetChild(i)
                            .GetComponent<FashionItem>();
                        if (item != null && item.fashionType == FashionType.FASHION_OVERALL)
                        {
                            iconHintUser.sprite = item.itemFashionGame.GetComponent<Image>().sprite;
                            break;
                        }
                    }

                    break;
                case 11:
                    //bot lacked
                    for (int i = 0; i < fashionTabs[currentTabActive.id].transform.childCount; i++)
                    {
                        FashionItem item = fashionTabs[currentTabActive.id].transform.GetChild(i)
                            .GetComponent<FashionItem>();
                        if (item != null && item.fashionType == FashionType.FASHION_BOT)
                        {
                            iconHintUser.sprite = item.itemFashionGame.GetComponent<Image>().sprite;
                            break;
                        }
                    }

                    break;
                case 13:
                    //shoes lacked
                    for (int i = 0; i < fashionTabs[currentTabActive.id].transform.childCount; i++)
                    {
                        FashionItem item = fashionTabs[currentTabActive.id].transform.GetChild(i)
                            .GetComponent<FashionItem>();
                        if (item != null && item.fashionType == FashionType.FASHION_SHOES)
                        {
                            iconHintUser.sprite = item.itemFashionGame.GetComponent<Image>().sprite;
                            break;
                        }
                    }

                    break;
            }

            ShowHintToPlayer();
        }

        private void ShowHintToPlayer()
        {
            bubbleHint.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).From(0).OnComplete((() =>
            {
                bubbleHint.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).SetDelay(0.5f);
            }));
        }
    }
}