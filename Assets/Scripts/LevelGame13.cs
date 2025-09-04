using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DevDuck
{
    public class LevelGame13 : MonoBehaviour
    {
        [SerializeField] List<SpriteRenderer> itemGame13Spr = new List<SpriteRenderer>();
        [SerializeField] List<desGame13> des13 = new List<desGame13>();
        [SerializeField] List<ItemGame13> itemGame13 = new List<ItemGame13>();

        public void ShowListItem()
        {
            foreach (var item in itemGame13Spr)
            {
                item.DOFade(1, 0.3f);
            }
        }

        public void CheckDistance(ItemGame13 curentItem)
        {
            for (int i = 0; i < des13.Count; i++)
            {
                if (curentItem.id == des13[i].id && curentItem.Shape == des13[i].Shape)
                {
                    float distance = Vector3.Distance(curentItem.transform.position, des13[i].transform.position);
                    if (distance < 0.5f)
                    {
                        curentItem.transform.position = des13[i].transform.position;
                        curentItem.DisableCollider();
                        itemGame13.Remove(curentItem);

                        if (itemGame13.Count == 0)
                        {
                            UiGame13.instance.uiWinLose.ShowWin3Star();
                            Debug.Log("Show Win Panel");
                        }
                    }
                    else
                    {
                        curentItem.transform.position = curentItem.startPosition;
                    }
                }
            }
        }
    }
}