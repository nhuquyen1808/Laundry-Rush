using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class TestButton : MonoBehaviour
    {
        public Image onButton, offButton;
        public Button testButton;
        
        bool isOn = true;
        void Start()
        {
            testButton = GetComponent<Button>();
            testButton.onClick.AddListener(OnLickTestButtonClicked);
        }

            bool isCanClick = true;
        private void OnLickTestButtonClicked()
        {
            if (isCanClick)
            {
                isCanClick = false;
                if (isOn)
                {
                    isOn = false;
                    Vector3 startPos = onButton.transform.position;
                    offButton.transform.SetAsFirstSibling();
                    onButton.transform.DOMove(offButton.transform.position, 0.4f).OnComplete(()=>
                    {
                        onButton.transform.position = startPos;
                        isCanClick = true;
                    });
                    offButton.DOFade(1,0.4f).From(0);
                    onButton.DOFade(0,0.4f).From(1);
                }
                else
                {
                    isOn = true;
                    Vector3 startPos = offButton.transform.position;
                    onButton.transform.SetAsFirstSibling();
                    offButton.transform.DOMove(onButton.transform.position, 0.4f).OnComplete(()=>
                    {
                        offButton.transform.position = startPos;
                        isCanClick = true;
                    });
                    onButton.DOFade(1,0.4f).From(0);
                    offButton.DOFade(0,0.4f).From(1);
                }
            }

        }
    }
}
