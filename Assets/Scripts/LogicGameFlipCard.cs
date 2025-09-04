using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DevDuck
{
    public class LogicGameFlipCard : MonoBehaviour
    {
        [Header("Sprite : ")] [SerializeField] Sprite modelHandUp, modelHandDown, catHandUp;

        [Header("List SpriteRenderer : ")] public List<GameObject> gameObjects = new List<GameObject>();
        public List<SpriteRenderer> CharactersSprite = new List<SpriteRenderer>();
        public List<SpriteRenderer> cats = new List<SpriteRenderer>();
        public List<SpriteRenderer> catsChild = new List<SpriteRenderer>();
        public List<SpriteRenderer> models = new List<SpriteRenderer>();

        [Header("Wave Sprite : ")] 
        [SerializeField] List<Sprite> wave1 = new List<Sprite>();
        [SerializeField] List<Sprite> wave2 = new List<Sprite>();
        [SerializeField] List<Sprite> wave3 = new List<Sprite>();


        void Start()
        {
            StartCoroutine(DelayFlip());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadSceneAsync("GameFlipCard");
            }
        }

        IEnumerator DelayFlip()
        {
            yield return new WaitForSeconds(1);
            ShowListObjUi(gameObjects);
        }

        private void ShowListObjUi(List<GameObject> _list)
        {
            SetLayer(cats, -3);
            SetLayer(catsChild, -3);
            SetLayer(models, -3);
            SetSprite(models,modelHandUp);
            SetSprite(cats,catHandUp);
            for (int i = 0; i < _list.Count; i++)
            {
                Sequence showItemUI = DOTween.Sequence();
                showItemUI.AppendInterval(i * 0.1f);
                showItemUI.Append(_list[i].transform.DORotate(new Vector3(200, 0, 0), 1f, RotateMode.FastBeyond360)
                    .SetEase(Ease.InBack).OnComplete(() =>
                    {
                        Debug.Log(i);
                    }));
                showItemUI.Insert(0.5f, _list[i].transform.GetChild(1).GetComponent<SpriteRenderer>().DOFade(1, 0.2f));
            }
        }
        private void SetLayer(List<SpriteRenderer> _list, int _layer)
        {
            foreach (SpriteRenderer sprite in _list)
            {
                sprite.sortingOrder = _layer;
            }
        }
        private void SetSprite(List<SpriteRenderer> _list, Sprite _spr)
        {
            foreach (SpriteRenderer spr in _list)
            {
                spr.sprite = _spr; 
            }
        }
    }
}