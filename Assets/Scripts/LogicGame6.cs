using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DevDuck
{
    public class LogicGame6 : MonoBehaviour
    {
        public static LogicGame6 Ins;
        [SerializeField] List<Food> foodsStart = new List<Food>();
        [SerializeField] List<GameObject> posFoods = new List<GameObject>();
        public List<NpcGame5> npcs = new List<NpcGame5>();
        RaycastHit2D hit;
        public  GameObject foodDes, plateFood, mask, playerModel, foodPar;
        Sequence mySequence;
        [SerializeField] GameObject foodPrefab;
        [SerializeField] private SpriteRenderer doorClose, doorOpen;
        public bool isCanPlay;
        public ParticleSystem smokeParticles;
        private int countFail = 6;

        public UiWinLose uiWinLose;
        public bool isDoneTutorial;
        public TutorialGame6 tutorialGame;
        public Animator effectCoinFlyUp;
        public TextMeshPro coinText;
        private int score;
        public Model model;
        public GameObject settingButton;
        private void Awake()
        {
            Ins = this;
        }

        private void Start()
        {
            GlobalData.isInGame = true;
            isDoneTutorial = PlayerPrefs.GetInt("isDoneTutorialGame6") == 1;
            OpenDoor();
            model.LoadAndInitFashion();
            ShowFoodOnTable();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && isCanPlay && GlobalData.isInGame)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.forward, Mathf.Infinity);
                if (hit.collider != null)
                {
                    mySequence = DOTween.Sequence();
                    Food o = hit.collider.gameObject.GetComponent<Food>();
                    if (isDoneTutorial)
                    {
                        CheckFood(o);
                    }
                    else
                    {
                        tutorialGame.npcGame5Tut.CheckFood(o);
                    }
                }
            }
        }

        private void CheckFood(Food o)
        {
            if (npcs.Count == 0) return;
            if (npcs[0].CheckNpcHasOrder(o.id) == false)
            {
                o.transform.DOJump(plateFood.transform.position, 0.5f, 1, 0.5f).OnComplete(() =>
                {
                    countFail--;
                    LogicUiGame6.instance.PlayNegativeParticles(new Vector3(1f, 4.28f, 0));
                    LogicUiGame6.instance.DisableHeartPieces(countFail);
                    o.ShowFailFood();
                    InitFoods(o.GetComponent<Food>().id);
                    if (countFail == 0)
                    {
                        uiWinLose.ShowLosePanel();
                    }
                });
            }
            else
            {
                isCanPlay = false;
                InitFoods(o.GetComponent<Food>().id);
                mySequence.Append(o.transform.DOJump(plateFood.transform.position, 0.5f, 1, 0.5f)
                    .OnComplete(() => Duck.PlayParticle(smokeParticles)));
                mySequence.Append(o.transform.DOJump(foodDes.transform.position, 0.5f, 1, 0.5f).SetDelay(0.5f));
                mySequence.Insert(1.1f, o.GetComponent<SpriteRenderer>().DOFade(0f, 0.15f).SetEase(Ease.InBack)
                    .OnComplete(() =>
                    {
                        isCanPlay = true;
                        LogicUiGame6.instance.PlayPositiveParticles(new Vector3(1f, 4.28f, 0));
                        if (npcs[0].CheckIsFullOrder() == false) return;
                        DOVirtual.DelayedCall(01f, MoveNpc);
                    }));
                score++;
                effectCoinFlyUp.Play("EffectGetCoin",0,0);
                // LogicUiGame5.instance.PlayPositiveParticles(new Vector3(2.7f, 1.6f, 0));
            }
        }

        public void MoveNpc()
        {
            NpcGame5 gameObject = npcs[0];
            int dir = Random.Range(0, 2);
            if (dir == 1)
            {
                gameObject.transform.DOMove(foodDes.transform.position + new Vector3(10, 0, 0), 1).SetDelay(0.3f)
                    .OnStart(
                        () => { gameObject.HideOrder(); });
            }
            else
            {
                gameObject.transform.DOMove(foodDes.transform.position + new Vector3(-10, 0, 0), 1).SetDelay(0.3f)
                    .OnStart(
                        () => { gameObject.HideOrder(); });
            }

            npcs.Remove(gameObject);
            ArrangeNpcs();
        }

        public void NpcMoveToTable()
        {
            Sequence sequence = DOTween.Sequence();
            for (int i = 0; i < npcs.Count; i++)
            {
                var a = i;
                sequence.Append(npcs[a].transform.DOMove(new Vector3(0, 7.35f, 0), .4f).OnComplete(() =>
                {
                    npcs[a].transform.DOScale(2 - (a * 0.1f), 1);
                }));
                sequence.Append(npcs[a].transform.DOMove(new Vector3(0, 2 + (a * 1f), 0), 1f).OnComplete(() =>
                {
                    if (a == 3)
                    {
                        mask.SetActive(false);
                        DOVirtual.DelayedCall(0.3f,(() => isCanPlay = true))  ;
                        npcs[0].ShowOrder();
                    }
                }));
                sequence.AppendInterval(a * 0.2f);
            }
        }

        private void OpenDoor()
        {
            doorClose.DOFade(0, .6f).SetDelay(0.3f);
            doorOpen.DOFade(1, .6f).SetDelay(0.3f).OnComplete(() =>
            {
                playerModel.transform.DOMove(new Vector3(2, -1.77f, 0), 1);
                if (isDoneTutorial)
                {
                    NpcMoveToTable();
                    ShowFoodOnTable();
                    tutorialGame.npcGame5Tut.gameObject.SetActive(false);
                }
                else
                {
                    tutorialGame.NpcTutorial();
                    DOVirtual.DelayedCall(0.2f, () => isCanPlay = true);
                }
            });
        }

        private void ArrangeNpcs()
        {
            isCanPlay = false;
            if (npcs.Count == 0)
            {
                settingButton.SetActive(false);
                uiWinLose.ShowWin3Star();
                Observer.Notify(EventAction.EVENT_GET_COINS, score*5);
            }
            else
            {
                npcs[0].ShowOrder();
                Sequence sequence = DOTween.Sequence();
                for (int i = 0; i < npcs.Count; i++)
                {
                    //  var a = i;
                    sequence.Append(npcs[i].transform.DOMove(npcs[i].transform.position - new Vector3(0, 1, 0), .4f)
                        .SetDelay(0.2f));
                    // sequence.AppendInterval(a * 0.2f);
                }
                DOVirtual.DelayedCall(0.2f *  npcs.Count, () => isCanPlay = true);
            }
        }

        public void InitFoods(int id)
        {
            Food food = Instantiate(foodPrefab.GetComponent<Food>(), posFoods[id - 1].gameObject.transform.position,
                Quaternion.identity);
            food.id = id;
            food.transform.SetParent(foodPar.transform);
            food.transform.localScale = Vector3.zero;
            food.gameObject.transform.DOScale(1, 0.3f).From(0).SetEase(Ease.OutBack).SetDelay(0.3f);
            food.transform.DOMove(posFoods[id - 1].gameObject.transform.position + new Vector3(0, 0.35f, 0), .1f)
                .SetDelay(0.35f).OnComplete(
                    () =>
                    {
                        food.BoxCollider2D.enabled = true;
                        food.transform.DOMove(posFoods[id - 1].gameObject.transform.position, .1f);
                    });
            food.SetFoodSprite(food.id);
        }

        public void ShowFoodOnTable()
        {
            Debug.Log("Start");
            for (int i = 0; i < foodsStart.Count; i++)
            {
                var a = i;
                foodsStart[a].gameObject.transform.DOScale(1, 0.2f).From(0).SetEase(Ease.OutBack)
                    .SetDelay(0.1f).OnComplete(() =>
                    {
                        foodsStart[a].BoxCollider2D.enabled = true;
                    });
            }
        }
    }
}