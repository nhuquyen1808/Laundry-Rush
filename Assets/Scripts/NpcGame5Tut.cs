using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DevDuck
{
    public class NpcGame5Tut : NpcGame5
    {
        public override void ShowOrder()
        {
            idFood1 = 3;
            food1.GetComponent<SpriteRenderer>().sprite =
                Resources.Load<Sprite>($"Art/game5/easy_mode/food_{idFood1}");
            idFood2 = 1;
            food2.GetComponent<SpriteRenderer>().sprite =
                Resources.Load<Sprite>($"Art/game5/easy_mode/food_{idFood2}");
            idFood3 = 2;
            food3.GetComponent<SpriteRenderer>().sprite =
                Resources.Load<Sprite>($"Art/game5/easy_mode/food_{idFood3}");
            boxOrder3Food.SetActive(true);
            food1.transform.SetParent(boxOrder3Food.transform);
            food2.transform.SetParent(boxOrder3Food.transform);
            food3.transform.SetParent(boxOrder3Food.transform);
            listID.Add(idFood1);
            listID.Add(idFood2);
            listID.Add(idFood3);
            food1.transform.DOScale(0.7f, 0.3f).SetEase(Ease.OutBack).From(0);
            food2.transform.DOScale(0.7f, 0.3f).SetDelay(0.2f).SetEase(Ease.OutBack).From(0);
            food3.transform.DOScale(0.7f, 0.3f).SetDelay(0.3f).SetEase(Ease.OutBack).From(0);
            food1.transform.localPosition = new Vector3(0.2f, -0.25f, 0f);
            food2.transform.localPosition = new Vector3(1.15f, -0.25f, 0f);
            food3.transform.localPosition = new Vector3(2.1f, -0.25f, 0f);
            boxOrder3Food.transform.DOScale(.6f, 0.3f).SetEase(Ease.OutBack).SetDelay(0.5f);
        }

        public void CheckFood(Food food)
        {
            if (!CheckNpcHasOrder(food.id))
            {
                food.transform.DOJump(LogicGame6.Ins.plateFood.transform.position, 0.5f, 1, 0.5f).OnComplete(() =>
                {
                    LogicUiGame6.instance.PlayNegativeParticles(new Vector3(1f, 4.28f, 0));
                    food.ShowFailFood();
                    LogicGame6.Ins.InitFoods(food.id);
                });
            }
            else
            {
                TutorialGame6.Instance.handHintPress.SetActive(false);

                Sequence mySequence = DOTween.Sequence();
                LogicGame6.Ins.isCanPlay = false;
                LogicGame6.Ins.InitFoods(food.id);
                mySequence.Append(food.transform.DOJump(LogicGame6.Ins.plateFood.transform.position, 0.5f, 1, 0.5f)
                    .OnComplete(() => Duck.PlayParticle(LogicGame6.Ins.smokeParticles)));
                mySequence.Append(food.transform.DOJump(LogicGame6.Ins.foodDes.transform.position, 0.5f, 1, 0.5f)
                    .SetDelay(0.5f));
                mySequence.Insert(1.1f, food.GetComponent<SpriteRenderer>().DOFade(0f, 0.15f).SetEase(Ease.InBack)
                    .OnComplete(() =>
                    {
                        LogicGame6.Ins.isCanPlay = true;

                        LogicUiGame6.instance.PlayPositiveParticles(new Vector3(1f, 4.28f, 0));
                        if (CheckIsFullOrder() == false) return;
                        DOVirtual.DelayedCall(1f, MoveOut);
                    }));
            }
        }

        private void MoveOut()
        {
            transform.DOMove(transform.position + new Vector3(10, 0, 0), 1).SetDelay(0.3f)
                .OnStart(
                    () =>
                    {
                        LogicGame6.Ins.isDoneTutorial = true;
                        LogicGame6.Ins.isCanPlay = false;
                        PlayerPrefs.SetInt("isDoneTutorialGame6", 1);
                        HideOrder();
                    }).OnComplete((() =>
                {
                    LogicGame6.Ins.NpcMoveToTable();
                    LogicGame6.Ins.ShowFoodOnTable();
                }));
        }
    }
}