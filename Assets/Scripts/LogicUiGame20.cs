using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class LogicUiGame20 : MonoBehaviour
    {
        public TextMeshPro startText;
        public Image PreventImage;

        public void AnimStartGame()
        {
            PreventImage.gameObject.SetActive(true);
            Sequence mySequence = DOTween.Sequence();
            startText.gameObject.SetActive(true);
            startText.text = "3";
            mySequence.Append(startText.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack).From(0));
            mySequence.Append(startText.transform.DOScale(0f, 0.2f).SetDelay(0.5f).SetEase(Ease.InBack)
                .OnComplete(() => startText.text = "2"));
            mySequence.Append(startText.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack));
            mySequence.Append(startText.transform.DOScale(0f, 0.5f).SetEase(Ease.InBack).SetDelay(0.5f).OnComplete(() =>
            {
                startText.text = "1";
            }));
            mySequence.Append(startText.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack));
            mySequence.Append(startText.transform.DOScale(0f, 0.5f).SetEase(Ease.InBack).SetDelay(0.5f)
                .OnComplete(() => startText.text = "Start!!"));
            mySequence.Append(startText.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack).SetDelay(0.5f)
                .OnComplete(() =>
                {
                    startText.DOFade(0, 0.2f).SetDelay(0.5f)
                        .OnComplete(() => PreventImage.gameObject.SetActive(false));
                }));
        }
    }
}