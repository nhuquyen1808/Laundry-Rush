using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace DevDuck
{
    public class TutorialGame6 : MonoBehaviour
    {
        [FormerlySerializedAs("npcGame60Tut")] [FormerlySerializedAs("npcGame6Tut")] public NpcGame5Tut npcGame5Tut;
        public GameObject glowPar;
        public GameObject handHintPress;
        public static TutorialGame6 Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void NpcTutorial()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(npcGame5Tut.transform.DOMove(new Vector3(0, 7.35f, 0), .4f).OnComplete(() =>
            {
                npcGame5Tut.transform.DOScale(2.5f , 1);
            }));
            sequence.Append(npcGame5Tut.transform.DOMove(new Vector3(0, 2, 0), 1f).OnComplete(()=>
            {
                npcGame5Tut.ShowOrder();
                glowPar.gameObject.SetActive(true);
                handHintPress.gameObject.SetActive(true);
            }));
            
           // Handheld.Vibrate();
        }
        
    }
}
