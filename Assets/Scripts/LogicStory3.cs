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
    public class LogicStory3 : MonoBehaviour
    {
        private Tween currentTween;
        List<Action> actions = new List<Action>();
        public GameObject model;
        public GameObject npc1;
        public GameObject modelInside, npc2;
        int step = 0;
        public float timer = 0;
        public TextRunning textRunning;
        public Image imageContent;
        public Sprite leftBoxContent, rightBoxContent;

        public ParticleSystem heartFlyParticle, angryParticle;
        public List<GameObject> vegetablesPos = new List<GameObject>();

        public Button skipButton;

        private void Awake()
        {
            skipButton.onClick.AddListener(OnClickSkipButton);
        }

        private void OnClickSkipButton()
        {
            skipButton.interactable = false;
            step = actions.Count - 1;
            npc1.transform.localPosition = new Vector3(-2.5f, -4.5f, 0);
            npc2.transform.localPosition = new Vector3(1.5f, -4.5f, 0);
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
                timer -= Duck.TimeMod;;
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
            StoryPerforment.ModelMovement(npc1);
            currentTween = npc1.transform.DOLocalMove(new Vector3(-2.5f, -4.5f, 0), 2f).OnComplete(() =>
            {
                StoryPerforment.DefaultRotatationModel(npc1);
                timer = 2;
                currentTween = null;
            });
        }

        public void Step2()
        {
            string textContent = "Ugh... Not this again. Do they ever change the menu?";
            StoryPerforment.ImageContentSetup(imageContent, true, leftBoxContent);
            currentTween = imageContent.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                StoryPerforment.ModelTalking(npc1);
                timer = textRunning.RunText(textContent);
                currentTween = null;
            });
        }

        public void Step3()
        {
            StoryPerforment.ImageContentSetup(imageContent, false, rightBoxContent);
            textRunning.Txt.text = "";
            string textContent = "What if I sell hamburgers?";
            currentTween = imageContent.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                currentTween = imageContent.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    StoryPerforment.ModelTalking(modelInside);
                    timer = textRunning.RunText(textContent);
                    currentTween = null;
                });
            });
        }

        public void Step4()
        {
            textRunning.Txt.text = "";
            string textContent = "What a good idea!?";
            currentTween = imageContent.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                currentTween = imageContent.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    Duck.PlayParticle(heartFlyParticle);
                    StoryPerforment.ModelTalking(modelInside);
                    timer = textRunning.RunText(textContent);
                    currentTween = null;
                });
            });
        }

        public void Step5()
        {
            textRunning.Txt.text = "";
            currentTween = imageContent.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete((() =>
            {
                Sequence moveSequence = DOTween.Sequence();
                for (int i = 0; i < vegetablesPos.Count; i++)
                {
                    var a = i;
                    currentTween = moveSequence.Append(npc2.transform.DOMove(vegetablesPos[i].transform.position, 0.5f)
                        .SetEase(Ease.Linear).SetDelay(0.5f).OnComplete(
                            () =>
                            {
                                vegetablesPos[a].transform
                                    .DOJump(vegetablesPos[a].transform.position + new Vector3(2, -4, 0), 3f, 1, 1,
                                        false);
                                if (a == vegetablesPos.Count - 1)
                                {
                                    timer = 1.5f;
                                    currentTween = null;
                                }
                            }));
                }

                moveSequence.Play();
            }));
        }

        public void Step6()
        {
            textRunning.Txt.text = "";
            string textContent = "Selling burgers? Cute. But let’s be real—who’d eat your greasy, amateur cooking?";
            StoryPerforment.ImageContentSetup(imageContent, true, leftBoxContent);
            currentTween = imageContent.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                currentTween = imageContent.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    StoryPerforment.ModelTalking(npc2);
                    timer = textRunning.RunText(textContent);
                    currentTween = null;
                });
            });
        }

        public void Step7()
        {
            textRunning.Txt.text = "";
            string textContent = "Just wait and see, you will have to change your mind.";
            StoryPerforment.ImageContentSetup(imageContent, false, rightBoxContent);

            currentTween = imageContent.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                currentTween = imageContent.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    Duck.PlayParticle(angryParticle);
                    StoryPerforment.ModelTalking(modelInside);
                    timer = textRunning.RunText(textContent);
                    currentTween = null;
                });
            });
        }

        void Step8()
        {
            currentTween = imageContent.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                currentTween = null;
                
                DOVirtual.DelayedCall(0.5f, (() =>
                {
                    DOTween.KillAll();
                  //  SceneManager.LoadScene(GlobalData.currentSceneMiniGame);
                    ManagerSceneDuck.ins.LoadScene(GlobalData.currentSceneMiniGame);
                }));
            });
        }
    }
}