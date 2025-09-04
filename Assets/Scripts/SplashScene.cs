using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class SplashScene : MonoBehaviour
    {
        public Image fillImage;

        private void Start()
        {
            AudioManager.instance.PlayBGMSound("BG");
            fillImage.DOFillAmount(1, 3f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                ManagerSceneDuck.ins.LoadScene("ClothesSort");
            });
        }
    }
}
