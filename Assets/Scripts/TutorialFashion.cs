using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace DevDuck
{
    public class TutorialFashion : MonoBehaviour
    {
        public GameObject handHint;
        public bool isDoneTutFashion;
        public bool isDoneTopHint;
        public bool isDoneTutBuyInFashion;
        public bool isDoneTopBuyInFashion;
        public GameObject handHintBuy;
        public GameObject TAPTOCHOOSE;
        
        private void Awake()
        {
            isDoneTutFashion = PlayerPrefs.GetInt(PlayerPrefsManager.TUT_FASHION, 0) == 1;
            //isDoneTutBuyInFashion = PlayerPrefs.GetInhhhhhkdsf 0) == 1;
            
        }

        private void Start()
        {
            HideTapToChoose();
            StartCoroutine(ActiveHand()) ;
        }

        IEnumerator ActiveHand()
        {
            yield return new WaitForSeconds(1.1f);
            if (!isDoneTutFashion )
            {
                handHint.SetActive(true);
            }
        }

        private void HideTapToChoose()
        {
            DOVirtual.DelayedCall(3.5f, (() =>
            {
                if (TAPTOCHOOSE != null)
                {
                    TAPTOCHOOSE.GetComponent<Animator>().enabled = false;
                    TAPTOCHOOSE.transform.DOScale(0, 0.3f);
                }
            }));
        }
    }
}