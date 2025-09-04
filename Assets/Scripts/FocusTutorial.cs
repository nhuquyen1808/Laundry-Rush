using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace DevDuck
{
    public class FocusTutorial : MonoBehaviour
    {
        private Renderer rend;
        public bool isLerpDone;
        public float lerpSpeed;
        public GameObject hand;
        float scale = 0;

        void Start()
        {
          DOVirtual.DelayedCall(1, () => { isLerpDone = true; });
        }

        void Update()
        {
            if (isLerpDone)
            {
                scale = Mathf.Lerp(scale, 15, lerpSpeed*Time.deltaTime);
                rend = GetComponent<Renderer>();
                rend.material.SetFloat("Scale_Focus",scale);
                if (scale >= 15f)
                {
                    isLerpDone = false;
                }
            }
            
            
        }
        
        
    }
}
