using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DevDuck
{
    public class LogicGame10EasyMode : MonoBehaviour
    {
        public static LogicGame10EasyMode instance;

        [HideInInspector]   public List<int> idUsed = new List<int>();
        [HideInInspector] public List<int> idCheck = new List<int>();
        public List<Sprite> spritesInGame = new List<Sprite>();
        public Sprite Sprite;
        [SerializeField] SpriteRenderer order1, order2, order3;
        public Animator bellAnim;
        [SerializeField] List<GameObject> customers = new List<GameObject>();
        [SerializeField] private int turn = 1;
        public GameObject boxOrder;
        public GameObject tableOrder;
        private int countSelect, countCorrect;
        public bool isStartSelect;
        public float timer;
        [Header("Board : ")] public List<FoodGame10> foodsOnBoard = new List<FoodGame10>();
        public List<int> idPieceShow = new List<int>();
        public Sprite plate;
        public SpriteRenderer avatar;
        public GameObject boxOrderOnBoard;
        public SpriteRenderer order1OnBoard, order2OnBoard, order3OnBoard;
        public GameObject clock, board;
        [SerializeField] private TextMeshPro timeText;
        public SpriteRenderer markSprite1, markSprite2, markSprite3;
        [SerializeField] private Sprite tickSpr, xSprite;
        public SpriteRenderer bgBoardSprite;

        [Header("Ui win lose")] [SerializeField]
        private UiWinLose uiWinLose;

        private RaycastHit2D hit;
        private Camera camera;
        bool isBoardActive;

        public Image tick1, tick2, tick3;
        public ParticleSystem sparkleEffect;
        public GameObject doorOpen, doorClose;
        public List<Sprite> avatarSprites = new List<Sprite>();
        public GameObject player, BgMain;
        public PlayParticleInGame playParticleInGame;

        public bool isDoneTut10EasyMode;
        public DataFoodGame10 dataFoodGame10;

        public Model modelInside;
        public GameObject settingButton;
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            isDoneTut10EasyMode = PlayerPrefs.GetInt("isDoneTut10EasyMode", 0) == 1;
            camera = Camera.main;
            OpenDoor();
            tick1.rectTransform.position = bellAnim.gameObject.transform.position;
            SetData();
            modelInside.LoadAndInitFashion();
            GlobalData.isInGame = true;
        }


        private void Update()
        {
            CountDown();
            if (Input.GetMouseButtonDown(0) && GlobalData.isInGame)
            {
                Vector3 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
                hit = Physics2D.Raycast(mousePos, Vector3.forward, Mathf.Infinity);
                if (hit.collider == null) return;
                Tag tag = hit.collider.gameObject.GetComponent<Tag>();
                if (tag.tagName == "NextButton")
                {
                    isBoardActive = true;
                    boxOrder.gameObject.SetActive(false);
                    boxOrder.transform.localScale = Vector3.zero;
                    tableOrder.gameObject.SetActive(true);
                    ShowTableOrder();
                    isStartSelect = true;
                    if (isDoneTut10EasyMode)
                    {
                        timer = 22;
                    }
                    else
                    {
                        timer = 101;
                    }

                    TutorialGame10.Instance.HandDisable();
                    BgMain.gameObject.SetActive(false);
                }

                if (tag.tagName == "FoodGame10")
                {
                    FoodGame10 food = hit.collider.gameObject.GetComponent<FoodGame10>();
                    food.ShowFoodSelected();
                    if (idCheck.Contains(food.id))
                    {
                        countSelect++;
                        countCorrect++;
                        SetMarkSprite(countSelect, true);
                        SetSpriteOrderDoneOrder(food);
                        idCheck.Remove(food.id);
                        CheckOrder();
                        food.BoxCollider2D.enabled = false;
                        TickAnim();
                        playParticleInGame.PlayParticle(true, avatar.transform.position + new Vector3(0, 0.5f, 0));
                        if (isDoneTut10EasyMode == false)
                        {
                            TutorialGame10.Instance.TurnOfHandAndBox();
                        }
                    }
                    else
                    {
                        countSelect++;
                        SetMarkSprite(countSelect, false);
                        SetSpriteOrderFaildOrder(food);
                        CheckOrder();
                        food.BoxCollider2D.enabled = false;
                        playParticleInGame.PlayParticle(false, avatar.transform.position + new Vector3(0, 0.5f, 0));
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadSceneAsync("Game10EasyMode");
            }
        }

        private void SetData()
        {
            idCheck.Clear();
            GetIdOrder(idUsed, spritesInGame.Count, 6);
            order1.sprite = spritesInGame[idUsed[0]];
            order2.sprite = spritesInGame[idUsed[1]];
            order3.sprite = spritesInGame[idUsed[2]];
            GetIdOrder(idPieceShow, 6, 6);
            SetSprites();
            for (int i = 0; i < 3; i++)
            {
                idCheck.Add(idUsed[i]);
            }
        }

        private void GetIdOrder(List<int> _list, int _max, int _amount)
        {
            _list.Clear();
            while (_list.Count < _amount)
            {
                int idRandom = Random.Range(0, _max);
                if (!_list.Contains(idRandom))
                {
                    _list.Add(idRandom);
                }
            }
        }

        private void SetSprites()
        {
            for (int i = 0; i < foodsOnBoard.Count; i++)
            {
                if (idUsed[i] < 12)
                {
                    foodsOnBoard[idPieceShow[i]].SpriteRenderer.sprite = plate;
                }
                else
                {
                    foodsOnBoard[idPieceShow[i]].SpriteRenderer.sprite = null;
                }

                foodsOnBoard[idPieceShow[i]].foodSpr.sprite = spritesInGame[idUsed[i]];
                foodsOnBoard[idPieceShow[i]].id = idUsed[i];
                for (int j = 0; j < dataFoodGame10.dataGame10.Count; j++)
                {
                    if (dataFoodGame10.dataGame10[j].id == foodsOnBoard[idPieceShow[i]].id)
                    {
                        foodsOnBoard[idPieceShow[i]].name.text = dataFoodGame10.dataGame10[j].name;
                        foodsOnBoard[idPieceShow[i]].description.text = dataFoodGame10.dataGame10[j].description;
                    }
                }
            }
        }

        void CustomerComeToTable(int turn)
        { 
            if ( turn <= customers.Count && customers[turn - 1] != null )
            {
                customers[turn - 1].transform.DOMove(new Vector2(0, 7.6f), 0.5f).OnComplete(() =>
                {
                    customers[turn - 1].transform.DOScale(0.72f, 0.5f);
                    customers[turn - 1].transform.DOMove(new Vector2(0, -1), 0.8f).OnComplete(() =>
                    {
                        boxOrder.gameObject.SetActive(true);
                        boxOrder.transform.DOScale(0.6f, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
                        {
                            bellAnim.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                            bellAnim.enabled = true;
                            bellAnim.Play("Bell", 0, 0);
                        });
                    });
                });
            }
        
        }

        void CustomerGoOut()
        {
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

            if (isDoneTut10EasyMode == true)
            {
                customers[turn - 2].transform.DOMoveX(xAxis, 0.5f).SetEase(Ease.Linear).SetDelay(0.5f).OnComplete((() =>
                {
                    CheckOrder();
                }));
            }
        }

        public void ShowTableOrder()
        {
            for (int i = 0; i < avatarSprites.Count; i++)
            {
                int spr = Random.Range(0, avatarSprites.Count);
                avatar.sprite = avatarSprites[spr];
                avatarSprites.Remove(avatarSprites[spr]);
            }

            bellAnim.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            bellAnim.Play("Default", 0, 0);
            order3OnBoard.transform.localScale = Vector3.zero;
            order2OnBoard.transform.localScale = Vector3.zero;
            order1OnBoard.transform.localScale = Vector3.zero;
            bgBoardSprite.DOColor(new Color32(255, 255, 255, 255), .2f);
            avatar.transform.DOMove(new Vector3(-3.3f, 6.5f, 0), 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                if (isDoneTut10EasyMode)
                {
                    boxOrderOnBoard.transform.DOScale(0.8f, 0.3f).SetEase(Ease.OutQuad);
                }
                else
                {
                    StartCoroutine(ShowHanHint());
                }
            });
            clock.transform.DOMove(new Vector3(2.8f, 4.3f, 0), 0.5f).SetEase(Ease.OutQuad).SetDelay(0.8f);
            board.transform.DOMove(new Vector3(0f, -3f, 0), 0.5f).SetEase(Ease.OutQuad).SetDelay(0.8f)
                .OnComplete((() => ShowListOrder()));
        }

        private void ShowListOrder()
        {
            /*order1.sprite = LogicGame10.Instance.order1.sprite;
            order2.sprite = LogicGame10.Instance.order2.sprite;
            order3.sprite = LogicGame10.Instance.order3.sprite;*/

            for (int i = 0; i < foodsOnBoard.Count; i++)
            {
                foodsOnBoard[i].BoxCollider2D.enabled = true;
                foodsOnBoard[i].spriteSelected.color = new Color32(255, 255, 255, 0);
                foodsOnBoard[i].transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack).From(0).SetDelay(0.1f * i);
            }
        }

        private void HideListOrder()
        {
            for (int i = 0; i < foodsOnBoard.Count; i++)
            {
                foodsOnBoard[i].BoxCollider2D.enabled = false;
            }
        }

        public void SetTimer(float time)
        {

            if (isDoneTut10EasyMode )
            {
                if (time > 20)
                {
                    timeText.text = "20s";
                    timeText.color = Color.white;
                }
                else
                {
                    timeText.text = Mathf.RoundToInt(time).ToString() + "s";
                    if (time <= 5.1f)
                    {
                        timeText.color = Color.red;
                        if (time < 1.1f)
                        {
                            ShakeClock();
                        }
                    }
                    else if (time <= 10.1f)
                    {
                        timeText.color = Color.yellow;
                    }
                    else
                    {
                        timeText.color = Color.white;
                    }
                }
            }
            else
            {
                if (time > 99)
                {
                    timeText.text = "99s";
                    timeText.color = Color.white;
                }
                else
                {
                    timeText.text = Mathf.RoundToInt(time).ToString() + "s";
                    if (time <= 5.1f)
                    {
                        timeText.color = Color.red;
                        if (time < 1.1f)
                        {
                            ShakeClock();
                        }
                    }
                    else if (time <= 10.1f)
                    {
                        timeText.color = Color.yellow;
                    }
                    else
                    {
                        timeText.color = Color.white;
                    }
                }
            }
            
            
          
        }

        private void ShakeClock()
        {
            bool isShake = false;
            if (!isShake)
            {
                isShake = true;
                clock.transform.DOShakeRotation(0.3f, new Vector3(0, 0, 3), 1).OnComplete((() =>
                {
                    clock.transform.rotation = Quaternion.identity;
                }));
            }
        }

        public void HideTableOrder()
        {
            tick1.gameObject.SetActive(false);
            tick2.gameObject.SetActive(false);
            tick3.gameObject.SetActive(false);
            markSprite1.sprite = null;
            markSprite2.sprite = null;
            markSprite3.sprite = null;
            isBoardActive = false;
            board.transform.DOMoveX(-20, 0.5f).SetEase(Ease.InBack);
            clock.transform.DOMoveX(15, 0.5f).SetEase(Ease.Linear).SetDelay(0.35f);
            avatar.transform.DOMoveX(-20, 0.5f).SetEase(Ease.Linear).SetDelay(0.7f);
            boxOrderOnBoard.transform.DOScale(0, 0.4f).SetEase(Ease.InBack).SetDelay(0.8f);
            bgBoardSprite.DOFade(0, 0.5f).SetEase(Ease.InBack).SetDelay(1f).OnComplete(() =>
            {
                for (int i = 0; i < foodsOnBoard.Count; i++)
                {
                    foodsOnBoard[i].transform.localScale = Vector3.zero;
                }

                CustomerGoOut();
                BgMain.gameObject.SetActive(true);
            });
        }

        public void SetMarkSprite(int count, bool isCorrect)
        {
            if (isCorrect)
            {
                if (count == 1)
                {
                    markSprite1.sprite = null;
                    markSprite1.transform.localPosition = new Vector3(0, -0.68f, 0);
                }

                if (count == 2)
                {
                    markSprite2.sprite = null;
                    markSprite2.transform.localPosition = new Vector3(0, -0.68f, 0);
                }

                if (count == 3)
                {
                    markSprite3.sprite = null;
                    markSprite3.transform.localPosition = new Vector3(0, -0.68f, 0);
                }
            }
            else
            {
                if (count == 1)
                {
                    markSprite1.sprite = xSprite;
                    markSprite1.transform.localPosition = new Vector3(0, 0f, 0);
                }

                if (count == 2)
                {
                    markSprite2.sprite = xSprite;
                    markSprite2.transform.localPosition = new Vector3(0, 0f, 0);
                }

                if (count == 3)
                {
                    markSprite3.sprite = xSprite;
                    markSprite3.transform.localPosition = new Vector3(0, 0f, 0);
                }
            }
        }

        private void CheckOrder()
        {
            if (idCheck.Count == 0 || countSelect == 3  || turn == 4 )
            {
                countSelect = 0;
                if (turn >= 3)
                {
                    settingButton.gameObject.SetActive(false);
                    if (countCorrect <= 3)
                    {
                        DOVirtual.DelayedCall(0.5f, () => uiWinLose.ShowLosePanel());
                    }
                    else if (countCorrect <= 7)
                    {
                        DOVirtual.DelayedCall(0.5f, () => uiWinLose.ShowWin2Star());
                    }
                    else if (countCorrect <= 9)
                    {
                        DOVirtual.DelayedCall(0.5f, () => uiWinLose.ShowWin3Star());
                    }

                    Observer.Notify(EventAction.EVENT_GET_COINS, countCorrect * 5);
                    speedTimer = 0;
                    HideListOrder();
                }
                else
                {
                    /*LogicGame10EasyMode.instance.*/
                    HideListOrder();
                    PerformAnimEnd();
                    turn++;
                    DOVirtual.DelayedCall(2f, () => HideTableOrder());
                    DOVirtual.DelayedCall(4.5f, () =>
                    {
                        isDoneTut10EasyMode = true;
                        CustomerComeToTable(turn);
                        SetData();
                    });
                }
            }
        }

        private void SetSpriteOrderDoneOrder(FoodGame10 food)
        {
            if (countSelect == 1)
            {
                order1OnBoard.sprite = food.foodSpr.sprite;
                order1OnBoard.transform.DOScale(1, 0.2f);
            }
            else if (countSelect == 2)
            {
                order2OnBoard.sprite = food.foodSpr.sprite;
                order2OnBoard.transform.DOScale(1, 0.2f);
            }
            else if (countSelect == 3)
            {
                order3OnBoard.sprite = food.foodSpr.sprite;
                order3OnBoard.transform.DOScale(1, 0.2f);
            }
        }

        private void SetSpriteOrderFaildOrder(FoodGame10 food)
        {
            switch (countSelect)
            {
                case 1:
                    order1OnBoard.sprite = food.foodSpr.sprite;
                    order1OnBoard.transform.DOScale(1, 0.2f);
                    break;
                case 2:
                    order2OnBoard.sprite = food.foodSpr.sprite;

                    order2OnBoard.transform.DOScale(1, 0.2f);
                    break;
                case 3:
                    order3OnBoard.sprite = food.foodSpr.sprite;
                    order3OnBoard.transform.DOScale(1, 0.2f);
                    break;
            }
        }

        private int speedTimer = 1;

        private void CountDown()
        {
            if (isStartSelect)
            {
                timer -= Duck.TimeMod * speedTimer;
                SetTimer(timer);
                if (timer <= 0)
                {
                    isStartSelect = false;
                    countSelect = 0;
                    timer = 22;
                    timeText.color = Color.white;
                    if (isBoardActive)
                    {
                        turn++;
                        HideListOrder();
                        PerformAnimEnd();
                        DOVirtual.DelayedCall(2f, () => HideTableOrder());
                        DOVirtual.DelayedCall(4.5f, () =>
                        {
                            CustomerComeToTable(turn);
                            SetData();
                        });
                    }
                }
            }
        }

        private void TickAnim()
        {
            if (countSelect == 1)
            {
                sparkleEffect.transform.position = order1OnBoard.transform.position;
                Duck.PlayParticle(sparkleEffect);
                tick1.gameObject.SetActive(true);
                tick1.transform.position = order1OnBoard.transform.position;
                tick1.DOFillAmount(1, 0.3f).From(0).SetDelay(0.3f);
            }
            else if (countSelect == 2)
            {
                sparkleEffect.transform.position = order2OnBoard.transform.position;
                Duck.PlayParticle(sparkleEffect);
                tick2.gameObject.SetActive(true);
                tick2.transform.position = order2OnBoard.transform.position;
                tick2.DOFillAmount(1, 0.3f).From(0).SetDelay(0.3f);
            }
            else
            {
                sparkleEffect.transform.position = order3OnBoard.transform.position;
                Duck.PlayParticle(sparkleEffect);
                tick3.gameObject.SetActive(true);
                tick3.transform.position = order3OnBoard.transform.position;
                tick3.DOFillAmount(1, 0.3f).From(0).SetDelay(0.3f);
            }
        }

        private void PerformAnimEnd()
        {
            if (tick1.gameObject.activeSelf)
            {
                GreenShow(order1OnBoard);
                ScaleShow(order1OnBoard);
            }
            else
            {
                RedShow(order1OnBoard);
                ScaleShow(order1OnBoard);
            }

            if (tick2.gameObject.activeSelf)
            {
                DOVirtual.DelayedCall(0.6f, () => GreenShow(order2OnBoard));
                DOVirtual.DelayedCall(0.6f, () => ScaleShow(order2OnBoard));
            }
            else
            {
                DOVirtual.DelayedCall(0.6f, () => RedShow(order2OnBoard));
                DOVirtual.DelayedCall(0.6f, () => ScaleShow(order2OnBoard));
            }

            if (tick3.gameObject.activeSelf)
            {
                DOVirtual.DelayedCall(1.2f, () => GreenShow(order3OnBoard));
                DOVirtual.DelayedCall(1.2f, () => ScaleShow(order3OnBoard));
            }
            else
            {
                DOVirtual.DelayedCall(1.2f, () => RedShow(order3OnBoard));
                DOVirtual.DelayedCall(1.2f, () => ScaleShow(order3OnBoard));
            }
        }

        private void ScaleShow(SpriteRenderer spriteRenderer)
        {
            spriteRenderer.transform.DOScale(1.1F, 0.2f).OnComplete(() =>
            {
                spriteRenderer.transform.DOScale(1F, 0.2f).OnComplete(() =>
                {
                    spriteRenderer.transform.DOScale(1.1F, 0.2f).OnComplete(() =>
                    {
                        spriteRenderer.transform.DOScale(1F, 0.2f);
                    });
                });
            });
        }

        private void GreenShow(SpriteRenderer spriteRenderer)
        {
            spriteRenderer.DOColor(new Color32(100, 255, 0, 255), .2f).OnComplete((() =>
            {
                spriteRenderer.DOColor(new Color32(255, 255, 255, 255), .2f).OnComplete((() =>
                {
                    spriteRenderer.DOColor(new Color32(100, 255, 0, 255), .2f).OnComplete((() =>
                    {
                        spriteRenderer.DOColor(new Color32(255, 255, 255, 255), .2f);
                    }));
                }));
            }));
        }

        private void RedShow(SpriteRenderer spr)
        {
            spr.DOColor(new Color32(220, 50, 50, 255), .2f).OnComplete((() =>
            {
                spr.DOColor(new Color32(255, 255, 255, 255), .2f).OnComplete((() =>
                {
                    spr.DOColor(new Color32(220, 50, 50, 255), .2f).OnComplete((() =>
                    {
                        spr.DOColor(new Color32(255, 255, 255, 255), .2f);
                    }));
                }));
            }));
        }

        private void OpenDoor()
        {
            player.transform.DOMove(new Vector3(-2.87f, -7.53f, 0), 1f);
            doorClose.transform.DORotate(new Vector3(0, 90, 0), 0.3f).OnComplete(() =>
            {
                doorOpen.GetComponent<SpriteRenderer>().DOFade(1, 0.3f).From(0).OnComplete((() =>
                {
                    if (isDoneTut10EasyMode)
                    {
                        CustomerComeToTable(turn);
                    }
                    else
                    {
                        TutorialGame10.Instance.CustomerMoveToTable();
                        ShowListOrder();
                    }
                }));
            });
        }

        IEnumerator ShowHanHint()
        {
            yield return new WaitForSeconds(1.5f);
            for (int i = 0; i < foodsOnBoard.Count; i++)
            {
                if (foodsOnBoard[i].id == idCheck[0])
                {
                    TutorialGame10.Instance.handHintPressOreder.transform.position = foodsOnBoard[i].transform.position;
                    TutorialGame10.Instance.handHintPressOreder.SetActive(true);
                }
            }
        }
    }
}