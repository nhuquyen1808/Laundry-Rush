using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class TutorialMenu : MonoBehaviour
    {
        private Renderer rend;
        public float lerpSpeed;
        public GameObject hand;
        float scale = 0;
        private int levelUnlock;

        public virtual void Start()
        {
            levelUnlock = PlayerPrefs.GetInt(PlayerPrefsManager.AREA_UNLOCK);
            if (levelUnlock == 0)
                StartCoroutine(ScaleFocus(11));
        }

        public IEnumerator ScaleFocus(float target)
        {
            float timeLapse = 0;
            yield return new WaitForSeconds(1f);
            hand.gameObject.SetActive(true);
            rend = GetComponent<Renderer>();
            timeLapse += Duck.TimeMod;
            while (timeLapse < 1.5)
            {
                scale = Mathf.Lerp(scale, target, lerpSpeed * Time.deltaTime);
                rend.material.SetFloat("Scale_Focus", scale);
                yield return null;
            }
        }
    }
}