using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class FocusTUTFashion : TutorialMenu
    {
        public GameObject nContent;
        public static FocusTUTFashion instance;

        private int areaUnlock;
        
        private void Awake()
        {
            instance = this;
        }

        public override void Start()
        {
            if (PlayerPrefs.GetInt(PlayerPrefsManager.TUT_FASHION) == 0)
            {
                StartCoroutine(SetHand());
            }
            
            areaUnlock = PlayerPrefs.GetInt(PlayerPrefsManager.AREA_UNLOCK);
            if (areaUnlock == 1)
            {
                Debug.Log("Area Unlock 1 and make tut buy here");
            }
        }

        IEnumerator SetHand()
        {
            Debug.Log("SetHand");
            StartCoroutine(ScaleFocus(8));
            yield return new WaitForSeconds(1);
            transform.position = LogicFashion.instance.listFashionItemsGot[1].transform.position;
            Vector3 botPos = LogicFashion.instance.listFashionItemsGot[1].transform.position +
                             new Vector3(0.4f, 0.2f, 0);
            hand.transform.position = botPos;
            hand.SetActive(true);
        }
    }
}