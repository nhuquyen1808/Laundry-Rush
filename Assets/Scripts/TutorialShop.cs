using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class TutorialShop : MonoBehaviour
    {
        public GameObject handHintBuy, handHintReturn;
        public bool isDoneTutShop;
        
        void Awake()
        {
            isDoneTutShop  = PlayerPrefs.GetInt("isDoneTutShop", 0) == 1;
        }

        public void EnableHandHintBuy(Vector3 pos)
        {
            handHintBuy.SetActive(true);
            handHintBuy.transform.position = pos;
        }

        public void EnableHandHintReturn()
        {
            handHintReturn.SetActive(true);
        }
    }
}
