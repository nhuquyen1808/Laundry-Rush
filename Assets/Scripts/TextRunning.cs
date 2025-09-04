using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DevDuck
{
    public class TextRunning : MonoBehaviour
    {
         TextMeshProUGUI txt;
           public TextMeshProUGUI Txt
           {
               get
               {
                   if (txt == null) txt = GetComponent<TextMeshProUGUI>();
                   return txt;
               }
           }
       
           public Action actDone;
       
           char[] strContent;
           char[] strTemp;
           public float RunText(string str, float time = 1.2f)
           {
               strContent = str.ToCharArray();
               strTemp = new char[strContent.Length];
       
               for (int i = 0; i < strContent.Length; ++i)
                   strTemp[i] = ' ';
       
               Txt.text = new string(strTemp);
       
               totalTime = time * strContent.Length / 20;
               timer = 0;
               return totalTime;
           }
       
           public float CalTime(string str, float time = 1f)
           {
               return time * str.Length / 20;
           }
       
           public float timer = -1;
           public float totalTime;
       
           void Update()
           {
               if (timer >= 0)
               {
                   timer += Duck.TimeMod;
       
                   int t = (int)(timer / totalTime * strContent.Length);
                   for (int i = 0; i <= t && i < strContent.Length; ++i)
                       strTemp[i] = strContent[i];
                   Txt.text = new string(strTemp);
       
                   if (t >= strContent.Length)
                   {
                       actDone?.Invoke();
                       timer = -1;
                   }
               }
           }
    }
}
