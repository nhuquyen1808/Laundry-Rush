using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DevDuck
{
    public class LogicStory5 : MonoBehaviour
    {
        private Tween currentTween;
        List<Action> actions = new List<Action>();
        public GameObject model;
        public GameObject lisa, john;
        public GameObject modelInside;
        int step = 0;
        public float timer = 0;
        public TextRunning textRunning;
        public Image imageContent;
        public Sprite leftBoxContent, rightBoxContent;
        public GameObject playerCoffee, lisaCoffee;

        public Button skipButton;

        private void Awake()
        {
            skipButton.onClick.AddListener(OnClickSkipButton);
        }

        private void OnClickSkipButton()
        {
            skipButton.interactable = false;
            step = actions.Count - 1;
            lisa.transform.localPosition = new Vector3(-2.5f, -4.5f, 0);
            john.transform.localPosition = new Vector3(2.5f, -4.5f, 0);
            model.transform.localPosition = new Vector3(0f, -9f, 0);
            Step10();
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
                Step9,
                Step10,
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
            StoryPerforment.ModelMovement(john);
            currentTween = john.transform.DOLocalMoveX(2, 2f).OnComplete(() =>
            {
                //  StoryPerforment.ModelMovement(modelInside);
                StoryPerforment.DefaultRotatationModel(john);
            });
        }

        public void Step2()
        {
            string textContent = "You seem different today. Hiding something?";
            imageContent.sprite = leftBoxContent;
            imageContent.rectTransform.anchoredPosition = new Vector2(-20, 60);
            currentTween = model.transform.DOLocalMoveX(0f, 1f).OnComplete(() =>
            {
                StoryPerforment.DefaultRotatationModel(modelInside);
                currentTween = imageContent.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    StoryPerforment.ModelTalking(modelInside);
                    timer = textRunning.RunText(textContent) + 2;
                    currentTween = null;
                });
            });
        }

        public void Step3()
        {
            textRunning.Txt.text = "";
            string textContent = "I just want to be with you. That’s all";
            currentTween = imageContent.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                StoryPerforment.ImageContentSetup(imageContent, false, rightBoxContent);
                currentTween = imageContent.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    StoryPerforment.ModelTalking(john);
                    timer = textRunning.RunText(textContent) + 2;
                    currentTween = null;
                });
            });
        }

        public void Step4()
        {
            textRunning.Txt.text = "";
            string textContent = "Nothing else? Then what about me, John?";
            StoryPerforment.ModelMovement(lisa);
            currentTween = imageContent.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                StoryPerforment.ImageContentSetup(imageContent, true, leftBoxContent);
                currentTween = lisa.transform.DOLocalMoveX(-3, 2f).OnComplete(() =>
                {
                    currentTween = imageContent.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).OnComplete((() =>
                    {
                        StoryPerforment.DefaultRotatationModel(lisa);
                        timer = textRunning.RunText(textContent) + 2;
                        currentTween = null;
                    }));
                });
            });
        }

        void Step5()
        {
            string textContent = "Jess....?";
            textRunning.Txt.text = "";

            imageContent.sprite = rightBoxContent;
            imageContent.rectTransform.anchoredPosition = new Vector2(-100, 60);
            imageContent.rectTransform.pivot = new Vector2(1, 0);
            StoryPerforment.DefaultRotatationModel(modelInside);
            currentTween = imageContent.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                // StoryPerforment.ImageContentSetup(imageContent, false, rightBoxContent);
                currentTween = playerCoffee.transform
                    .DOJump(playerCoffee.transform.position + new Vector3(0, -5), 2, 1, .5f, false).OnComplete((
                        () =>
                        {
                            playerCoffee.gameObject.SetActive(false);
                            currentTween = imageContent.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).OnComplete(
                                () =>
                                {
                                    StoryPerforment.ModelTalking(modelInside);
                                    timer = textRunning.RunText(textContent) + 2;
                                    currentTween = null;
                                });
                        }));
            });
        }

        void Step6()
        {
            textRunning.Txt.text = "";
            string textContent = "Didn’t she know? He’s been dating me for the past three months.";
            currentTween = imageContent.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                StoryPerforment.ImageContentSetup(imageContent, true, leftBoxContent);
                StoryPerforment.ModelTalking(lisa);
                currentTween = lisaCoffee.transform
                    .DOJump(john.transform.position + new Vector3(-1, 1), 2, 1, .5f, false).OnComplete((() =>
                    {
                        lisaCoffee.gameObject.SetActive(false);
                        currentTween = imageContent.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
                        {
                            StoryPerforment.DefaultRotatationModel(lisa);
                            timer = textRunning.RunText(textContent) + 2;
                            currentTween = null;
                        });
                    }));
            });
        }


        void Step7()
        {
            textRunning.Txt.text = "";
            string textContent = "Let me explain";
            StoryPerforment.ImageContentSetup(imageContent, false, rightBoxContent);
            currentTween = imageContent.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                currentTween = imageContent.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    StoryPerforment.ModelTalking(john);
                    timer = textRunning.RunText(textContent) + 2;
                    currentTween = null;
                });
            });
        }

        void Step8()
        {
            textRunning.Txt.text = "";
            string textContent = "Hey Lisa! Let’s play a game — whoever wins gets to slap him.";
            StoryPerforment.ModelMovement(lisa);
            currentTween = imageContent.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                StoryPerforment.ImageContentSetup(imageContent, true, leftBoxContent);
                currentTween = imageContent.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    StoryPerforment.DefaultRotatationModel(lisa);
                    timer = textRunning.RunText(textContent) + 2;
                    currentTween = null;
                });
            });
        }

        void Step9()
        {
            textRunning.Txt.text = "";
            string textContent = "Okay. I will win!";
            imageContent.sprite = leftBoxContent;
            imageContent.rectTransform.pivot = new Vector2(0f, 0f);

            imageContent.rectTransform.anchoredPosition = new Vector2(-20, 60);
            StoryPerforment.DefaultRotatationModel(modelInside);

            currentTween = imageContent.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                currentTween = imageContent.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    StoryPerforment.ModelTalking(modelInside);
                    timer = textRunning.RunText(textContent) + 2;
                    currentTween = null;
                });
            });
        }

        void Step10()
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