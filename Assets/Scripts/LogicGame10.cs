using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace DevDuck
{
    public class LogicGame10 : MonoBehaviour
    {
        private RaycastHit2D hit;
        Camera camera;
        [SerializeField] private TableOrderGame10 tableOrder;
        private float timer;
        [SerializeField] List<GameObject> customers = new List<GameObject>();
        [SerializeField] List<Sprite> listOrder1 = new List<Sprite>();
        [SerializeField] List<Sprite> listOrder2 = new List<Sprite>();
        [SerializeField] List<Sprite> listOrder3 = new List<Sprite>();
        public GameObject boxOrder;
        public SpriteRenderer order1, order2, order3;
        public List<int> idOrders = new List<int>();
        [SerializeField] List<int> idList = new List<int>();
        List<int> idListDefault = new List<int>() { 1, 2, 3 };
        [SerializeField] List<FoodGame10> foodOnBoard = new List<FoodGame10>();
        [SerializeField] private int turn = 1;
        [SerializeField] private Sprite plateSprite;
        public Animator bellAnim;
        public static LogicGame10 Instance;
        public bool isStartSelect;
        private int countPlayerSelected;
        private int countSelect;
        public int countCorrect;
        [SerializeField] private Sprite xIcon;
        [SerializeField] private UiWinLose uiWinLose;
        public GameObject doorOpen, doorClose;
        public GameObject player;

        public bool isDoneTutorial;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            isDoneTutorial = PlayerPrefs.GetInt("isDoneTutorial10", 0) == 1;

            camera = Camera.main;
            OpenTheDoor();
            idList = Duck.GenerateDerangement(idListDefault);
            InitOrderOnStart();
        }

        void Update()
        {
            CountDown();
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
                hit = Physics2D.Raycast(mousePos, Vector3.forward, Mathf.Infinity);
                if (hit.collider == null) return;
                Tag tag = hit.collider.gameObject.GetComponent<Tag>();
                if (tag.tagName == "NextButton")
                {
                    boxOrder.gameObject.SetActive(false);
                    boxOrder.transform.localScale = Vector3.zero;
                    tableOrder.gameObject.SetActive(true);
                    tableOrder.ShowTableOrder();
                    isStartSelect = true;
                    timer = 22;
                }

                if (tag.tagName == "FoodGame10")
                {
                    FoodGame10 food = hit.collider.gameObject.GetComponent<FoodGame10>();
                    food.ShowFoodSelected();
                    if (idOrders.Contains(food.id))
                    {
                        countSelect++;
                        countCorrect++;
                        tableOrder.SetMarkSprite(countSelect, true);
                        SetSpriteOrderDoneOrder(food);
                        idOrders.Remove(food.id);
                        CheckOrder();
                    }
                    else
                    {
                        countSelect++;
                        //  tableOrder.HideTableOrder();
                        tableOrder.SetMarkSprite(countSelect, false);
                        SetSpriteOrderFaildOrder();
                        CheckOrder();
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadSceneAsync("Game10HardMode");
            }
        }
        private void CheckOrder()
        {
            if (idOrders.Count == 0 || countSelect == 3)
            {
                countSelect = 0;
                Debug.Log("turn    :  " + turn);
                if (turn >= 3)
                {
                    if (countCorrect <= 3) uiWinLose.ShowLosePanel();
                    else if (countCorrect <= 7) uiWinLose.ShowWin2Star();
                    else if (countCorrect <= 9) uiWinLose.ShowWin3Star();
                }
                else
                {
                    CustomerGoOut();

                    turn++;
                    tableOrder.HideTableOrder();
                    DOVirtual.DelayedCall(3.5f, () =>
                    {
                        CustomerComeToTable(turn);
                        InitOrderOnStart();
                    });
                }
            }
        }

        private void SetSpriteOrderDoneOrder(FoodGame10 food)
        {
            var tempList = GetListOrder();
            if (countSelect == 1)
            {
                tableOrder.order1.sprite = tempList[food.id];
                tableOrder.order1.transform.DOScale(1, 0.2f);
            }
            else if (countSelect == 2)
            {
                tableOrder.order2.sprite = tempList[food.id];
                tableOrder.order2.transform.DOScale(1, 0.2f);
            }
            else if (countSelect == 3)
            {
                tableOrder.order3.sprite = tempList[food.id];
                tableOrder.order3.transform.DOScale(1, 0.2f);
            }
        }

        private void SetSpriteOrderFaildOrder()
        {
            switch (countSelect)
            {
                case 1:
                    tableOrder.order1.transform.DOScale(1, 0.2f);
                    break;
                case 2:
                    tableOrder.order2.transform.DOScale(1, 0.2f);
                    break;
                case 3:
                    tableOrder.order3.transform.DOScale(1, 0.2f);
                    break;
            }
        }

        public void InitOrderOnStart()
        {
            idOrders.Clear();
            while (idOrders.Count < 3)
            {
                int id = Duck.GetRandom(0, 6);
                if (!idOrders.Contains(id))
                {
                    idOrders.Add(id);
                }
            }

            SetSpriteOrder();
            SetDataBoardOrder();
        }

        private void SetSpriteOrder()
        {
            switch (idList[turn - 1])
            {
                case 1:
                    order1.sprite = listOrder1[idOrders[0]];
                    order2.sprite = listOrder1[idOrders[1]];
                    order3.sprite = listOrder1[idOrders[2]];
                    break;
                case 2:
                    order1.sprite = listOrder2[idOrders[0]];
                    order2.sprite = listOrder2[idOrders[1]];
                    order3.sprite = listOrder2[idOrders[2]];
                    break;
                case 3:
                    order1.sprite = listOrder3[idOrders[0]];
                    order2.sprite = listOrder3[idOrders[1]];
                    order3.sprite = listOrder3[idOrders[2]];
                    break;
            }
        }

        private void SetDataBoardOrder()
        {
            var list = GetListOrder();
            for (int i = 0; i < foodOnBoard.Count; i++)
            {
                foodOnBoard[i].foodSpr.sprite = list[i];
                if (idList[turn - 1] == 3)
                {
                    foodOnBoard[i].SpriteRenderer.sprite = null;
                }
                else
                {
                    foodOnBoard[i].SpriteRenderer.sprite = plateSprite;
                }
            }
        }

        public List<Sprite> GetListOrder()
        {
            switch (idList[turn - 1])
            {
                case 1:
                    return listOrder1;
                    break;
                case 2:
                    return listOrder2;
                    break;
                case 3:
                    return listOrder3;
                    break;
                default:
                    return null;
            }
        }

        void CustomerComeToTable(int turn)
        {
            customers[turn - 1].transform.DOMove(new Vector2(0, 7.6f), 0.5f).OnComplete(() =>
            {
                customers[turn - 1].transform.DOScale(0.9f, 0.5f);
                customers[turn - 1].transform.DOMove(new Vector2(0, -1), 0.8f).OnComplete((() =>
                {
                    boxOrder.gameObject.SetActive(true);
                    boxOrder.transform.DOScale(0.6f, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
                    {
                        bellAnim.enabled = true;
                        bellAnim.Play("Bell", 0, 0);
                    });
                }));
            });
        }

        void CustomerGoOut()
        {
            bellAnim.Play("Default", 0, 0);
            int rand = Duck.GetRandom(0, 2);
            int xAxis = 0;
            if (rand == 0)
            {
                xAxis = -15;
            }
            else
            {
                xAxis = 15;
            }

            if (isDoneTutorial)
            {
                customers[turn - 1].transform.DOMoveX(xAxis, 0.5f).SetEase(Ease.Linear).SetDelay(2f);
            }
           
        }

        private void CountDown()
        {
            if (isStartSelect)
            {
                timer -= Duck.TimeMod;
                tableOrder.SetTimer(timer);
                if (timer <= 0)
                {
                    isStartSelect = false;
                    timer = 22;
                }
            }
        }

        public void OpenTheDoor()
        {
            doorClose.transform.DORotate(new Vector3(0, 90, 0), 0.5f).OnComplete(() =>
            {
                doorOpen.gameObject.SetActive(true);
                doorOpen.GetComponent<SpriteRenderer>().DOFade(1, 0.3f).From(0);
                doorClose.gameObject.SetActive(false);
                player.transform.DOMove(new Vector3(-3.17f, -7.53f, 0), 0.5f);

                Debug.Log(isDoneTutorial + "  isDoneTutorial");
                if (isDoneTutorial)
                {
                    CustomerComeToTable(turn);
                }
                else
                {
                    TutorialGame10.Instance.CustomerMoveToTable();
                    InitOrderOnStart();
                }
            });
        }
    }
}