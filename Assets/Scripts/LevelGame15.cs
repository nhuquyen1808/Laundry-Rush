using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DevDuck
{
    public class LevelGame15 : MonoBehaviour
    {
        public PlayerGame15 playerGame15;
        public GameObject board;


        public List<GameObject> objectsToShow = new List<GameObject>();

        public void ShowObjectScene()
        {
            Vector3 boardPos = board.transform.position;
            Vector3 newPos = boardPos + new Vector3(15, 0f, 0);
            board.transform.position = newPos;
            board.transform.DOMove(boardPos, 0.7f).SetDelay(0.3f).SetEase(Ease.OutBack).OnComplete((() =>
            {
                for (int i = 0; i < objectsToShow.Count; i++)
                {
                    var a = i;
                    objectsToShow[a].transform.DOScale(1, 0.2f).SetEase(Ease.OutBack).SetDelay(a * 0.07f).OnComplete((
                        () =>
                        {
                            if (a == objectsToShow.Count - 1)
                            {
                                LogicGame15.instance.isInsDone = true;
                            }
                        }));
                }
            }));
        }
    }
}