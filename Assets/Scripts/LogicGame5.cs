using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace DevDuck
{
    public class LogicGame5 : MonoBehaviour
    {
        private RaycastHit2D hit;
        [SerializeField] GameObject toppingContainer;
        [SerializeField] FoodGame6 _foodGame6Prefab;
        [SerializeField] private List<GameObject> toppingPos = new List<GameObject>();
        [SerializeField] private ObjectPool pool;
        [SerializeField] private ParticleSystem hitPlateEffect;
        public static LogicGame5 ins;
        [SerializeField] private PlateFoodGame6 plateFoodGame6;
        [SerializeField] private LogicUiGame5 logicUiGame5;
        public FOODTYPE currentFOODTYPE;
        public List<NpcGame6> npcGame6s = new List<NpcGame6>();
        [SerializeField] private GameObject playerModel, mask, doorOpen, doorClose, foodDes;
        public bool isCanPlay;
        private int countFail = 3;
        [SerializeField] List<ParticleSystem> negativeParticles = new List<ParticleSystem>();
        [SerializeField] List<ParticleSystem> positiveParticles = new List<ParticleSystem>();
        public bool isDoneTutorial;
        public TutorialGame5 tutorial;

        private void Awake()
        {
            ins = this;
        }

        private void Start()
        {
            isDoneTutorial = PlayerPrefs.GetInt("isDoneTutorialGame5", 0) == 1;

            pool.SetupPool();
            OpenDoor();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && isCanPlay)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                hit = Physics2D.Raycast(mousePos, Vector3.forward, Mathf.Infinity);
                if (hit.collider == null) return;
                Tag o = hit.collider.GetComponent<Tag>();
                if (o.tagName == "Food")
                {
                    FoodGame6 food = o.GetComponent<FoodGame6>();
                    food.ToppingClicked(toppingContainer);
                    InsTopping(food.id);
                    plateFoodGame6.InitToppingOnPlate(food.id, food.type);
                }

                if (o.tagName == "Bell")
                {
                    plateFoodGame6.DefaultBell();
                    if (npcGame6s[0].orders.Contains(currentFOODTYPE))
                    {
                        isCanPlay = false;
                        plateFoodGame6.MoveFoodToDes(true);
                        npcGame6s[0].ShowTick(currentFOODTYPE);
                        npcGame6s[0].orders.Remove(currentFOODTYPE);
                        // PlayPositiveParticleEffect(new Vector3(4.3f, 2, 0));
                        if (npcGame6s[0].orders.Count == 0)
                        {
                            PlayPositiveParticleEffect(new Vector3(1, 4, 0));
                            DOVirtual.DelayedCall(1.5f, MoveNpc);
                            PlayerPrefs.SetInt("isDoneTutorialGame5", 1);
                            isDoneTutorial = true;
                            if (isDoneTutorial)
                            {
                                NpcMoveToTable();
                            }
                            Debug.Log("Has already full order");
                        }
                    }
                    else
                    {
                        plateFoodGame6.MoveFoodToDes(false);
                        countFail--;
                        logicUiGame5.DisableHeartPieces(countFail);
                        // PlayNegativeParticleEffect(new Vector3(4.3f, 2, 0));
                        if (countFail == 0)
                        {
                            logicUiGame5.ShowLoseUi();
                        }
                    }
                }
            }
        }

        private void InsTopping(int index)
        {
            // FoodGame6 topping =  Instantiate(_foodGame6Prefab, toppingPos[index].transform.position, Quaternion.identity);
            PooledObject food = pool.GetPooledObject(toppingPos[index].transform.position, Quaternion.identity);
            FoodGame6 topping = food.GetComponent<FoodGame6>();
            topping.id = index;
            topping.SetFoodSprite(index);
            topping.ToppingAppear();
        }

        IEnumerator InitToppingOnStart()
        {
            for (int i = 0; i < toppingPos.Count; i++)
            {
                yield return new WaitForSeconds(i * 0.2f);
                InsTopping(i);
            }
        }

        public void PlayParticleHitPlateEffect()
        {
            Duck.PlayParticle(hitPlateEffect);
        }

        private void OpenDoor()
        {
            doorClose.GetComponent<SpriteRenderer>().DOFade(0, 0.5f);
            doorOpen.GetComponent<SpriteRenderer>().DOFade(1, 0.5f).OnComplete(() =>
            {
                playerModel.transform.DOMove(new Vector3(3.5f, -2f, 0), 1);
                if (isDoneTutorial)
                {
                    NpcMoveToTable();
                    StartCoroutine(InitToppingOnStart());
                }
                else
                {
                    Debug.Log("In tutorial");
                    tutorial.NpcOnTutorial();
                    StartCoroutine(InitToppingOnStart());
                }
            });
        }

        private void NpcMoveToTable()
        {
            Sequence sequence = DOTween.Sequence();
            for (int i = 0; i < npcGame6s.Count; i++)
            {
                var a = i;
                sequence.Append(npcGame6s[a].transform.DOMove(new Vector3(0, 7.35f, 0), .4f).OnComplete(() =>
                {
                    npcGame6s[a].transform.DOScale(2 - (a * 0.1f), 1);
                }));
                sequence.Append(npcGame6s[a].transform.DOMove(new Vector3(0, 2 + (a * 1f), 0), 1f).OnComplete(() =>
                {
                    if (a == 3)
                    {
                        DOVirtual.DelayedCall(.4f, () =>isCanPlay = true);
                        Debug.Log("npc action done");
                        mask.SetActive(false);
                        npcGame6s[0].ShowOrder();
                    }
                }));
                sequence.AppendInterval(a * 0.2f);
            }
        }

        private void MoveNpc()
        {
            NpcGame6 gameObject = npcGame6s[0];
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

            npcGame6s.Remove(gameObject);
            if (npcGame6s.Count == 0)
            {
                logicUiGame5.ShowWinUi();
            }

            ArrangeNpcs();
        }

        private void ArrangeNpcs()
        {
            if (npcGame6s.Count == 0) return;
            npcGame6s[0].ShowOrder();
            Sequence sequence = DOTween.Sequence();
            for (int i = 0; i < npcGame6s.Count; i++)
            {
                //  var a = i;
                sequence.Append(npcGame6s[i].transform
                    .DOMove(npcGame6s[i].transform.position - new Vector3(0, 1, 0), .4f)
                    .SetDelay(0.3f));
                // sequence.AppendInterval(a * 0.2f);
            }
        }

        public void PlayNegativeParticleEffect(Vector3 position)
        {
            int index = Duck.GetRandom(0, negativeParticles.Count);
            negativeParticles[index].transform.position = position;
            Duck.PlayParticle(negativeParticles[index]);
        }

        public void PlayPositiveParticleEffect(Vector3 position)
        {
            int index = Duck.GetRandom(0, positiveParticles.Count);
            positiveParticles[index].transform.position = position;
            Duck.PlayParticle(positiveParticles[index]);
        }
    }
}