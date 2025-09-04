using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DevDuck
{
    public class LogicStory4 : MonoBehaviour
    {
        private Tween currentTween;
        List<Action> actions = new List<Action>();
        public GameObject model;
        public GameObject npc1;
        public GameObject modelInside, tube, poison;
        int step = 0;
        public float timer = 0;
        public TextRunning textRunning;
        public Image imageContent;
        public Sprite leftBoxContent, rightBoxContent;      
        public ParticleSystem poisonParticle;
        public Button skipButton;
        private void Awake()
        {
            skipButton.onClick.AddListener(OnClickSkipButton);
        }

        private void OnClickSkipButton()
        {
            skipButton.interactable = false;
            step = actions.Count - 1;
            npc1.transform.localPosition = new Vector3(0, 1, 0);
            npc1.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            Step8();
        }

        private void Start()
        {
            ManagerGame.TIME_SCALE = 1;
            modelInside.GetComponent<Model>().LoadAndInitFashion();
            actions = new List<Action>()
            {
                Step1,
                Step2,
                Step3,
                Step4,
                Step5,
                 Step6,
                 Step7,
                 Step8,
            };

            Step1();
            timer = 2;
        }

        private void Update()
        {
            if (timer >= 0)
            {
                timer -=  Duck.TimeMod;;
                if (timer <= 0)
                {
                    ++step;
                    if (step < actions.Count)
                    {
                        actions[step]();
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                Scene sceneManager = SceneManager.GetActiveScene();
                SceneManager.LoadScene(sceneManager.name);
            }


            if (Input.GetMouseButtonDown(0))
            {
                if (currentTween != null)
                {
                    currentTween.Kill(true);
                }
                else if (textRunning.timer > 0)
                {
                    textRunning.timer = 10000;
                }
                else
                {
                    timer = 0;
                }
            }
        }

        public void Step1()
        {
            //string a = "this is text running step 1";
            currentTween = npc1.transform.DOLocalMove(new Vector3(0f, 7.4f, 0), 0.5f).OnComplete(() =>
            {
                StoryPerforment.ModelMovement(npc1);
                currentTween = npc1.transform.DOLocalMove(new Vector3(0f, 1f, 0), 1f).OnStart((() =>
                {
                    npc1.transform.DOScale(0.8f, 1);
                })).OnComplete(() =>
                {
                    StoryPerforment.DefaultRotatationModel(npc1);
                    timer = 2;
                    currentTween = null;
                });
            });
        }

        public void Step2()
        {
            string textContent = "Did you make this pastry?";
            imageContent.sprite = leftBoxContent;
            imageContent.rectTransform.anchoredPosition = new Vector2(50, 350);
            imageContent.rectTransform.pivot = new Vector2(0, 0);
            currentTween = imageContent.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).SetDelay(0.5f).OnComplete(() =>
            {
                StoryPerforment.ModelTalking(npc1);
                timer = textRunning.RunText(textContent) + 2;
                currentTween = null;
            });
        }

        public void Step3()
        {
            textRunning.Txt.text = "";
            string textContent = "Yes, want to try one? ";
            currentTween = imageContent.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                StoryPerforment.ModelTalking(modelInside);
                StoryPerforment.ImageContentSetup(imageContent, false, rightBoxContent);
                currentTween = imageContent.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    timer = textRunning.RunText(textContent) + 2;
                    currentTween = null;
                });
            });
        }

        public void Step4()
        {
            textRunning.Txt.text = "";
            string textContent = "Wait… this one looks weird. Is Lisa selling spoiled food? ";
            currentTween = imageContent.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                StoryPerforment.ModelTalking(npc1);
                imageContent.sprite = leftBoxContent;
                imageContent.rectTransform.anchoredPosition = new Vector2(50, 350);
                imageContent.rectTransform.pivot = new Vector2(0, 0);
                tube.gameObject.SetActive(true);
                tube.GetComponent<Animator>().Play("tubeStory4");
                Duck.PlayParticle(poisonParticle);

                currentTween = imageContent.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    StoryPerforment.DefaultRotatationModel(npc1);
                    timer = textRunning.RunText(textContent) + 3;
                    currentTween = null;
                });
            });
        }

        public void Step5()
        {
            textRunning.Txt.text = "";
            string textContent = "No way, they look normal!";
            currentTween = imageContent.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                StoryPerforment.ModelTalking(modelInside);
                StoryPerforment.ImageContentSetup(imageContent, false, rightBoxContent);
                currentTween = imageContent.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).SetDelay(2).OnComplete(
                    () =>
                    {
                        timer = textRunning.RunText(textContent) + 2;
                        currentTween = null;
                    });
            });
        }

        public void Step6()
        {
            string textContent = "Did you make this pastry?";
            imageContent.sprite = leftBoxContent;
            imageContent.rectTransform.anchoredPosition = new Vector2(50, 350);
            imageContent.rectTransform.pivot = new Vector2(0, 0);
            currentTween = imageContent.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                currentTween = imageContent.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    StoryPerforment.ModelTalking(npc1);
                    timer = textRunning.RunText(textContent) + 2;
                    currentTween = null;
                });
            });
        }

        void Step7()
        {
            textRunning.Txt.text = "";
            string textContent =
                "Fine! I’ll throw away the pastry you ruined and sell to even more customers than you!";
            currentTween = imageContent.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                StoryPerforment.ModelTalking(modelInside);
                StoryPerforment.ImageContentSetup(imageContent, false, rightBoxContent);
                currentTween = imageContent.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).SetDelay(2).OnComplete(
                    () =>
                    {
                        timer = textRunning.RunText(textContent) + 2;
                        currentTween = null;
                    });
            });
        }

        void Step8()
        {
            currentTween = imageContent.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                currentTween = null;
                DOTween.KillAll();
                ManagerSceneDuck.ins.LoadScene(GlobalData.currentSceneMiniGame);
            });
        }
    }
}