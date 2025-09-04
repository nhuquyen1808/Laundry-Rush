using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DevDuck
{
    public class AnimationOtherGame : AnimationEventGame4
    {
        public override void FallStarLose()
        {
            animator.enabled = false;
            rightStarLose.GetComponent<RectTransform>().DOJump(rightStarLose.transform.position - new Vector3(-100, 2000, 0), 700, 1, 1.3f).SetDelay(0.5f).OnComplete(() =>
            {
                Observer.Notify(EventAction.EVENT_POPUP_SHOW_LOSE_DONE, "");
            });
            pieceRightStarLose.GetComponent<RectTransform>().DOJump(pieceRightStarLose.transform.position - new Vector3(-100, 2000, 0), 700, 1, 1).SetDelay(0.5f);
        }
    }
}
