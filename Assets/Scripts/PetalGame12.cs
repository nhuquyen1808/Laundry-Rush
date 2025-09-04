using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DevDuck
{
    public class PetalGame12 : MonoBehaviour
    {
        public BoxCollider2D boxCollider;
        public bool isHasEnegy;
        public List<Petal> petals = new List<Petal>();
        private int countStrinkShow;
        public int id;
        
        private void Start()
        {
            for (int i = 0; i < petals.Count; i++)
            {
                if (petals[i].strink != null)
                {
                    countStrinkShow++;
                }
            }
        }

        public void CheckPetals()
        {
            for (int i = 0; i < petals.Count; i++)
            {
                petals[i].Hit();
                petals[i].SetDarkSprite();
               
            }
            DOVirtual.DelayedCall(.1f, CheckPetalDone) ;
        }

        private int countPetalDone;
        
        public void CheckPetalDone()
        {
            countPetalDone = 0;
            for (int i = 0; i < petals.Count; i++)
            {
                if (petals[i].isDone)
                {
                    countPetalDone++;
                    if (countPetalDone == countStrinkShow)
                    {
                        Debug.Log("?////"); 
                        ShowPetalDone();
                        Level_1_Game12.instance.SetEnegy(id);
                    }
                }
            }
        }
        public void ShowPetalDone()
        {
            for (int i = 0; i < petals.Count; i++)
            {
                petals[i].ShowDone();
            }
        }
    }
}
