using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DevDuck
{
    public class TutorialGame5 : MonoBehaviour
    {
        public GameObject handHintPress;
        public NpcGame6Tut npcTutorial;

        public void NpcOnTutorial()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(npcTutorial.transform.DOMove(new Vector3(0, 7.35f, 0), .4f).OnComplete(() =>
            {
                npcTutorial.transform.DOScale(2.2f, 1);
            }));
            sequence.Append(npcTutorial.transform.DOMove(new Vector3(0, 2, 0), 1f).OnComplete(() =>
            {
                npcTutorial.box3Order.gameObject.SetActive(true);
                npcTutorial.ShowOrder();
                LogicGame5.ins.isCanPlay = true;
            }));
        }
    }
}