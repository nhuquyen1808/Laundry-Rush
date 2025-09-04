using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

namespace DevDuck
{
    public class LogicGame16 : MonoBehaviour
    {
        private RaycastHit2D hit;
        Camera camera;
        public CardGame16 cardGame;
        public List<CardGame16> cardGames = new List<CardGame16>();
        public List<Vector3> posToShow = new List<Vector3>();
        public List<int> idsMixed = new List<int>();
        [SerializeField] List<int> idsUsed = new List<int>();
        // public List<CardGame16> cardGamesUsed = new List<CardGame16>();
        public CardGame16 lastCardGame, currentCardGame;
        public int countCardOpen;
        [SerializeField] private bool isCanPlay = true;

        [SerializeField] ParticleSystem card1Glow, card2Glow;
        [SerializeField] UiWinLose uiWinLose;
        private int currentLevel;

        public GameObject settingButton;
        void Start()
        {
            camera = Camera.main;
            StartCoroutine(SetUpCardGame());
            currentLevel = PlayerPrefs.GetInt("currentLevel");
            PlayerPrefs.SetInt("currentLevel", currentLevel + 1);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && isCanPlay && GlobalData.isInGame)
            {
                Vector3 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
                hit = Physics2D.Raycast(mousePosition, Vector3.forward, Mathf.Infinity);
                if (hit.collider != null)
                {
                    CardGame16 go = hit.collider.gameObject.GetComponent<CardGame16>();
                    if (go == null) return;
                    countCardOpen++;
                    if (countCardOpen == 1)
                    {
                        lastCardGame = go;
                        lastCardGame.DisableBoxCollider();
                        CardSelected(lastCardGame);
                    }
                    else if (countCardOpen == 2)
                    {
                        currentCardGame = go;
                        isCanPlay = false;
                        currentCardGame.DisableBoxCollider();
                        CardSelected(currentCardGame);
                        DOVirtual.DelayedCall(0.6f, () => CheckCard());
                    }
                }
            }
            /*if (Input.GetKeyDown(KeyCode.A))
            {
                uiWinLose.ShowLosePanel();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                uiWinLose.ShowWin3Star();
            }*/
        }

        IEnumerator SetUpCardGame()
        {
            yield return new WaitForSeconds(.3f);
            InsCard();
            yield return new WaitForSeconds(.1f);
            InsCard();
            yield return new WaitForSeconds(.1f);
            posToShow = SavePositonGame16.instance.LoadLevelPositionGame16(currentLevel);
            
            for (int i = 0; i < AmountCardLoad(currentLevel) * 2; i++)
            {
                idsMixed.Add(i);
            }

            yield return new WaitForSeconds(.1f);
            idsUsed = Duck.GenerateDerangement(idsMixed);
            yield return new WaitForSeconds(.1f);
            for (int i = 0; i < idsUsed.Count; i++)
            {
                CardGame16 cardTemp = cardGames[i];
                cardGames[i] = cardGames[idsUsed[i]];
                cardGames[idsUsed[i]] = cardTemp;
            }

            yield return new WaitForSeconds(.1f);
            for (int i = 0; i < cardGames.Count; i++)
            {
                // var a = i;
                cardGames[i].transform.DOMove(posToShow[i], 0.7f).SetDelay(i * 0.1f).SetEase(Ease.OutQuart);
            }
        }

        public void InsCard()
        {
            for (int i = 0; i < AmountCardLoad(currentLevel); i++)
            {
                CardGame16 card = Instantiate(cardGame, transform.position, Quaternion.identity)
                    .GetComponent<CardGame16>();
                card.SetSpriteAndId(i);
                card.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                card.transform.SetParent(transform);
                cardGames.Add(card);
                card.transform.position = new Vector3(0, 20, 0);
                card.transform.name = "card_" + i;
            }
        }

        private void CardSelected(CardGame16 card)
        {
            card.transform.DORotate(new Vector3(0, 180, 0), 0.5f).OnUpdate(() =>
            {
                if (card.transform.rotation.eulerAngles.y > 90)
                {
                    card.transform.localScale += new Vector3(0.001f, 0.001f, 0.001f);
                    card.GetComponent<SpriteRenderer>().sortingOrder = 3;
                }
            }).OnComplete(() => { card.transform.DOScale(0.8f, 0.2f); });
        }

        private void HideCards(CardGame16 card)
        {
            card.transform.DORotate(new Vector3(0, 0, 0), 0.5f).OnUpdate(() =>
            {
                if (card.transform.rotation.eulerAngles.y < 90)
                {
                    card.GetComponent<SpriteRenderer>().sortingOrder = 0;
                }
            }).OnComplete(() =>
            {
                card.EnableBoxCollider();
                isCanPlay = true;
                currentCardGame = null;
                lastCardGame = null;
                countCardOpen = 0;
            });
        }

        private void CheckCard()
        {
            if (lastCardGame.id == currentCardGame.id)
            {
                card1Glow.transform.position = lastCardGame.transform.position;
                card2Glow.transform.position = currentCardGame.transform.position;
                Duck.PlayParticle(card1Glow);
                Duck.PlayParticle(card2Glow);
                MoveCard(lastCardGame);
                MoveCard(currentCardGame);
            }
            else
            {
                HideCards(lastCardGame);
                HideCards(currentCardGame);
            }
        }

        public void MoveCard(CardGame16 card)
        {
            if (lastCardGame.transform.position.x > 0 && currentCardGame.transform.position.x > 0)
            {
                CardMoveLeftRight(card);
            }
            else if (lastCardGame.transform.position.x < 0 && currentCardGame.transform.position.x < 0)
            {
                CardMoveLeftRight(card);
            }
            else
            {
                CardMovement(lastCardGame);
                CardMovement(currentCardGame);
            }
        }

        private void CardMoveLeftRight(CardGame16 card)
        {
            lastCardGame.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
            currentCardGame.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);

            lastCardGame.transform.DOMove(new Vector3(1.25f, 0, 0), 0.5f).SetDelay(1).SetEase(Ease.Linear).OnComplete(
                () =>
                {
                    lastCardGame.transform.DOMove(new Vector3(0f, -15, 0), 1f).SetDelay(0.5f).OnComplete((() =>
                    {
                        Reset(card);
                    }));
                });
            currentCardGame.transform.DOMove(new Vector3(-1.25f, 0, 0), 0.5f).SetDelay(1).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    currentCardGame.transform.DOMove(new Vector3(0f, -15, 0), 1f).SetDelay(0.5f).OnComplete((() =>
                    {
                        Reset(card);
                    }));
                });
        }

        private void Reset(CardGame16 card)
        {
            cardGames.Remove(card);
            currentCardGame = null;
            lastCardGame = null;
            countCardOpen = 0;
            isCanPlay = true;
            if (cardGames.Count <= 0)
            {
                settingButton.gameObject.SetActive(false);
                uiWinLose.ShowWin3Star();
            }
        }

        private void CardMovement(CardGame16 card)
        {
            card.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
            if (card.transform.position.x > 0)
            {
                card.transform.DOMove(new Vector3(1.25f, 0, 0), 0.5f).SetDelay(1).SetEase(Ease.Linear).OnComplete(() =>
                {
                    card.transform.DOMove(new Vector3(0f, -15, 0), 1).SetDelay(0.5f).OnComplete((() =>
                    {
                        Reset(card);
                    }));
                });
            }
            else
            {
                card.transform.DOMove(new Vector3(-1.25f, 0, 0), 0.5f).SetDelay(1).SetEase(Ease.Linear).OnComplete(() =>
                {
                    card.transform.DOMove(new Vector3(0f, -15, 0), 1).SetDelay(0.5f).OnComplete((() =>
                    {
                        Reset(card);
                    }));
                });
            }
        }

        public int AmountCardLoad(int level)
        {
            switch (level + 1)
            {
                case 0:
                case 1:
                    return 3;
                case 2:
                    return 4;
                case 3:
                    return 5;
                case 4:
                    return 7;
                default:
                    return 10;
            }
        }
    }
}