using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

namespace DevDuck
{
    public class LogicUiGame6 : MonoBehaviour
    {
        //  public Sprite tickSprite;
        public static LogicUiGame6 instance;

        public ParticleSystem smokeEffect;

        //  public Image tickImage;
        public List<ParticleSystem> positiveParticles = new List<ParticleSystem>();
        public List<ParticleSystem> negativeParticles = new List<ParticleSystem>();

        [Header("Win lose : ")] [SerializeField]
        private TextMeshProUGUI finishText;

        [Header("HeartBar : ")] [SerializeField]
        private List<Image> heartPieces = new List<Image>();

        [SerializeField] private List<Image> heartPar = new List<Image>();
        public GameObject tickObject;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            PopupHeartBar();
        }

        public void PlaySmokeEffect()
        {
            Duck.PlayParticle(smokeEffect);
        }

        public void ShowTickImage(GameObject target)
        {
            /*tickImage.gameObject.SetActive(true);
            tickImage.rectTransform.position = Camera.main.WorldToScreenPoint(target.transform.position);
            tickImage.color = Color.white;
            tickImage.DOFillAmount(1, 0.7f).From(0).SetEase(Ease.OutBack).OnComplete(() =>
            {
                tickImage.DOFade(0, 0.01f).SetEase(Ease.InBack).SetDelay(0.3f);
            });*/
        }

        public void DisableHeartPieces(int piecesToDisable)
        {
            if (heartPieces[piecesToDisable] == null) return;
            heartPieces[piecesToDisable].DOFade(0, 0.5f).SetEase(Ease.InBack);
        }

        private void PopupHeartBar()
        {
            Sequence sequence = DOTween.Sequence();
            for (int i = 0; i < heartPar.Count; i++)
            {
                /*var a = i;*/
                sequence.Append(heartPar[i].transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).From(0));
                sequence.AppendInterval(i * 0.1f);
            }
        }

        public void PlayPositiveParticles(Vector3 pos)
        {
            int a = Duck.GetRandom(0, positiveParticles.Count);
            positiveParticles[a].transform.position = pos;
            Duck.PlayParticle(positiveParticles[a]);
        }

        public void PlayNegativeParticles(Vector3 pos)
        {
            int a = Duck.GetRandom(0, negativeParticles.Count);
            negativeParticles[a].transform.position = pos;
            Duck.PlayParticle(negativeParticles[a]);
        }
    }
}