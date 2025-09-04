using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DevDuck
{
    public class FinalPetalGame12 : MonoBehaviour
    {
        public List<PetaFinalPiece> listFinalPetalGame12 = new List<PetaFinalPiece>();


        public void CheckFinalPetalGame12()
        {
            for (int i = 0; i < listFinalPetalGame12.Count; i++)
            {
                listFinalPetalGame12[i].Check();
            }

            CheckFinalPetalGame12Done();
        }

        public void CheckFinalPetalGame12Done()
        {
            int count = 0;
            for (int i = 0; i < listFinalPetalGame12.Count; i++)
            {
                if (listFinalPetalGame12[i].isDone == true)
                {
                    count++;
                    if (count == listFinalPetalGame12.Count)
                    {
                        DOVirtual.DelayedCall(1, () => ShowEffectDone());
                        Debug.Log("WIN ????");
                    }
                }
            }
        }

        public void ShowEffectDone()
        {
            for (int i = 0; i < listFinalPetalGame12.Count; i++)
            {
                listFinalPetalGame12[i].SetLightSprite();
            }

            gameObject.transform.DOScale(1.2f, 0.2f).OnComplete(() =>
            {
                gameObject.transform.DOScale(1f, 0.2f);
            });
        }
    }
}