using System;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DevDuck
{
    public enum TYPEITEM8
    {
        RED,
        GREEN
    }

    public class LogicGame8 : MonoBehaviour
    {
        [HideInInspector] public List<Vector3> posDefaultLeft = new List<Vector3>();
        [HideInInspector] public List<Vector3> posDefaultRight = new List<Vector3>();
        public List<GameObject> standsObjLeft = new List<GameObject>();
        public List<GameObject> standsObjRight = new List<GameObject>();
        public static LogicGame8 ins;
        [SerializeField] GameObject posInsGift;
        [SerializeField] List<Sprite> giftColor = new List<Sprite>();
        [SerializeField] List<GameObject> giftObjects = new List<GameObject>();
        [SerializeField] private GameObject redBox, greenBox, tubeIns;
        [SerializeField] int redCount, greenCount;
        [SerializeField] TextMeshProUGUI redCountTxt, greenCountTxt;
        float timer;
        [SerializeField] ObjectPool objectPool;
        [SerializeField] LogicUiGame8 uiGame8;
        [SerializeField] ParticleSystem leftBubbleEffect, rightBubbleEffect;
        [SerializeField] private int countItemIns, score;

        public GameObject settingButton;

        public float totalTimer;
        int target;
        public LevelDataGame levelDataGame;

        private void Awake()
        {
            if (ins != null)
            {
                Destroy(gameObject);
            }
            else
            {
                ins = this;
            }
        }

        private void Start()
        {
            SetData();
            ManagerGame.TIME_SCALE = 1;
            objectPool.SetupPool();
            uiGame8.AnimStartGame();
            SetDefaultRotation();
            Observer.AddObserver(EventAction.EVENT_GET_GREEN, GetGreenGift);
            Observer.AddObserver(EventAction.EVENT_GET_RED, GetRedGift);
        }
        [SerializeField] int currentLevel;
        public void SetData()
        {
             currentLevel = PlayerPrefs.GetInt("CurrentLevel");
            if (currentLevel > 15)
            {
                currentLevel = 8;
                PlayerPrefs.SetInt("CurrentLevel", currentLevel);
            }
            target = levelDataGame.listLevelData[currentLevel].target;
            totalTimer = levelDataGame.listLevelData[currentLevel].timer + 4;
            uiGame8.SetTargetText(target);
        }

        private void OnDestroy()
        {
            Observer.RemoveObserver(EventAction.EVENT_GET_GREEN, GetGreenGift);
            Observer.RemoveObserver(EventAction.EVENT_GET_RED, GetRedGift);
        }

        bool isEndGame = false;

        private void Update()
        {
            timer += 1 *  Duck.TimeMod;
            totalTimer -= 1 * Duck.TimeMod;
            uiGame8.SetTimeText(totalTimer);

            if (timer > TimeToSpawn(countItemIns) && uiGame8.isCanInstantiate)
            {
                InsGift();
                timer = 0;
                if (totalTimer < 0 && !isEndGame)
                {
                    isEndGame = true;
                    uiGame8.isCanInstantiate = false;
                    CheckWin();
                }
            }
        }

        private void SetDefaultRotation()
        {
            for (int i = 0; i < standsObjLeft.Count; i++)
            {
                posDefaultLeft.Add(standsObjLeft[i].transform.eulerAngles);
            }

            for (int i = 0; i < standsObjRight.Count; i++)
            {
                posDefaultRight.Add(standsObjRight[i].transform.eulerAngles);
            }
        }


        public void Reset(bool isLeft)
        {
            if (isLeft)
            {
                for (int i = 0; i < standsObjLeft.Count; i++)
                {
                    // standsObjLeft[i].transform.rotation = Quaternion.identity;
                    standsObjLeft[i].transform.DORotate(posDefaultLeft[i], 0.3f);
                }
            }
            else
            {
                for (int i = 0; i < standsObjRight.Count; i++)
                {
                    // standsObjRight[i].transform.rotation = Quaternion.identity;
                    standsObjRight[i].transform.DORotate(posDefaultRight[i], 0.3f);
                }
            }
        }

        public void RotateLeft()
        {
            foreach (GameObject obj in standsObjLeft)
            {
                if (obj.transform.eulerAngles.z >= 330 || obj.transform.eulerAngles.z == 0)
                {
                    obj.transform.Rotate(Vector3.back * 80 * Time.deltaTime);
                }
            }
        }

        public void RotateRight()
        {
            foreach (GameObject obj in standsObjRight)
            {
                if (obj.transform.eulerAngles.z <= 30)
                {
                    obj.transform.Rotate(Vector3.forward, 80 * Time.deltaTime);
                }
            }
        }

        public void InsGift()
        {
            AudioManager.instance.PlaySound("Bubble");
            int id = Random.Range(0, giftColor.Count);
            PooledObject o = objectPool.GetPooledObject(posInsGift.transform.position, Quaternion.identity);
            o.GetComponent<SpriteRenderer>().sprite = giftColor[id];
            if (id == 0)
            {
                o.GetComponent<GiftLevel6>().type = TYPEITEM8.RED;
            }
            else
            {
                o.GetComponent<GiftLevel6>().type = TYPEITEM8.GREEN;
            }

            o.transform.SetParent(this.transform);
            o.transform.localScale = new Vector3(0.7F, 0.7F, 0.7F);
        }

        private void GetRedGift(object obj)
        {
            int a = (int)obj;
            redCount += a;
            redCountTxt.text = "red : " + redCount.ToString();
            Duck.PlayParticle(leftBubbleEffect);
            ShakeBox(redBox);
            AudioManager.instance.PlaySound("GetScore");

        }

        private void GetGreenGift(object obj)
        {
            int a = (int)obj;
            greenCount += a;
            greenCountTxt.text = "green : " + greenCount.ToString();
            Duck.PlayParticle(rightBubbleEffect);
            ShakeBox(greenBox);
            AudioManager.instance.PlaySound("GetScore");

        }

        private void ShakeBox(GameObject o)
        {
            o.transform.DOKill();
            o.transform.DOScale(new Vector3(1.05f, 0.95f, 0), 0.2f);
            o.transform.DOMove(o.transform.position + new Vector3(0, 0.6f, 0), 0.3f).OnComplete(() =>
            {
                o.transform.DOMove(o.transform.position + new Vector3(0, -0.6f, 0), 0.2f);
            });
            o.transform.DOScale(new Vector3(1f, 1f, 0), 0.2f);
            uiGame8.SetScoreText(redCount + greenCount);
        }

        private void AnimTubeIns()
        {
        }

        public float TimeToSpawn(int count)
        {
            switch (count)
            {
                case 1:
                    return 4.5f;
                case 2:
                case 5:
                case 6:
                case 7:
                case 8:
                case 12:
                    return 3;
                case 3:
                    return 4;
                case 4:
                    return 3.5f;
                case 9:
                case 10:
                    return 2.5f;
                default:
                    return 2f;
            }
        }

        public void CheckWin()
        {
            settingButton.SetActive(false);
            score = greenCount + redCount;
            if (score < target)
            {
                Debug.Log("Lose");
                PopupLose.instance.Show();
                AudioManager.instance.PlaySound("Lose");

            }
            else
            {
                Debug.Log("WIn");
                PopupWin.ins.Show();
                AudioManager.instance.PlaySound("Win");

            }
        }
    }
}