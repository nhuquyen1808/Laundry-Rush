using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DevDuck
{
    public class UI_TextAnimatorCoin : MonoBehaviour
    {
        public TextMeshProUGUI coinText;

        public void Set200CoinText()
        {
            coinText.text = "200";
        }
        public void Set400CoinText()
        {
            coinText.text = "400";
        }
        public void Set600CoinText()
        {
            coinText.text = "600";
        }
    }
}
