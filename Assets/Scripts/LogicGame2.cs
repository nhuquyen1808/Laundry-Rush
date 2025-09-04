using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using Spine.Unity;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DevDuck
{
    [System.Serializable]
    public class ScoreDictionary
    {
        public ScoreGame2 obj;
        public float distance;
        public int score;
        public string name;
        public int scoreAdded;

        public ScoreDictionary(ScoreGame2 obj, string name, float distance, int score)
        {
            this.obj = obj;
            this.name = name;
            this.distance = distance;
            this.score = score;
            this.scoreAdded = 0;
        }
    }

    public class LogicGame2 : MonoBehaviour
    {
        public static LogicGame2 instance;
        public UiWinLose uiWinLose;
        [SerializeField] GameObject Light;
        [SerializeField] List<GameObject> posCameraAppear = new List<GameObject>();
        [SerializeField] List<GameObject> posCameraIns = new List<GameObject>();
        float timer, timeToTakePhoto;
        Camera camera;
        bool isCountDown, isEndGame;
        bool isCanAppear = true;
        [SerializeField] List<GameObject> playersCheckIn = new List<GameObject>();
        [SerializeField] List<float> distances = new List<float>();
        [SerializeField] Image boxCountDown;
        [SerializeField] TextMeshProUGUI timeToTakePhotoTxt;
        List<int> ids = new List<int> { 0, 1, 2, 3, 4 };
        [HideInInspector] List<ScoreDictionary> scoreDictionaries = new List<ScoreDictionary>();
        [SerializeField] List<Image> redDotLists = new List<Image>();
        [SerializeField] List<Image> greenDotLists = new List<Image>();
        int wave;
        [Header("Score :  ")] [SerializeField] List<ScoreGame2> scores = new List<ScoreGame2>();
        public SkeletonAnimation topSkeletonCamera, bottomSkeletonCamera, frontSkeletonCamera, skeletonToShow;
        public ParticleSystem smokeParticles;
        private Vector3 posToJump;
        [SerializeField] LogicUiGame2 uiGame2;
        [SerializeField] List<ScoreGame2> scoreGame2s = new List<ScoreGame2>();
        public bool isDoneTutorial;
        public GameObject handStart;
        public GameObject settingButton;
        private void Awake()
        {
            instance = this;
            isDoneTutorial = PlayerPrefs.GetInt("TutorialGame2", 0) == 1 ? true : false;
        }

        private void Start()
        {
            ManagerGame.TIME_SCALE = 1;

            camera = Camera.main;
            float aspectRatio = (float)Mathf.Max(Screen.width, Screen.height) / Mathf.Min(Screen.width, Screen.height);
            if (aspectRatio > 2f)
            {
                camera.orthographicSize = 13.5f;
                camera.DOOrthoSize(12f, 1.5f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    if (isDoneTutorial) return;
                    focusTutorial.transform.position =
                        camera.ScreenToWorldPoint(handStart.GetComponent<RectTransform>().position) +
                        new Vector3(0, 0, 10);
                    FocusTutorialGame2.Instance.StartCoroutine(FocusTutorialGame2.Instance.ScaleFocus(3, 15));
                });
            }
            else if (aspectRatio > 1.6f)
            {
                camera.orthographicSize = 12f;
                camera.DOOrthoSize(9.6f, 1.5f).SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        if (isDoneTutorial) return;
                        focusTutorial.transform.position =
                            camera.ScreenToWorldPoint(handStart.GetComponent<RectTransform>().position) +
                            new Vector3(0, 0, 10);
                        FocusTutorialGame2.Instance.StartCoroutine(FocusTutorialGame2.Instance.ScaleFocus(3, 15));
                    });
            }
            else
            {
                camera.orthographicSize = 12f;
                camera.DOOrthoSize(9.6f, 1.5f).SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        if (isDoneTutorial) return;
                        focusTutorial.transform.position =
                            camera.ScreenToWorldPoint(handStart.GetComponent<RectTransform>().position) +
                            new Vector3(0, 0, 10);
                        FocusTutorialGame2.Instance.StartCoroutine(FocusTutorialGame2.Instance.ScaleFocus(3, 15));
                    });
            }
            /*camera.orthographicSize = 12;
            camera.DOOrthoSize(9.6f,1.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                screenCollider.SetActive(true);
            });*/
        }

        void Update()
        {
            if (isDoneTutorial)
            {
                if (isEndGame) return;
                timer += 1 * Duck.TimeMod;
                if (timer > 3 && isCanAppear)
                {
                    isCanAppear = false;
                    SetPosCameraAppear();
                }

                TakePhoto();
            }
            else
            {
                if (!isCountDown) return;
                timeToTakePhoto -= 1 * Duck.TimeMod;
                timeToTakePhotoTxt.text = Mathf.Round(timeToTakePhoto).ToString();
                if (timeToTakePhoto <= 0)
                {
                    ManagerGame.TIME_SCALE = 0;
                    // HideBoxAndTakePhoto();
                    ManagerSpine.PlaySpineAnimation(frontSkeletonCamera, "photo", false);
                    boxCountDown.transform.DOScale(0, 0.3f).From(1).SetEase(Ease.InBack).OnComplete(() =>
                    {
                        Light.gameObject.SetActive(true);
                        Light.GetComponent<SpriteRenderer>().DOFade(1, 0.5f).From(0).OnComplete(() =>
                        {
                            Light.GetComponent<SpriteRenderer>().DOFade(0, 0.5f).OnComplete(() =>
                            {
                                ManagerGame.TIME_SCALE = 1;
                                StartCoroutine(ScaleArrow());
                            });
                        });
                        ;
                    });
                    isCountDown = false;
                }
            }
        }

        private void HideBoxAndTakePhoto()
        {
            ManagerSpine.PlaySpineAnimation(skeletonToShow, "photo", false);
            boxCountDown.transform.DOScale(0, 0.3f).From(1).SetEase(Ease.InBack).OnComplete(() =>
            {
                Light.gameObject.SetActive(true);
                Light.GetComponent<SpriteRenderer>().DOFade(1, 0.5f).From(0).OnComplete(() =>
                {
                    Light.GetComponent<SpriteRenderer>().DOFade(0, 0.5f).OnComplete(() =>
                    {
                        Light.gameObject.SetActive(false);
                        GetScore(skeletonToShow.transform.position);
                        ManagerGame.TIME_SCALE = 1;
                    });
                });
                ;
            });
        }

        private void SetPosCameraAppear()
        {
            if (ids.Count > 0)
            {
                int index = ids[Random.Range(0, ids.Count)];
                SetCameraSkeleton(index);
                skeletonToShow.transform.position = posCameraIns[index].transform.position;
                ManagerSpine.PlaySpineAnimation(skeletonToShow, "walk", true);
                skeletonToShow.transform.DOMove(posToJump, 1.5f).SetEase(Ease.Linear).SetDelay(2).OnComplete(() =>
                {
                    ManagerSpine.PlaySpineAnimation(skeletonToShow, "jump", false);
                    skeletonToShow.transform.DOMove(posCameraAppear[index].transform.position, 0.34f)
                        .SetEase(Ease.Linear).SetDelay(0.23f).OnComplete(() =>
                        {
                            ManagerSpine.PlaySpineAnimation(skeletonToShow, "idle", true);
                            timeToTakePhoto = 3;
                            timeToTakePhotoTxt.text = "3";
                            Light.transform.position = skeletonToShow.transform.position;
                            ShowLightCamera(index);
                            boxCountDown.gameObject.SetActive(true);
                            boxCountDown.transform.DOScale(1, 0.3f).From(0).SetEase(Ease.OutBack)
                                .OnComplete(() => isCountDown = true);
                            timer = 0;
                        });
                });
                skeletonToShow.transform
                    .DOMove(posCameraAppear[index].transform.position + new Vector3(0, 0.7f, 0), 0.27f).SetDelay(8.77f)
                    .OnStart((() => { ManagerSpine.PlaySpineAnimation(skeletonToShow, "end", false); })).OnComplete(
                        () =>
                        {
                            smokeParticles.transform.position = skeletonToShow.transform.position;
                            Duck.PlayParticle(smokeParticles);
                            isCanAppear = true;
                            skeletonToShow.gameObject.SetActive(false);
                        });
                //skeletonToShow.transform.DOMove(posCameraIns[index].transform.position, 1).SetDelay(6).OnComplete(() => isCanAppear = true);
                ids.Remove(index);
            }
            else
            {
                isEndGame = true;
                ManagerGame.TIME_SCALE = 0;
                /*Debug.Log("---------- End Game ?? ------------");
                int x = 0;
                string name = "";
                for (int i = 0; i < scoreDictionaries.Count; i++)
                {
                    if (x <= scoreDictionaries[i].obj.score)
                    {
                        x = scoreDictionaries[i].obj.score;
                        name = scoreDictionaries[i].obj.name.ToString();
                    }
                }

                ShowUiEndGame(name);
                Debug.Log(x + "   " + name);*/

                GetScore();
            }
        }

        public void ShowLightCamera(int id)
        {
            switch (id)
            {
                case 0:
                    Light.transform.eulerAngles = new Vector3(0, 0, 0);
                    boxCountDown.rectTransform.position =
                        camera.WorldToScreenPoint(Light.transform.position + new Vector3(0.5f, 1, 0));
                    SetLocalScale(true);

                    break;
                case 1:
                    Light.transform.eulerAngles = new Vector3(0, 0, 60);
                    boxCountDown.rectTransform.position =
                        camera.WorldToScreenPoint(Light.transform.position + new Vector3(0.5f, 1, 0));
                    SetLocalScale(true);
                    break;
                case 2:
                    Light.transform.eulerAngles = new Vector3(0, 0, -60);
                    boxCountDown.rectTransform.position =
                        camera.WorldToScreenPoint(Light.transform.position + new Vector3(0.5f, 1, 0));
                    SetLocalScale(false);

                    break;
                case 3:
                    Light.transform.eulerAngles = new Vector3(0, 0, 100);
                    boxCountDown.rectTransform.position =
                        camera.WorldToScreenPoint(Light.transform.position + new Vector3(0.5f, 1, 0));
                    SetLocalScale(true);

                    break;
                case 4:
                    Light.transform.eulerAngles = new Vector3(0, 0, -100);
                    boxCountDown.rectTransform.position =
                        camera.WorldToScreenPoint(Light.transform.position + new Vector3(0.5f, 1, 0));
                    SetLocalScale(false);
                    break;
            }
        }

        bool isDraw;

        public void GetScore(Vector3 camPos)
        {
            distances.Clear();
            scoreDictionaries.Clear();

            for (int i = 0; i < playersCheckIn.Count; i++)
            {
                distances.Add(Duck.GetDistance(camPos, playersCheckIn[i].transform.position));
                ScoreDictionary a = new ScoreDictionary(playersCheckIn[i].GetComponent<ScoreGame2>(),
                    $"{playersCheckIn[i].transform.name}",
                    Duck.GetDistance(camPos, playersCheckIn[i].transform.position), 0);
                scoreDictionaries.Add(a);
            }

            distances.Sort();
            for (int i = 0; i < distances.Count; i++)
            {
                for (int j = 0; j < scoreDictionaries.Count; j++)
                {
                    if (scoreDictionaries[j].distance == distances[i])
                    {
                        if (distances[i] < 5f)
                        {
                            scoreDictionaries[j].obj.score += 3 - i;
                            scoreDictionaries[j].scoreAdded += 3 - i;
                            for (int k = 0; k < scoreGame2s.Count; k++)
                            {
                                if (scoreGame2s[k].name == scoreDictionaries[j].obj.name)
                                {
                                    scoreGame2s[k].ShowScoreGeted(scoreDictionaries[j].scoreAdded);
                                }
                            }
                        }

                        Observer.Notify(EventAction.EVENT_GET_SCORE, isDoneTutorial);
                    }
                }
            }

            if (isDoneTutorial)
            {
                HideDotWave(wave);
                wave++;
            }
        }

        private void TakePhoto()
        {
            if (!isCountDown) return;
            timeToTakePhoto -= 1 * Duck.TimeMod;
            timeToTakePhotoTxt.text = Mathf.Round(timeToTakePhoto).ToString();
            if (timeToTakePhoto <= 0)
            {
                ManagerGame.TIME_SCALE = 0;
                HideBoxAndTakePhoto();
                isCountDown = false;
            }
        }

        private void SetLocalScale(bool isFlip)
        {
            if (isFlip)
            {
                boxCountDown.transform.localScale = new Vector3(1, 1, 1);
                timeToTakePhotoTxt.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                boxCountDown.transform.localScale = new Vector3(-1, 1, 1);
                timeToTakePhotoTxt.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        private void HideDotWave(int wave)
        {
            if (redDotLists[wave] != null)
                redDotLists[wave].DOFade(0, 0.3f).SetEase(Ease.InBack);
            if (greenDotLists[wave] != null)
                greenDotLists[wave].DOFade(1, 0.3f).SetEase(Ease.OutBack);
            // greenDotLists[wave+2].DOFade(1, 0.3f).SetEase(Ease.InBack);
            // greenDotLists[wave-1].DOFade(0, 0.3f).SetEase(Ease.InBack);
        }

        public void SetCameraSkeleton(int index)
        {
            switch (index)
            {
                case 0:
                    skeletonToShow = frontSkeletonCamera;
                    skeletonToShow.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    posToJump = new Vector3(0, 6.3f, 0);
                    break;
                case 1:
                    skeletonToShow = topSkeletonCamera;
                    skeletonToShow.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
                    posToJump = new Vector3(-5.4f, 2.74f, 0);
                    break;
                case 2:
                    skeletonToShow = topSkeletonCamera;
                    skeletonToShow.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    posToJump = new Vector3(5.4f, 2.74f, 0);

                    break;
                case 3:
                    skeletonToShow = bottomSkeletonCamera;
                    skeletonToShow.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    posToJump = new Vector3(-4.86f, -4.52f, 0);
                    break;
                case 4:
                    skeletonToShow = bottomSkeletonCamera;
                    skeletonToShow.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
                    posToJump = new Vector3(4.55f, -4f, 0);
                    break;
            }

            skeletonToShow.gameObject.SetActive(true);
        }

        public void ShowUiEndGame(string name)
        {
            if (name == "Player")
            {
                uiWinLose.ShowWin3Star();
                GetScore();
                Debug.Log("Get Score and Show there");
            }
            else
            {
                uiWinLose.ShowLosePanel();
            }
        }

        [Header("Tutorial")] public GameObject notiIcon;
        public GameObject effectTakePhotoTutorial;
        public SpriteRenderer arrowTwoSide;
        public GameObject handHintMoveHere, player;
        public FocusTutorialGame2 focusTutorial;
        public GameObject joystick;

        public IEnumerator CameraMoveTutorial()
        {
            frontSkeletonCamera.transform.position = posCameraIns[0].transform.position;
            notiIcon.gameObject.SetActive(true);
            SpawnBot();
            yield return new WaitForSeconds(1.5f);
            frontSkeletonCamera.gameObject.SetActive(true);
            ManagerSpine.PlaySpineAnimation(frontSkeletonCamera, "walk", true);

            frontSkeletonCamera.transform.DOMove(new Vector3(0, 6.3f, 0), 1.5f).SetDelay(1).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    ManagerSpine.PlaySpineAnimation(frontSkeletonCamera, "jump", false);
                    frontSkeletonCamera.transform.DOMove(new Vector3(0, 5f, 0), 0.34f)
                        .SetEase(Ease.Linear).SetDelay(0.23f).OnComplete(() =>
                        {
                            notiIcon.gameObject.SetActive(false);
                            ManagerSpine.PlaySpineAnimation(frontSkeletonCamera, "idle", true);
                            StartCoroutine(ShowHandHintMoveHere());
                        });
                });
        }

        public BotMoveMentGame2 bot1, bot2;

        public void SpawnBot()
        {
            bot1.SpawnBot();
            bot2.SpawnBot();
            //  bot1.transform.position = Vector2.MoveTowards(bot1.transform.position,new Vector3(-1, 4f, 0) , 0.1f);
            //  bot2.transform.position = Vector2.MoveTowards(bot2.transform.position,new Vector3(1, 4f, 0) , 0.1f);
            bot1.PosTutorial = new Vector3(-1, 2f, 0);
            bot1.moveOnTutorial = true;

            bot2.PosTutorial = new Vector3(1, 2.1f, 0);
            bot2.moveOnTutorial = true;
            /*bot1.MoveOnTutorial(ref bot1.PosTutorial new Vector3(-1, 4f, 0));*/
        }


        IEnumerator ShowHandHintMoveHere()
        {
            effectTakePhotoTutorial.SetActive(true);
            handHintMoveHere.SetActive(true);
            //   /
            yield return new WaitForSeconds(2f);

            Light.gameObject.SetActive(false);
            handHintMoveHere.gameObject.SetActive(false);
            effectTakePhotoTutorial.gameObject.SetActive(false);

            timeToTakePhoto = 3;
            timeToTakePhotoTxt.text = "3";
            Light.transform.position = frontSkeletonCamera.transform.position;
            Light.transform.eulerAngles = new Vector3(0, 0, 0);
            boxCountDown.rectTransform.position =
                camera.WorldToScreenPoint(Light.transform.position + new Vector3(0.5f, 1, 0));
            SetLocalScale(true);
            boxCountDown.gameObject.SetActive(true);
            boxCountDown.transform.DOScale(1, 0.3f).From(0).SetEase(Ease.OutBack)
                .OnComplete(() => isCountDown = true);
            timer = 0;
        }

        IEnumerator ScaleArrow()
        {
            Vector3 centerPos =
                new Vector3((frontSkeletonCamera.transform.position.x + player.transform.position.x) / 2,
                    (frontSkeletonCamera.transform.position.y + player.transform.position.y) / 2);
            arrowTwoSide.transform.position = centerPos;
            float dis = Vector2.Distance(frontSkeletonCamera.transform.position, player.transform.position);

            Vector2 direction = frontSkeletonCamera.transform.position - arrowTwoSide.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrowTwoSide.transform.rotation = Quaternion.Euler(0, 0, angle);
            arrowTwoSide.gameObject.SetActive(true);
            float elapsedTime = 0;
            while (elapsedTime < 3)
            {
                arrowTwoSide.size = new Vector2(Mathf.Lerp(0f, dis - 1, elapsedTime / 3), arrowTwoSide.size.y);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            GetScore(frontSkeletonCamera.transform.position);
            focusTutorial.transform.position = player.transform.position;

            focusTutorial.gameObject.SetActive(true);

            FocusTutorialGame2.Instance.StartCoroutine(FocusTutorialGame2.Instance.ScaleFocus(3, 15));

            /*yield return new WaitForSeconds(.5f);
            FocusTutorialGame2.Instance.StartCoroutine(FocusTutorialGame2.Instance.ScaleFocus(1.5f, 0));*/
            yield return new WaitForSeconds(3.5f);
            focusTutorial.gameObject.SetActive(false);
            arrowTwoSide.gameObject.SetActive(false);
            PlayerPrefs.SetInt("TutorialGame2", 1);
            isDoneTutorial = true;

            bot1.GetComponent<BotMoveMentGame2>().isSpawned = true;
            bot2.GetComponent<BotMoveMentGame2>().isSpawned = true;
            bot2.GetComponent<ScoreGame2>().scoreTextObject.transform.localPosition =
                bot2.GetComponent<ScoreGame2>().startPos;
            bot1.GetComponent<ScoreGame2>().scoreTextObject.transform.localPosition =
                bot1.GetComponent<ScoreGame2>().startPos;

            player.GetComponent<ScoreGame2>().scoreTextObject.transform.localPosition =
                player.GetComponent<ScoreGame2>().startPos;
            /*player.GetComponent<ScoreGame2>().ShowScoreGeted( player.GetComponent<ScoreGame2>().score);
            bot1.GetComponent<ScoreGame2>().ShowScoreGeted( bot1.GetComponent<ScoreGame2>().score);
            bot2.GetComponent<ScoreGame2>().ShowScoreGeted( bot2.GetComponent<ScoreGame2>().score);*/
            smokeParticles.transform.position = frontSkeletonCamera.transform.position;
            Duck.PlayParticle(smokeParticles);
            frontSkeletonCamera.gameObject.SetActive(false);

            //   set ispawn for bot ????
        }

        public bool distanceTutPlayer()
        {
            float distance = Vector2.Distance(player.transform.position, frontSkeletonCamera.transform.position);
            if (distance < 3f)
            {
                joystick.GetComponent<CanvasGroup>().alpha = 0.5f;
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<int> scoresCheck = new List<int>();

        public void GetScore()
        {
            for (int i = 0; i < scoreGame2s.Count; i++)
            {
                scoresCheck.Add(scoreGame2s[i].score);
            }

            scoresCheck.Sort();

            int playerScore = player.GetComponent<ScoreGame2>().score;
            for (int i = 0; i < scoresCheck.Count; i++)
            {
                if (playerScore == scoresCheck[i])
                {
                    Debug.Log(scoresCheck[i]);
                    Top(i);
                }
            }
        }

        public void Top(int top)
        {
            settingButton.gameObject.SetActive(false);
            if (top == 2)
            {
                uiWinLose.ShowWin3Star();
                Observer.Notify(EventAction.EVENT_GET_COINS, 50);
            }
            else if (top == 1)
            {
                uiWinLose.ShowWin2Star();
                Observer.Notify(EventAction.EVENT_GET_COINS, 30);
            }
            else
            {
                uiWinLose.ShowLosePanel();
                Observer.Notify(EventAction.EVENT_GET_COINS, 20);
            }
        }
    }
}