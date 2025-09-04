using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace DevDuck
{
    public class ScoreGame2 : MonoBehaviour
    {
        public int score;
        public TextMeshProUGUI scoreText;
        public TextMeshPro scoreTextObject;
        public Vector3 startPos ;
        void Start()
        {
            Observer.AddObserver(EventAction.EVENT_GET_SCORE, GetScoreAndUpdateUi);
            Vector3 startPos = scoreTextObject.transform.localPosition;
        }

        private void OnDestroy()
        {
            Observer.RemoveObserver(EventAction.EVENT_GET_SCORE, GetScoreAndUpdateUi);
        }

        private void GetScoreAndUpdateUi(object obj)
        {
            bool isDone = (bool)obj;
                scoreText.text = score.ToString();
                scoreText.transform.DOScale(1.2f, 0.2f).OnComplete(() => { scoreText.transform.DOScale(1f, 0.2f); });
        }
        public void ShowScoreGeted(int score)
        {
            scoreTextObject.gameObject.SetActive(true);
            scoreTextObject.text = "+" + score.ToString();
           
            Vector3 newPos = startPos + new Vector3(0, 1.25f, 0);

            if (LogicGame2.instance.isDoneTutorial)
            {
                scoreTextObject.transform.DOLocalMove(newPos, 0.6f).SetEase(Ease.OutBack).OnComplete((() =>
                {
                    scoreTextObject.gameObject.SetActive(false);
                    scoreTextObject.transform.localPosition = startPos;
                }));
            }
            else
            {
                scoreTextObject.transform.DOLocalMove(newPos, 0.7f).SetEase(Ease.OutBack).OnComplete((() =>
                {

                    StartCoroutine(DelayHide());
                }));
            }
          
        }

        IEnumerator DelayHide()
        {
            yield return new WaitForSeconds(1f);
            scoreTextObject.gameObject.SetActive(false);
            scoreTextObject.transform.localPosition = startPos;
        }
        
    }
}