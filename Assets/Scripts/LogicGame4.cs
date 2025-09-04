using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DevDuck
{
    public class LogicGame4 : MonoBehaviour
    {
        public Vector2 areaSize = new Vector2(10, 10);
        private Vector2 targetPosition, insPosition;
        [SerializeField] ObjectPool pool;
        float timeSpawnObj1, timeSpawnObj2;
        public Sprite item1, item2, item3;
        [SerializeField] List<ItemGame4> shadowIns = new List<ItemGame4>();
        public List<ItemGame4> itemsIns = new List<ItemGame4>();
        [SerializeField] int countShadow, countItems;
        public static LogicGame4 instance;
        public bool isTransitionWave, isShowCountDown;
        [SerializeField] LogicUiGame4 UiGame4;
        [SerializeField] PoolSmokeHideGame4 poolSmokeHideGame4;
        bool isWave2, isWave3,isEndGame;
        private int wave = 1;
        [SerializeField] int playerScore,yellowBotScore,redBotScore;
        public  List<int> scores = new List<int>();

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            pool.SetupPool();

            isShowCountDown = true;
            isTransitionWave = true;
            
            Observer.AddObserver(EventAction.EVENT_BOT_RED_GET_SCORE,RedBotGetScore);
            Observer.AddObserver(EventAction.EVENT_BOT_YELLOW_GET_SCORE, YellowBotGetScore);
            Observer.AddObserver(EventAction.EVENT_GET_SCORE, PlayerGetScore);
      
        }

        private void PlayerGetScore(object obj)
        {
            int a = (int)obj;
            playerScore += a;
        }

        private void YellowBotGetScore(object obj)
        {
            int a = (int)obj;
            yellowBotScore += a;
        }

        private void RedBotGetScore(object obj)
        {
            int a = (int)obj;
            redBotScore += a;
        }

        private void Update()
        {
            if(isEndGame) return;   
            if (isTransitionWave)
            {
                if (isShowCountDown)
                {
                    isShowCountDown = false;
                    UiGame4.SetCountDownText();
                    ManagerGame.TIME_SCALE = 0;
                }
            }
            else
            {
                SpawnItems();
            }
        }

        private void SpawnItems()
        {
            timeSpawnObj1 += 1 * Duck.TimeMod;
            timeSpawnObj2 += 1 * Duck.TimeMod;
            if (timeSpawnObj1 >= 2)
            {
                timeSpawnObj1 = 0;
                SetRandomTargetPosition();
            }

            if (timeSpawnObj2 >= 3)
            {
                timeSpawnObj2 = 0;
                SetRandomTargetPosition();
            }
        }

        void SetRandomTargetPosition()
        {
            float randomX = Random.Range(-areaSize.x / 2, areaSize.x / 2);
            float randomY = Random.Range( /*-areaSize.y / 2*/ -2, areaSize.y / 2);
            targetPosition = new Vector2(randomX, randomY);
            insPosition = new Vector2(randomX, randomY + 15);
            if (isTransitionWave) return;
            SetWave();
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Vector2.zero, areaSize);
        }

        private void InsObject(Vector2 pos, Sprite spr, bool isShadow, TypeItem type)
        {
            var o = pool.GetPooledObject(pos, Quaternion.identity);
            o.GetComponent<SpriteRenderer>().sprite = spr;
            o.GetComponent<ItemGame4>().des = new Vector2(pos.x, pos.y - 15);

            if (isShadow)
            {
                countShadow += 1;
                shadowIns.Add(o.GetComponent<ItemGame4>());
                o.GetComponent<ItemGame4>().id = countShadow;
                o.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 210);
                o.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                o.GetComponent<CircleCollider2D>().enabled = false;
                o.GetComponent<ItemGame4>().isItem = false;
                o.GetComponent<SpriteRenderer>().sortingOrder = -1;
                o.GetComponent<ItemGame4>().TYPE = type;
            }
            else
            {
                countItems++;
                itemsIns.Add(o.GetComponent<ItemGame4>());
                o.GetComponent<ItemGame4>().id = countItems;
                o.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
                o.GetComponent<SpriteRenderer>().sortingOrder = 3;
                o.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                o.GetComponent<CircleCollider2D>().enabled = true;
                o.GetComponent<ItemGame4>().isItem = true;
                o.GetComponent<ItemGame4>().TYPE = type;
            }
        }

        public void HideShadow(int id)
        {
            for (int i = 0; i < shadowIns.Count; i++)
            {
                if (shadowIns[i].id == id)
                {
                    shadowIns[i].GetComponent<PooledObject>().ReturnToPool();
                    PooledObject o = poolSmokeHideGame4.GetPooledObject(shadowIns[i].transform.position,
                        Quaternion.Euler(90, 0, 0));
                    o.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    Duck.PlayParticle(o.GetComponent<ParticleSystem>());
                    DOVirtual.DelayedCall(1, o.ReturnToPool);
                    shadowIns.Remove(shadowIns[i]);
                }
            }
        }

        public void GetPoint(int id)
        {
            for (int i = 0; i < shadowIns.Count; i++)
            {
                if (shadowIns[i].id == id)
                {
                    shadowIns[i].GetComponent<PooledObject>().ReturnToPool();
                    shadowIns.Remove(shadowIns[i]);
                }
            }
        }

        public void SetWave()
        {
            if (countItems < 15)
            {
                InsObject(targetPosition, item1, true, TypeItem.SHRIMP);
                InsObject(insPosition, item1, false, TypeItem.SHRIMP);
            }
            else if (countItems <= 30)
            {
                if (!isWave2)
                {
                    isWave2 = true;
                    isTransitionWave = true;
                    isShowCountDown = true;
                    wave++;
                    UiGame4.ShowWaveText(wave);
                    DOVirtual.DelayedCall(2, ClearObjectToNextWave);
                    // ClearObjectToNextWave();
                }
                else
                {
                    InsObject(targetPosition, item2, true, TypeItem.SALMON);
                    InsObject(insPosition, item2, false, TypeItem.SALMON);
                }
            }
            else if (countItems <= 45)
            {
                if (!isWave3)
                {
                    isWave3 = true;
                    isTransitionWave = true;
                    isShowCountDown = true;
                    wave++;
                    UiGame4.ShowWaveText(wave);
                    DOVirtual.DelayedCall(2, ClearObjectToNextWave);
                    //  ClearObjectToNextWave();
                }
                else
                {
                    InsObject(targetPosition, item3, true, TypeItem.BENTO);
                    InsObject(insPosition, item3, false, TypeItem.BENTO);
                }
            }
            else
            {
                isEndGame = true;
                Debug.Log("Check WIN lose here");
                ManagerGame.TIME_SCALE = 0;
                CheckWhoIsWinner();
            }
        }

        private void ClearObjectToNextWave()
        {
            for (int i = 0; i < itemsIns.Count; i++)
            {
                itemsIns[i].GetComponent<PooledObject>().ReturnToPool();
            }
            itemsIns.Clear();
            for (int i = 0; i < shadowIns.Count; i++)
            {
                shadowIns[i].GetComponent<PooledObject>().ReturnToPool();
            }
            shadowIns.Clear();
            /*wave++;
            UiGame4.ShowWaveText(wave);*/
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<Tag>().tagName == "ItemGame4")
            {
                playerScore++;
            }
        }

            int scoreMax ;
        private void CheckWhoIsWinner()
        {
            scores.Add(playerScore);
            scores.Add(yellowBotScore);
            scores.Add(redBotScore);
            scoreMax = scores.Max();
            Debug.Log(scoreMax);
            if (scoreMax == playerScore)
            {
                Debug.Log("You Win!");
                UiGame4.ShowWinPanel();
            }
            else if (scoreMax == yellowBotScore)
            {
                Debug.Log("You Lose! Yellow Win");
                UiGame4.ShowLosePanel();

            }
            else if (scoreMax == redBotScore)
            {
                Debug.Log("You Lose! Red Win");
                UiGame4.ShowLosePanel();

            }
            
        }
    }
}