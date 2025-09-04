using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DevDuck
{
    public class Level_1_Game12 : MonoBehaviour
    {
        private RaycastHit2D hit;
        public CanvasScaler canvas;
        bool isCanClick = true;

        public LayerMask stroke, nostrokes;
        public static Level_1_Game12 instance;
        public FinalPetalGame12 finalPetal;
        public StartGame12 startGame12;
        public List<PetalGame12> ListPetalGame12 = new List<PetalGame12>();
        private void Awake()
        {
            instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (Screen.width < 1080)
            {
                canvas.referenceResolution = new Vector2(1080, 1920);
            }
            else
            {
                canvas.referenceResolution = new Vector2(Screen.width, 1920);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadSceneAsync("Game12");
            }

            if (Input.GetMouseButtonDown(0) && isCanClick)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                hit = Physics2D.Raycast(mousePos, Vector3.forward, Mathf.Infinity, stroke);
                if (hit.collider == null) return;
                PetalGame12 petal = hit.collider.gameObject.GetComponent<PetalGame12>();
                if (petal != null)
                {
                    isCanClick = false;
                    
                    petal.transform.DORotate(petal.transform.eulerAngles + new Vector3(0, 0, 90), 0.5f)
                        .SetEase(Ease.OutBack).OnComplete(() =>
                        {
                            isCanClick = true;
                            petal.CheckPetals();
                            startGame12.CheckType();
                            finalPetal.CheckFinalPetalGame12();
                            CheckPetals();
                            // DOVirtual.DelayedCall(0.6f, () =>finalPetal.CheckFinalPetalGame12());
                        });
                }
            }
        }

        public void SetEnegy(int id)
        {
            for (int i = 0; i < ListPetalGame12.Count; i++)
            {
                if (ListPetalGame12[i].id  == id +1)
                {
                    if( ListPetalGame12[i] != null)
                    {
                        Debug.Log("MMMMM");
                        ListPetalGame12[i].GetComponent<BoxCollider2D>().enabled = true;
                    }
                    
                }
            }
        }

        public void CheckPetals()
        {
            for (int i = 0; i < ListPetalGame12.Count; i++)
            {
                ListPetalGame12[i].CheckPetals();
            }
        }
    }
}