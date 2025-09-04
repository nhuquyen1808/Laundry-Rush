using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DevDuck
{
    public class KnifeHitMiniGame : MonoBehaviour
    {
        public Vector3 insPlayerPos = new Vector3(0, -6, 0);
        public float timer;
        public Camera cam;
        public bool isEmpty = true;
        public static KnifeHitMiniGame instance;
        [SerializeField] ObjectPool pool;
        public GameObject target;
        [Header("bot")] [SerializeField] GameObject botObj;
        public Vector3 insBotPos = new Vector3(0, 7, 0);
        [SerializeField] float timerBot;
        RaycastHit2D hit;
        [SerializeField] int countBotUsed, countPlayerUsed;
        [SerializeField] ManagerUiGameKnife managerUiGameKnife;
        [Header("Score : ")] public int botScore, playerScore;
        [Header("Wave : ")] [SerializeField] int wave = 1;
        [SerializeField] Sprite wave1TargetSpr, wave2TargetSpr, wave3TargetSpr, itemWave2Spr, itemWave3Spr;
        [SerializeField] bool isCanPlay = true;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(instance);
            }
            else
            {
                instance = this;
            }
        }

        private void Start()
        {
            StartCoroutine(DelayIns());
        }

        IEnumerator DelayIns()
        {
            yield return new WaitForSeconds(0.5f);
            InsLipStickPref();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && isCanPlay && countPlayerUsed < 7)
            {
                Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.forward, Mathf.Infinity);
                if (hit.collider == null) return;
                if (hit.collider.GetComponent<LipStick>() != null)
                {
                    LipStick o = hit.collider.GetComponent<LipStick>();
                    o.HitLipStick();
                    InsLipStickPref();
                    countPlayerUsed++;
                    managerUiGameKnife.DisableItemUsed(countPlayerUsed, false);
                    ChangeTargetAndItem();
                }
            }

            BotDetectSpace();
        }

        int count;

        void InsLipStickPref()
        {
            if (count < 7)
            {
                SetObjectPlayer(insPlayerPos);
                count++;
                timer = 0;
            }
        }

        private LipStick SetObjectPlayer(Vector3 pos)
        {
            PooledObject lip = pool.GetPooledObject(pos, Quaternion.identity);
            lip.transform.position = pos;
            lip.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).SetDelay(0.3f).From(0);
            lip.GetComponent<Rigidbody2D>().isKinematic = true;
            lip.GetComponent<CapsuleCollider2D>().isTrigger = false;
            lip.GetComponent<LipStick>().id = -1;
            
            lip.transform.name = $"player{countBotUsed}";
            if (wave == 2)
            {
                lip.GetComponent<SpriteRenderer>().sprite = itemWave2Spr;
            }
            else if (wave == 3)
            {
                lip.GetComponent<SpriteRenderer>().sprite = itemWave3Spr;
            }
            return lip.GetComponent<LipStick>();
        }
        //   --------  BOT  ---------
        private void BotDetectSpace()
        {
            timerBot += 1 * Duck.TimeMod;
            if (timerBot > 1.5f)
            {
                hit = Physics2D.Raycast(botObj.transform.position,
                    (target.transform.position - botObj.transform.position), Mathf.Infinity);
                if (hit.collider != null && isCanPlay)
                {
                    Tag tag = hit.collider.gameObject.GetComponent<Tag>();
                    if (tag == null) return;
                    if (tag.tagName == "Target" && countBotUsed < 7)
                    {
                        LipStick lip = SetObjectBot(insBotPos);
                        lip.transform.eulerAngles = new Vector3(0, 0, 180);
                        countBotUsed++;
                        managerUiGameKnife.DisableItemUsed(countBotUsed, true);
                        ChangeTargetAndItem();
                        lip.transform.DOScale(1, 0.3f).From(0).OnComplete(() => lip.HitLipStick());
                        timerBot = 0;
                    }
                    else
                    {
                        timerBot = 1;
                    }
                }
            }
        }

        private LipStick SetObjectBot(Vector3 pos)
        {
            PooledObject lip = pool.GetPooledObject(pos, Quaternion.identity);
            lip.transform.position = pos;
            lip.transform.name = $"bot{countBotUsed}";
            lip.GetComponent<Rigidbody2D>().isKinematic = true;
            lip.GetComponent<CapsuleCollider2D>().isTrigger = false;
            lip.GetComponent<LipStick>().id = -2;
            if (wave == 2)
            {
                lip.GetComponent<SpriteRenderer>().sprite = itemWave2Spr;
            }
            else if (wave == 3)
            {
                lip.GetComponent<SpriteRenderer>().sprite = itemWave3Spr;
            }

            return lip.GetComponent<LipStick>();
        }

        private void OnDrawGizmos()
        {
            Debug.DrawRay(botObj.transform.position, (target.transform.position - botObj.transform.position),
                Color.red);
        }

        void ChangeTargetAndItem()
        {
            if (countPlayerUsed >= 7 && countBotUsed >= 7)
            {
                count = 0;
                isCanPlay = false;
                countPlayerUsed = countBotUsed = 0;
                Vector3 targetPos = target.transform.position;
                wave++;
                Debug.Log("Change Target" + wave);
                if (wave <= 3)
                {
                    MoveTarget(targetPos);
                }
                else
                {
                    ShowEndGame();
                }
            }
        }

        private void MoveTarget(Vector3 targetPos)
        {
            managerUiGameKnife.DisableWaveIcon(wave);
            target.transform.DOMoveX(targetPos.x + 15, 1).SetDelay(1).SetEase(Ease.InBack).OnComplete(() =>
            {
                RemoveItemOnTarGet(wave);
                target.transform.DOMoveX(targetPos.x, 0.7f).SetEase(Ease.OutBack).SetDelay(.5f)
                    .OnComplete(() =>
                    {
                        isCanPlay = true;
                        InsLipStickPref();
                    });
            });
        }

        void RemoveItemOnTarGet(int waveCount)
        {
            managerUiGameKnife.Reset(waveCount);
            switch (waveCount)
            {
                case 2:
                    target.GetComponent<SpriteRenderer>().sprite = wave2TargetSpr;
                    break;
                case 3:
                    target.GetComponent<SpriteRenderer>().sprite = wave3TargetSpr;
                    break;
            }

            for (int i = 0; i < target.transform.childCount; i++)
            {
                Destroy(target.transform.GetChild(i).gameObject);
                /*  target.transform.GetChild(i).GetComponent<PooledObject>().ReturnToPool();
                  target.transform.GetChild(i).transform.SetParent(null);*/
            }
        }

        void ShowEndGame()
        {
            if (playerScore > botScore)
            {
                Debug.Log("Win");
                managerUiGameKnife.ShowWinPanel();
            }
            else
            {
                Debug.Log("Lose");
                managerUiGameKnife.ShowLosePanel();
            }
        }
    }
}