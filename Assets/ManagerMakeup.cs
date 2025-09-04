using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DevDuck
{
    public class ManagerMakeup : MonoBehaviour
    {
        [SerializeField] SpriteRenderer nLips, nEyelen, nEyeLine, nBlush;
        public List<itemMakeUp> listItemsMakeup = new List<itemMakeUp>();
        public List<ItemMakeupOnModel> listItemsMakeupModel = new List<ItemMakeupOnModel>();
        [SerializeField] private GameObject brushGameObject;
        public Button nextButton;
        public ParticleSystem sparklesFaces;
        private RaycastHit2D _hit;
        itemMakeUp currentItemMakeUp;
        [SerializeField] List<ParticleSystem> emotionParticles = new List<ParticleSystem>();
        [SerializeField] private Model _model;
        [SerializeField] private GameObject handTut;
        [SerializeField] private List<string> emotionList = new List<string>();
        [SerializeField] private GameObject bubbleEmotion;
        [SerializeField] private TextMeshPro emotionText;
        
        private void Awake()
        {
            nextButton.onClick.AddListener(OnClickNextButton);
        }
        private void OnClickNextButton()
        {
            ManagerSceneDuck.ins.LoadScene($"Story{GlobalData.currentStory}");
        }

        void Start()
        {
            GlobalData.isInGame = true;
            _model.LoadAndInitFashion();

            if (PlayerPrefs.GetInt(PlayerPrefsManager.TUT_MAKEUP) == 0)
            {
                handTut.gameObject.SetActive(true);
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && GlobalData.isInGame)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _hit = Physics2D.Raycast(mousePos, Vector3.forward, Mathf.Infinity);
                if (_hit.collider != null)
                {
                    currentItemMakeUp = _hit.collider.gameObject.GetComponent<itemMakeUp>();
                    if (currentItemMakeUp != null)
                    {
                        EquipMakeUp(currentItemMakeUp.id);
                        Duck.PlayParticle(sparklesFaces);
                        PlayPositiveParticles();
                    }
                }
            }
        }

        public void EquipMakeUp(int id)
        {
            if (PlayerPrefs.GetInt(PlayerPrefsManager.TUT_MAKEUP) == 0)
            {
                PlayerPrefs.SetInt(PlayerPrefsManager.TUT_MAKEUP,1);
                handTut.gameObject.SetActive(false);
            }
            for (int i = 0; i < listItemsMakeup.Count; i++)
            {
                if (listItemsMakeup[i].id == id)
                {
                    SetSprite(listItemsMakeup[i].makeupType, listItemsMakeup[i].id);
                    if (id > 2 && id < 11)
                    {
                        PerformBrush(id);
                    }
                    else if (id > 13)
                    {
                        PerformLipStick(currentItemMakeUp.id, currentItemMakeUp);
                    }
                    else
                    {
                        PerformLeftRightItemSelected(currentItemMakeUp);
                        SetItemMakeUpModel(currentItemMakeUp);
                    }
                }
            }
            currentItemMakeUp = null;
            ShowBubbleEmotionText();
        }

        public void SetSprite(MakeupType makeupType, int id)
        {
            switch (makeupType)
            {
                case MakeupType.LIP:
                    nLips.color = new Color32(255, 255, 255, 0);
                    nLips.sprite = Resources.Load<Sprite>("Art/AsssetToChange/makeup_" + id);
                    nLips.DOFade(1, 0.3f);
                    break;
                case MakeupType.EYES_LINE:
                    nEyeLine.color = new Color32(255, 255, 255, 0);
                    nEyeLine.sprite = Resources.Load<Sprite>("Art/AsssetToChange/makeup_" + id);
                    nEyeLine.DOFade(1, 0.3f);
                    break;
                case MakeupType.EYES_LEN:
                    nEyelen.color = new Color32(255, 255, 255, 0);
                    nEyelen.sprite = Resources.Load<Sprite>("Art/AsssetToChange/makeup_" + id);
                    nEyelen.DOFade(1, 0.3f);
                    break;
                case MakeupType.BLUSH:
                    nBlush.color = new Color32(255, 255, 255, 0);
                    nBlush.sprite = Resources.Load<Sprite>("Art/AsssetToChange/makeup_" + id);
                    nBlush.DOFade(1, 0.3f).SetDelay(1.5f);
                    break;
            }
        }

        public void PerformBrush(int id)
        {
            brushGameObject.GetComponent<Animator>().Play("default", 0, 0);
            brushGameObject.GetComponent<Animator>().Play($"blush_{id}", 0, 0);
        }
        public void PerformLipStick(int lipID, itemMakeUp item)
        {
            for (int i = 0; i < listItemsMakeup.Count; i++)
            {
                if (listItemsMakeup[i].makeupType == MakeupType.LIP)
                {
                    listItemsMakeup[i].GetComponent<Animator>().Play("default", 0, 0);
                    listItemsMakeup[i].OnItemReleased();
                }
            }
            item.GetComponent<Animator>().Play($"lipstick_{lipID}", 0, 0);
        }
        public void PerformLeftRightItemSelected(itemMakeUp item)
        {
            item.GetComponent<SpriteRenderer>().enabled = false;
            item.OnItemSelected();
        }

        public void SetItemMakeUpModel(itemMakeUp itemMakeUp)
        {
            for (int i = 0; i < listItemsMakeupModel.Count; i++)
            {
                if (itemMakeUp.makeupType == itemMakeUp.makeupType)
                {
                    int temp = listItemsMakeupModel[i].currentID;
                    listItemsMakeupModel[i].currentID = itemMakeUp.id;
                    for (int j = 0; j < listItemsMakeup.Count; j++)
                    {
                        if (listItemsMakeup[j].id == temp)
                        {
                            listItemsMakeup[j].BoxCollider2D.enabled = true;
                            listItemsMakeup[j].GetComponent<SpriteRenderer>().enabled = true;
                        }
                    }
                }
            }
        }

        private void PlayPositiveParticles()
        {
            int idPlay = Duck.GetRandom(0,emotionParticles.Count);
            Duck.PlayParticle(emotionParticles[idPlay]);
        }

        private void ShowBubbleEmotionText()
        {
            bubbleEmotion.gameObject.SetActive(true);
            int idContent = Duck.GetRandom(0,emotionList.Count);
            emotionText.text = emotionList[idContent];
            bubbleEmotion.transform.localScale = Vector3.zero;
            bubbleEmotion.transform.DOKill();
            bubbleEmotion.transform.DOScale(0.8f, 0.3f).SetEase(Ease.OutBack);
            bubbleEmotion.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).SetDelay(1f);
        }
        
    }
}