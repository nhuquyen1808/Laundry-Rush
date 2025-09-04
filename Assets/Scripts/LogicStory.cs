using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DevDuck
{
    public class LogicStory : MonoBehaviour
    {
        private Tween currentTween;
        List<Action> actions = new List<Action>();
        public GameObject model, npc,modelInside;
        int step = 0;
        public float timer = 0;
        public TextRunning textRunning;
        public Image imageContent;
        public Sprite leftBoxContent, rightBoxContent;
        public GameObject coin, clawMachine;
        public Button skipButton;

        private void Awake()
        {
            skipButton.onClick.AddListener(OnClickSkipButton);
        }

        private void OnClickSkipButton()
        {
            skipButton.interactable = false;
            step = actions.Count - 1;
            npc.transform.position =  new Vector3(2.65f, -4.6f, 0);
            Step6();
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
                Step6
            };

            Step1();
            timer = 2;
        }

        private void Update()
        {
            if (timer >= 0)
            {
                timer -=   Duck.TimeMod;
                if (timer <= 0)
                {
                    ++step;
                    if (step < actions.Count)
                    {
                        actions[step]();
                    }
                }
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
            StoryPerforment.ModelMovement(modelInside);
            currentTween = model.transform.DOLocalMove(new Vector3(-1.5f, -9f, 0), 2f).OnComplete(() =>
            {
                StoryPerforment.ImageContentSetup(imageContent, true, leftBoxContent);
                StoryPerforment.DefaultRotatationModel(modelInside);
                currentTween = null;
            });
        }

        public void Step2()
        {
            coin.gameObject.SetActive(true);
            currentTween = coin.transform.DOJump(clawMachine.transform.position, 1.5f, 1, 0.3f, false)
                .SetDelay(1f).OnComplete((() =>
                {
                    coin.gameObject.SetActive(false);
                    StoryPerforment.ModelMovement(npc);
                    currentTween = npc.transform.DOLocalMove(model.transform.position + new Vector3(1, 4.35f, 0), 1f)
                        .OnComplete(() =>
                        {
                            StoryPerforment.DefaultRotatationModel(npc);
                            model.transform.DOMove(model.transform.position + new Vector3(-1.5f, 0, 0), .5f);
                            currentTween = npc.transform.DOMove(npc.transform.position + new Vector3(0.65f, 0, 0), 0.5f).SetEase(Ease.OutBack);
                            textRunning.Txt.text = "";
                            timer = 2;
                           // currentTween = null;
                        });
                }));
        }

        public void Step3()
        {
            string a = "Hey! But I already put my coin in...";
            StoryPerforment.ImageContentSetup(imageContent, true, leftBoxContent);
            currentTween = imageContent.transform.DOScale(0, 0.3f).OnComplete((() =>
            {
                currentTween = imageContent.transform.DOScale(1, 0.3f).From(0).OnComplete((() =>
                {
                    StoryPerforment.ModelTalking(npc);
                    timer = textRunning.RunText(a) + 2;
                }));
            }));
        }

        public void Step4()
        {
            string a = "Hah! Do you really think you can win against me? Go ahead and try!";
            textRunning.Txt.text = "";
            currentTween = npc.transform.DOMoveX(3, 0.5f);
            StoryPerforment.ImageContentSetup(imageContent, false, rightBoxContent);
            currentTween = imageContent.transform.DOScale(0, 0.3f).OnComplete((() =>
            {
                currentTween = imageContent.transform.DOScale(1, 0.3f).From(0).OnComplete(() =>
                {
                    StoryPerforment.ModelTalking(npc);
                    timer = textRunning.RunText(a) + 2;
                    currentTween = null;
                });
            }));
        }

        public void Step5()
        {
            string content = "I will defeat you.";
            textRunning.Txt.text = "";
            currentTween =   model.transform.DORotate(new Vector3(0, 0, 0), 1f);
            StoryPerforment.ImageContentSetup(imageContent, true, leftBoxContent);
            currentTween = imageContent.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                currentTween = imageContent.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    StoryPerforment.ModelTalking(modelInside);
                    timer = textRunning.RunText(content) + 2;
                    currentTween = null;
                });
            });
        }

        public void Step6()
        {
            currentTween = imageContent.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                DOTween.KillAll();
                ManagerSceneDuck.ins.LoadScene(GlobalData.currentSceneMiniGame);
            });
        }
    }
}