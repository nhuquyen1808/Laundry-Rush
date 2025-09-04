using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DevDuck
{
    public class LogicGame13 : MonoBehaviour
    {
        private RaycastHit2D _hit2D;
        bool isDragging = false;
        private ItemGame13 currentItemGame13;
        public GameObject board, leftCurtain, rightCurtain;
        [SerializeField] LevelGame13 currentLevelGame13;
        public ParticleSystem starExplosion;
        
        private void Start()
        {
            MoveBoard();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _hit2D = Physics2D.Raycast(mousePosition, Vector3.forward, Mathf.Infinity);
                if (_hit2D.collider == null) return;
                ItemGame13 itemGame13 = _hit2D.collider.GetComponent<ItemGame13>();
                if (itemGame13 != null)
                {
                    isDragging = true;
                    currentItemGame13 = itemGame13;
                }
            }

            if (isDragging && currentItemGame13 != null)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 pos = new Vector3(mousePosition.x, mousePosition.y, 0);
                currentItemGame13.transform.position = Vector3.Lerp(currentItemGame13.transform.position, pos, 0.2f);
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (currentItemGame13 != null)
                {
                    currentLevelGame13.CheckDistance(currentItemGame13);
                    if (currentItemGame13.isDone)
                    {
                        starExplosion.transform.position = currentItemGame13.transform.position;
                        Duck.PlayParticle(starExplosion);
                    }
                    currentItemGame13 = null;
                    isDragging = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }

        private void MoveBoard()
        {
            board.transform.DOScale(1, 0.7f).SetEase(Ease.OutBack).From(0).OnComplete(() =>
            {
                leftCurtain.transform.DOMove(leftCurtain.transform.position + new Vector3(-8, 0, 0), 0.8f)
                    .SetDelay(0.2f);
                rightCurtain.transform.DOMove(rightCurtain.transform.position + new Vector3(8, 0, 0), 0.8f)
                    .SetDelay(0.4f).OnComplete((
                        () => { currentLevelGame13.ShowListItem(); }));
            });
        }
    }
}