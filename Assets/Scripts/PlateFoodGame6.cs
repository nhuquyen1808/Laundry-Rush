using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace DevDuck
{
    public enum FOODTYPE
    {
        BURGER_TYPE_1,
        BURGER_TYPE_2,
        BURGER_TYPE_3,
        BURGER_TYPE_4,
        NONE
    }

    public class PlateFoodGame6 : MonoBehaviour
    {
        [SerializeField] List<FoodLayer> _foodLayers = new List<FoodLayer>();
        [SerializeField] List<Sprite> foodFlagSprites = new List<Sprite>();
        public SpriteRenderer flag;
        [SerializeField] List<int> idToppingsOnPlate = new List<int>();
        private List<int> idFood1 = new List<int>() { 0, 2, 3 };
        private List<int> idFood2 = new List<int>() { 0, 1, 3 };
        private List<int> idFood3 = new List<int>() { 0, 1, 2, 3 };
        private List<int> idFood4 = new List<int>() { 3, 2, 1 };
        int countToppingsOnPlate = 0;
        public GameObject dishesPrefab, dishesOnPlate, flagPrefab, desOnTable;
        [SerializeField] private ParticleSystem smokeHideFood;
        [SerializeField] private List<SpriteRenderer> toppingsOnPlate = new List<SpriteRenderer>();
        [SerializeField] private GameObject bell;
        
        private void Start()
        {
            InitDishPrefab();
            DefaultBell();
        }
        public void InitToppingOnPlate(int idTopping, TOPPING_TYPE type)
        {
            for (int i = 0; i < _foodLayers.Count; i++)
            {
                if (_foodLayers[i].type == type)
                {
                    _foodLayers[i].layer = idTopping;
                    if (idTopping != 3)
                    {
                        countToppingsOnPlate++;
                        _foodLayers[i].SetSpriteFoodLayer(idTopping);
                        if (_foodLayers[0].layer != -1)
                        {
                            _foodLayers[_foodLayers.Count - 1].SetSpriteFoodLayer(3);
                        }
                    }
                    else
                    {
                        if (countToppingsOnPlate < 1)
                        {
                            _foodLayers[2].SetSpriteFoodLayer(3);
                        }
                        else
                        {
                            _foodLayers[_foodLayers.Count - 1].SetSpriteFoodLayer(3);
                        }

                        _foodLayers[0].SetSpriteFoodLayer(-1);
                    }
                }
            }

            CheckTypeOfFood();
        }

        public void PeformAnimFlag(int idFlag)
        {
            ShakeBell();

            Vector2 flagPos = new Vector3(0.75f, 1.5f);
            flag.gameObject.transform.localPosition = flagPos + new Vector2(0.4f, 1f);
            flag.gameObject.SetActive(true);
            flag.sprite = foodFlagSprites[idFlag - 1];
            flag.DOFade(1f, 0.5f).From(0);
            flag.transform.DOLocalMove(flagPos, .7f).SetEase(Ease.OutQuad);
        }
        void CheckTypeOfFood()
        {
            bool isAdded = false;
            idToppingsOnPlate.Clear();
            for (int i = 0; i < _foodLayers.Count; i++)
            {
                if (_foodLayers[i].layer >= 0 && _foodLayers[i].layer != 3)
                {
                    idToppingsOnPlate.Add(_foodLayers[i].layer);
                }

                if (_foodLayers[i].layer == 3 && isAdded == false)
                {
                    idToppingsOnPlate.Add(_foodLayers[i].layer);
                    isAdded = true;
                }
            }

            //  Debug.Log(BurgerType());
            LogicGame5.ins.currentFOODTYPE = BurgerType();
        }

        public FOODTYPE BurgerType()
        {
            if (idFood1.All(item => idToppingsOnPlate.Contains(item) == true) &&
                idFood1.Count == idToppingsOnPlate.Count)
            {
                PeformAnimFlag(1);
                return FOODTYPE.BURGER_TYPE_1;
            }

            if (idFood2.All(item => idToppingsOnPlate.Contains(item) == true) &&
                idFood2.Count == idToppingsOnPlate.Count)
            {
                PeformAnimFlag(2);

                return FOODTYPE.BURGER_TYPE_2;
            }

            if (idFood3.All(item => idToppingsOnPlate.Contains(item) == true) &&
                idFood3.Count == idToppingsOnPlate.Count)
            {
                PeformAnimFlag(3);

                return FOODTYPE.BURGER_TYPE_3;
            }

            if (idFood4.All(item => idToppingsOnPlate.Contains(item) == true) &&
                idFood4.Count == idToppingsOnPlate.Count)
            {
                PeformAnimFlag(4);

                return FOODTYPE.BURGER_TYPE_4;
            }
            else
            {
                return FOODTYPE.NONE;
            }
        }

        public void SetDefautId()
        {
            idToppingsOnPlate.Clear();
            for (int i = 0; i < _foodLayers.Count; i++)
            {
                _foodLayers[i].layer = -1;
            }
        }

        private void InitDishPrefab()
        {
            _foodLayers.Clear();
            toppingsOnPlate.Clear();
            GameObject dish = Instantiate(dishesPrefab, transform.position, Quaternion.identity);
            dish.transform.SetParent(transform);
            dish.transform.localPosition = Vector3.zero;
            dish.transform.localScale = Vector3.one;
            dishesOnPlate = dish;
            for (int i = 0; i < dish.transform.childCount; i++)
            {
                if (dish.transform.GetChild(i).GetComponent<FoodLayer>() != null)
                {
                    _foodLayers.Add(dish.transform.GetChild(i).GetComponent<FoodLayer>());
                    toppingsOnPlate.Add(dish.transform.GetChild(i).GetComponent<SpriteRenderer>());
                }
            }

            InitFlag(dish);
        }

        private void InitFlag(GameObject dish)
        {
            GameObject flag = Instantiate(flagPrefab, dish.transform.position, Quaternion.identity);
            flag.transform.SetParent(dish.transform);
            this.flag = flag.GetComponent<SpriteRenderer>();
        }

        public void MoveFoodToDes(bool isCorrectOrder)
        {
            if (dishesOnPlate == null) return;

            if (isCorrectOrder)
            {
                dishesOnPlate.transform.DOJump(desOnTable.transform.localPosition, 1f, 1, 0.7f).OnComplete(() =>
                {
                    dishesOnPlate.transform
                        .DOJump(desOnTable.transform.localPosition + new Vector3(0, 1f, 0), 1f, 1, 0.3f).SetDelay(0.3f)
                        .OnComplete(
                            () =>
                            {
                                LogicGame5.ins.PlayPositiveParticleEffect(new Vector3(1, 4, 0));
                                LogicGame5.ins.isCanPlay = true;
                                dishesOnPlate.gameObject.SetActive(false);
                                Duck.PlayParticle(smokeHideFood);
                                InitDishPrefab();
                            });
                });
            }
            else
            {
                //dishesOnPlate.gameObject.SetActive(false);
                ShowFailFoodOrdered();
               // Duck.PlayParticle(smokeHideFood);
            }
        }

        private void ShowFailFoodOrdered()
        {
            Vector2 pos = dishesOnPlate.transform.localPosition;
            Vector2 des = pos + new Vector2(0, .5f);
            for (int i = 0; i < toppingsOnPlate.Count; i++)
            {
                var a = i;
                toppingsOnPlate[a].DOColor(Color.red, 0.1f).OnComplete(() =>
                {
                    toppingsOnPlate[a].DOColor(Color.white, 0.1f);
                });
            }
            dishesOnPlate.transform.DOLocalMove(des, 0.3f).OnComplete(() =>
            {
                dishesOnPlate.gameObject.SetActive(false);
                LogicGame5.ins.isCanPlay = true;
                InitDishPrefab();
            });
        }

        public void ShakeBell()
        {
            bell.GetComponent<BoxCollider2D>().enabled = true;
            bell.GetComponent<Animator>().enabled = true;
            bell.GetComponent<Animator>().Play("Bell",0,0);
            Debug.Log("Shaked Bell");
        }

        public void DefaultBell()
        {
            bell.GetComponent<BoxCollider2D>().enabled = false;
            bell.GetComponent<Animator>().enabled = false;
        }
    }
}