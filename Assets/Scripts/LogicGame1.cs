using System;
using DG.Tweening;
using Spine.Unity;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DevDuck
{
    [Serializable]
    public class ListItem
    {
        public List<itemGame1> items = new List<itemGame1>();
    }

    [Serializable]
    public class DataGame1
    {
        public List<ListItem> itemsGame1 = new List<ListItem>();
    }

    public class LogicGame1 : MonoBehaviour
    {
        public DataGame1 dataGame1 = new DataGame1();
        [SerializeField] LogicUiGame1 _logicUiGame1;
        public ShadowControl shadowControl;
        public itemGame1 pieceGame1;
        public static LogicGame1 instance;
        public SkeletonGraphic clawMachineHandle;
        public SkeletonAnimation handMachineHandle;
        public List<float> listDistances = new List<float>();
        [SerializeField] float appropriateRatio;
        public float scoreRun, score;
        private bool isRunDone, isShow;
        private int itemAmount;
        public List<itemGame1> itemGame1s = new List<itemGame1>();
        public List<itemGame1> itemGame1Done = new List<itemGame1>();
        public List<DesGame1> defaultPositions = new List<DesGame1>();
        [SerializeField] private ParticleSystem sparkleBody, starExplosionItem;

        public GameObject settings, coinBar;

        private int currentLevel;

        public itemGame1 tutorialItem;
        public bool isTutorial, isPressControl, isInSceneTutorial;
        public GameObject shadowTutorial, dropButton, handPress, handHintRotate, focusTutorial;
        public Button dropButtonUI;
        [SerializeField] private GameObject model;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else instance = this;

            isTutorial = PlayerPrefs.GetInt("isTutorial", 0) == 1 ? true : false;
        }

        private void Start()
        {
            coinBar.SetActive(false);
            SetData();
            Observer.AddObserver(EventAction.EVENT_POPUP_SHOW, ShowPopUp);
            handMachineHandle.AnimationState.SetAnimation(0, "hold", false);
            
            Debug.Log(shadowControl.pieceGame1.type);
            
            shadowControl.grabObject.transform.DOMove(new Vector3(-1, 7f, 0), 0.5f).SetDelay(1f).SetEase(Ease.OutBack);
            shadowControl.transform.DOMove(new Vector3(-1, shadowControl.yPosItem(shadowControl.pieceGame1), 0), 0.5f).SetDelay(1f).SetEase(Ease.OutBack).OnComplete(
                () =>
                {
                    shadowControl.timeText.transform.localScale = Vector3.one;
                    shadowControl.isEmpty = false;
                    shadowControl.timer = 20;
                    shadowControl.isCanMove = true;
                });
        }

        private void OnDestroy()
        {
            Observer.RemoveObserver(EventAction.EVENT_POPUP_SHOW, ShowPopUp);
        }

        private void Update()
        {
            if (isRunDone == false && isShow)
            {
                RunAppropriateText();
            }

            CheckDistanceItemsTutorial();
        }

        public void InsPieceGame1()
        {
            if (itemGame1s.Count > 0)
            {
                itemGame1 o = Instantiate(pieceGame1, shadowControl.transform.position, Quaternion.identity);
                o.transform.SetParent(shadowControl.posHold.transform);
                o.transform.localPosition = Vector3.zero;
                if (o.isOverSize)
                {
                    o.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
                else
                {
                    o.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                }

                o.type = itemGame1s[0].type;
                o.GetComponent<SpriteRenderer>().sprite = (itemGame1s[0].GetSprite());
                shadowControl._spriteShadow.sprite = itemGame1s[0].GetSprite();
                shadowControl.pieceGame1 = o;
                itemGame1Done.Add(o);
                itemGame1s.RemoveAt(0);
            }
            else
            {
                Debug.Log("CHECK WIN LOSE");
            }
        }

        private void ShowPopUp(object obj)
        {
            isShow = true;
            CheckDistanceItems();
            score = AverageDistances();
            _logicUiGame1.ShowScoreText();
        }

        float AverageDistances()
        {
            for (int i = 0; i < listDistances.Count; i++)
            {
                if (listDistances[i] < 0.5f)
                {
                    if (itemAmount == 2) appropriateRatio += Random.Range(45, 50);
                    if (itemAmount == 3) appropriateRatio += Random.Range(30, 34);
                    if (itemAmount == 4) appropriateRatio += Random.Range(20, 25);
                }
                else if (listDistances[i] < 0.6f)
                {
                    if (itemAmount == 2) appropriateRatio += Random.Range(30, 45);
                    if (itemAmount == 3) appropriateRatio += Random.Range(20, 30);
                    if (itemAmount == 4) appropriateRatio += Random.Range(15, 20);
                }
                else if (listDistances[i] < 1f)
                {
                    appropriateRatio += Random.Range(10, 15);
                }
                else
                {
                    appropriateRatio += Random.Range(0, 10);
                }
            }

            if (appropriateRatio >= 100)
                return 100;
            else
            {
                return appropriateRatio;
            }
        }

        private void RunAppropriateText()
        {
            scoreRun += 15 * Duck.TimeMod * 1.2f;
            _logicUiGame1.ScoreRunning(scoreRun);
            if (scoreRun >= score)
            {
                isRunDone = true;
                if (score > 50)
                {
                    _logicUiGame1.ShowWinPanel();
                    coinBar.SetActive(true);
                    settings.SetActive(false);
                    isShow = false;
                }
                else
                {
                    _logicUiGame1.ShowLosePanel();
                    coinBar.SetActive(true);
                    settings.SetActive(false);
                    isShow = false;
                }
            }
        }

        public void SetData()
        {
            if (!isTutorial)
            {
                tutorialItem = Instantiate(pieceGame1, new Vector3(0, 0, 0), Quaternion.identity);
                tutorialItem.transform.SetParent(shadowControl.posHold.transform);
                tutorialItem.transform.localPosition = Vector3.zero;
                tutorialItem.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                shadowControl._spriteShadow.sprite = tutorialItem.GetSprite();
                shadowControl.pieceGame1 = tutorialItem;
                shadowTutorial.gameObject.SetActive(true);
                focusTutorial.transform.position = (Handle.rectTransform.position);
                handHintRotate.transform.position = (Handle.rectTransform.position);
                dropButtonUI.enabled = false;
            }
            else
            {
                joyStick.enabled = true;
                ManagerGame.TIME_SCALE = 1;
                shadowControl.focusTutorial.gameObject.SetActive(false);
                shadowTutorial.gameObject.SetActive(false);
                shadowControl.focusTutorial.hand.gameObject.SetActive(false);
                if (PlayerPrefs.GetInt("LEVELGAME1") >= 3)
                {
                    PlayerPrefs.SetInt("LEVELGAME1", 1);
                }
                else
                {
                    if (GlobalData.isReplay)
                    {
                        PlayerPrefs.SetInt("LEVELGAME1", PlayerPrefs.GetInt("LEVELGAME1"));
                        Debug.Log("////////////");
                    }
                    else
                    {
                        PlayerPrefs.SetInt("LEVELGAME1", PlayerPrefs.GetInt("LEVELGAME1") + 1);
                        Debug.Log("-------------");

                    }
                }
                GlobalData.isReplay = false;
                currentLevel = PlayerPrefs.GetInt("LEVELGAME1") - 1;
                for (int i = 0; i < dataGame1.itemsGame1[currentLevel].items.Count; i++)
                {
                    itemGame1s.Add(dataGame1.itemsGame1[currentLevel].items[i]);
                }

                itemAmount = itemGame1s.Count;
                if (!isInSceneTutorial)
                {
                    InsPieceGame1();
                }
            }
        }

        public void CheckDistanceItems()
        {
            for (int i = 0; i < defaultPositions.Count; i++)
            {
                for (int j = 0; j < itemGame1Done.Count; j++)
                {
                    if (defaultPositions[i].Type == itemGame1Done[j].type)
                    {
                        listDistances.Add(Duck.GetDistance(defaultPositions[i].transform.localPosition,
                            itemGame1Done[j].transform.localPosition));
                    }
                }
            }
        }

        public void CalculateScore(itemGame1 item)
        {
            for (int i = 0; i < defaultPositions.Count; i++)
            {
                if (defaultPositions[i].Type == item.type)
                {
                    float distance = Duck.GetDistance(defaultPositions[i].transform.localPosition,
                        item.transform.localPosition);
                    if (distance < 1)
                    {
                        Duck.PlayParticle(sparkleBody);
                        _logicUiGame1.SetPosStarBar(0.1f);
                        starExplosionItem.transform.localPosition = item.transform.localPosition;
                        Duck.PlayParticle(starExplosionItem);
                        ShowUserGetScore();
                    }
                    else
                    {
                        _logicUiGame1.SetPosStarBar(-0.1f);
                    }
                }
            }
        }

        [FormerlySerializedAs("handleImage")] public FixedJoystick joyStick;

        public Image Handle;

        public void CheckDistanceItemsTutorial()
        {
            float distance = Vector2.Distance(shadowControl.transform.position,
                shadowTutorial.transform.position);
            if (distance < .2f && isTutorial == false && shadowControl.isDropTutorial == false)
            {
                if (isPressControl)
                {
                    shadowControl.focusTutorial.transform.position = dropButton.transform.position;
                    shadowControl.focusTutorial.gameObject.SetActive(true);
                    handPress.SetActive(true);
                    dropButtonUI.enabled = true;
                    // shadowControl.isCanMove = false;
                    joyStick.input = Vector2.zero;
                    Handle.rectTransform.anchoredPosition = Vector2.zero;
                    ManagerGame.TIME_SCALE = 0;
                }
            }
            else
            {
                if (isPressControl && shadowControl.isDropTutorial == false)
                {
                    // isPressControl = false;
                    shadowControl.focusTutorial.gameObject.SetActive(false);
                    //  shadowTutorial.gameObject.SetActive(false);
                }
            }
        }

        public void RunScoretutorial()
        {
            score = Random.Range(90, 100);
            RunAppropriateText();
        }

        public TextMeshPro emotonText;
        public GameObject emotionBoxChat;

        private List<string> emtionsText = new List<string>()
            { "Wow", "Incredible", "Awesome", "Perfect", "Unbelievable" };

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
    }
}