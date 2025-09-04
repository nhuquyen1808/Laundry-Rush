using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DevDuck
{
    public class LogicStory2 : MonoBehaviour
    {
        private Tween currentTween;
        List<Action> actions = new List<Action>();
        public GameObject model, npc, modelInside;
        int step = 0;
        public float timer = 0;
        public TextRunning textRunning;
        public Image imageContent;
        public Sprite leftBoxContent, rightBoxContent;
        public GameObject camera;
        public GameObject tripod;
        public ParticleSystem smokeParticle;
        public Image flashImage;

        public Button skipButton;

        private void Awake()
        {
            skipButton.onClick.AddListener(OnClickSkipButton);
        }

        private void OnClickSkipButton()
        {
            skipButton.interactable = false;
            step = actions.Count - 1;
            model.transform.localPosition = new Vector3(-1f, -9f, 0);
            npc.transform.localPosition = new Vector3(2.5f, -4.5f, 0);
            camera.transform.position = tripod.transform.position;
            Step9();
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
            };

            Step1();
            timer = 2;
        }

        private void Update()
        {
            if (timer >= 0)
            {
                timer -= Duck.TimeMod;
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
            currentTween = model.transform.DOLocalMove(new Vector3(-2.5f, -8f, 0), 2f).OnComplete(() =>
            {
                StoryPerforment.ImageContentSetup(imageContent, true, leftBoxContent);
                StoryPerforment.DefaultRotatationModel(modelInside);
                timer = 2;
                currentTween = null;
            });
        }

        public void Step2()
        {
            camera.gameObject.SetActive(true);
            textRunning.Txt.text = "3";
            currentTween = camera.transform.DOJump(tripod.transform.position, 1.5f, 1, 0.5f, false)
                .OnComplete(() =>
                {
                    camera.transform.SetParent(tripod.transform);
                    camera.GetComponent<SpriteRenderer>().sortingOrder = -5;
                    
                    timer = 2;
                    Duck.PlayParticle(smokeParticle);
                    currentTween = null;
                });
        }

        void Step3()
        {
            textRunning.Txt.text = "3";
            StoryPerforment.ImageContentSetup(imageContent, true, leftBoxContent);
            currentTween = imageContent.transform.DOScale(1, 0.5f).OnComplete(() =>
            {
                StoryPerforment.ImageContentSetup(imageContent, true, leftBoxContent);
                currentTween = imageContent.transform.DOScale(0, 0.5f).SetDelay(0.5f).OnComplete((() =>
                {
                    textRunning.Txt.text = "2";
                    StoryPerforment.ImageContentSetup(imageContent, true, leftBoxContent);
                    currentTween = imageContent.transform.DOScale(1, 0.5f).OnComplete(() =>
                    {
                        currentTween = imageContent.transform.DOScale(0, 0.5f).SetDelay(0.5f).OnComplete((() =>
                        {
                            textRunning.Txt.text = "1";
                            StoryPerforment.ImageContentSetup(imageContent, true, leftBoxContent);
                            currentTween = imageContent.transform.DOScale(1, 0.5f).OnComplete(() =>
                            {
                                StoryPerforment.ImageContentSetup(imageContent, true, leftBoxContent);
                                currentTween = imageContent.transform.DOScale(0, 0.5f).SetDelay(0.5f).OnComplete((() =>
                                {
                                    textRunning.Txt.text = "";
                                    timer = 2;
                                    currentTween = null;
                                }));
                            });
                        }));
                    });
                }));
            });
        }

        void Step4()
        {
            string textContent = "Chesse";
            StoryPerforment.ImageContentSetup(imageContent, false, rightBoxContent);
            currentTween = npc.transform.DOMove(new Vector3(3, -3.75f, 0), 1).OnComplete((() =>
            {
                currentTween = imageContent.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    timer = textRunning.RunText(textContent) + 2;
                    currentTween = null;
                });
            }));
        }

        void Step5()
        {
            flashImage.gameObject.SetActive(true);
            DOVirtual.DelayedCall(0.25f, () => { flashImage.gameObject.SetActive(false); });
            currentTween = imageContent.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete((() =>
                timer = 2));
        }

        void Step6()
        {
            textRunning.Txt.text = "";
            currentTween = model.transform.DOMoveX(0, 0.5f).OnComplete((() =>
            {
                currentTween = npc.transform.DOMoveX(4, 0.5f).OnComplete((() =>
                {
                    StoryPerforment.ModelTalking(modelInside);
                    imageContent.sprite = leftBoxContent;
                    imageContent.rectTransform.anchoredPosition = new Vector3(70, -10, 0);
                    imageContent.rectTransform.pivot = new Vector2(0, 0);
                    currentTween = imageContent.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
                    {
                        timer = textRunning.RunText("But... this was supposed to be my moment...") + 2;
                        currentTween = null;
                    });
                }));
            }));
        }

        void Step7()
        {
            textRunning.Txt.text = "";
            StoryPerforment.ModelTalking(npc);
            StoryPerforment.ImageContentSetup(imageContent, false, rightBoxContent);
            string textContent = "Oops! Didn’t see you there. Think you can take a better shot than us? Prove it";
            currentTween = npc.transform.DOLocalMove(npc.transform.position + new Vector3(-1, 0, 0), 0.3f).OnComplete(
                () =>
                {
                    currentTween = imageContent.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
                    {
                        timer = textRunning.RunText(textContent) + 2;
                        StoryPerforment.DefaultRotatationModel(npc);
                        currentTween = null;
                    });
                });

        }

        void Step8()
        {
            textRunning.Txt.text = "";
            StoryPerforment.ModelTalking(modelInside);
           imageContent.sprite = leftBoxContent;
           imageContent.rectTransform.anchoredPosition = new Vector3(50, -10, 0);
           imageContent.rectTransform.pivot = new Vector2(0, 0);
           currentTween = imageContent.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
           {
               string textContent = "Oh, you think I can't? Watch me!";
               
               currentTween = imageContent.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
               {
                   timer = textRunning.RunText(textContent) + 2;
                   StoryPerforment.DefaultRotatationModel(npc);
                   currentTween = null;
               });
           });
        }

        void Step9()
        {
            textRunning.Txt.text = "";
            currentTween = imageContent.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                DOTween.KillAll();
               ManagerSceneDuck.ins.LoadScene(GlobalData.currentSceneMiniGame);
            });
        }
    }
}