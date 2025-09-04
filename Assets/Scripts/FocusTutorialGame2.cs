using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class FocusTutorialGame2 : MonoBehaviour
    {
       public Renderer _renderer;

       public static FocusTutorialGame2 Instance;

       private void Awake()
       {
           Instance = this;
       }

       private void Start()
       {
           _renderer = GetComponent<Renderer>();
       }

       public virtual IEnumerator ScaleFocus(float time,float endValue)
       {
           float elapsedTime = 0;
           float scaleFocus = 0;
           
           while (elapsedTime < time)
           {
               elapsedTime += Duck.TimeMod;
               scaleFocus = Mathf.Lerp(scaleFocus, endValue, elapsedTime / time);
               _renderer.material.SetFloat("Scale_Focus", scaleFocus);
               yield return null;
           }
       }
       
       public IEnumerator ScaleFocusOut(float time,float endValue)
       {
           float elapsedTime = 0;
           float scaleFocus = 0;
           
           while (elapsedTime < time)
           {
               elapsedTime += Time.deltaTime;
               scaleFocus = Mathf.Lerp(scaleFocus, endValue, elapsedTime / time);
               _renderer.material.SetFloat("Scale_Focus", scaleFocus);
               yield return null;
           }
       }
    }
}
