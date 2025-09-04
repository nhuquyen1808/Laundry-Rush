using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DevDuck
{
    public class LogicGame20 : MonoBehaviour
    {
        [SerializeField] private LogicUiGame20 uiGame20;
        public List<SpriteRenderer> spritesLv1 = new List<SpriteRenderer>();
        public List<SpriteRenderer> spritesLv2 = new List<SpriteRenderer>();
        public List<SpriteRenderer> spritesLv3 = new List<SpriteRenderer>();
        public List<SpriteRenderer> spritesLv4 = new List<SpriteRenderer>();
        public List<SpriteRenderer> spritesLv5 = new List<SpriteRenderer>();
        public List<SpriteRenderer> spritesLvUsed = new List<SpriteRenderer>();
        public List<PieceDragUGame20> pieceDragUGame20 = new List<PieceDragUGame20>();
        public List<Sprite> shadowSprites = new List<Sprite>();
        public List<SpriteRenderer> lightSprites = new List<SpriteRenderer>();
        private List<int> ids = new List<int>();
        private List<int> tempIdsPieceShowed = new List<int>();
        [HideInInspector] public List<int> idsUsed = new List<int>();
        [HideInInspector] public List<int> idPieceRotated = new List<int>();
        public GameObject pieceOnHand;
        bool isOnHand = false;
        public int currentPiece = 0;
        SpriteRenderer hittedPiece;
        RaycastHit2D hit;
        PieceDragUGame20 pieceDragUGame20Current;
        [SerializeField] private ParticleSystem starExplosion;
        int countPieceExist;
        [SerializeField] private UiWinLose uiWinLose;
        public TutorialGame20 tutorialGame20;
        public GameObject nContentBelow;

        public GameObject settingsButton;
        public Vector3 rotZ;
        public  List<PieceDragUGame20> pieceDragUGame20s = new List<PieceDragUGame20>();
        public bool currentPieceRotated = false;
        void Start()
        {
            SetData();
            Observer.AddObserver(EventAction.EVENT_PLAYER_BEGINSELECT, PlayerSelect);
            Observer.AddObserver(EventAction.EVENT_PLAYER_ENDSELECT, PlayerEndSelect);
            uiGame20.AnimStartGame();
            SetUp();
        }

        private void OnDestroy()
        {
            Observer.RemoveObserver(EventAction.EVENT_PLAYER_BEGINSELECT, PlayerSelect);
            Observer.RemoveObserver(EventAction.EVENT_PLAYER_ENDSELECT, PlayerEndSelect);
        }

        private int amountPieceEmpty;
        public void SetData()
        {
            if (GlobalData.isReplay)
            {
                PlayerPrefs.SetInt("currentLevelGame20", PlayerPrefs.GetInt("currentLevelGame20") );
            }
            else
            {
                PlayerPrefs.SetInt("currentLevelGame20", PlayerPrefs.GetInt("currentLevelGame20") + 1);
            }
            GlobalData.isReplay = false;
            switch (PlayerPrefs.GetInt("currentLevelGame20"))
            {
                case 1:
                    spritesLvUsed = spritesLv1;
                    amountPieceEmpty = spritesLv1.Count - 4;
                    
                    break;
                case 2:
                    spritesLvUsed = spritesLv2;
                    amountPieceEmpty = spritesLv2.Count - 5;
                    break;
                case 3:
                    spritesLvUsed = spritesLv3;
                    amountPieceEmpty = spritesLv3.Count - 6;
                    break;
                case 4:
                    spritesLvUsed = spritesLv4;
                    amountPieceEmpty = spritesLv4.Count - 7;
                    break;
                case 5:
                    spritesLvUsed = spritesLv5;
                    amountPieceEmpty = spritesLv5.Count - 8;
                    break;
                default:
                    spritesLvUsed = spritesLv1;
                    amountPieceEmpty = spritesLv1.Count - 10;
                    break;
            }
            countPieceExist = spritesLv1.Count - amountPieceEmpty;
 
        }

        void Update()
        {
            if (isOnHand)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pieceOnHand.transform.position = new Vector3(pos.x, pos.y, 0);
                hit = Physics2D.Raycast(pos, Vector3.forward, Mathf.Infinity);
                pieceOnHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotZ.z));
                if (pieceOnHand.transform.position.y < -5f && !currentPieceRotated)
                {
                    pieceOnHand.transform.GetChild(0).gameObject.SetActive(true);
                }
                if(pieceOnHand.transform.position.y > -5f || currentPieceRotated)
                {
                    pieceOnHand.transform.GetChild(0).gameObject.SetActive(false);
                }
                if (hit.collider == null) return;
                hittedPiece = hit.collider.gameObject.GetComponent<SpriteRenderer>();
                if (currentPiece.ToString() == hittedPiece.name)
                {
                    //   Debug.Log("Have the samme ---------");
                   // hittedPiece.color = new Color32(255, 255, 255, 255);
                    if (!tutorialGame20.isDoneTut20)
                    {
                        PlayerPrefs.SetInt("isDoneTutorial20",1);
                        tutorialGame20.isDoneTut20 = true;
                        tutorialGame20.handImage.gameObject.SetActive(false);
                    }
                }
                /*else
                {
                    //   hittedPiece.color = new Color32(255, 255, 255, 0);
                    for (int i = 0; i < lightSprites.Count; i++)
                    {
                        if (lightSprites[i].name != hittedPiece.name || hittedPiece == null)
                        {
                            lightSprites[i].color = new Color32(255, 255, 255, 0);
                        }
                    }
                }*/
            }
        }

        void SetUp()
        {
            while (tempIdsPieceShowed.Count < amountPieceEmpty)
            {
                int temp = Random.Range(1, 16);
                if (!tempIdsPieceShowed.Contains(temp))
                {
                    tempIdsPieceShowed.Add(temp);
                }
            }

            for (int i = 1; i < 16; i++)
            {
                ids.Add(i);
            }

            idsUsed = Duck.GenerateDerangement(ids);

            for (int i = 0; i < pieceDragUGame20.Count; i++)
            {
                pieceDragUGame20[i].id = idsUsed[i];
            }
            for (int i = 0; i < tempIdsPieceShowed.Count; i++)
            {
                if (idsUsed.Contains(tempIdsPieceShowed[i]))
                {
                    idsUsed.Remove(tempIdsPieceShowed[i]);
                }
            }
            while (idPieceRotated.Count < 3)
            {
                int temp = idsUsed[Random.Range(0, idsUsed.Count)];
                if (!idPieceRotated.Contains(temp))
                {
                    idPieceRotated.Add(temp);
                }
            }

            StartCoroutine(InitPiece());
        }

        IEnumerator InitPiece()
        {
            for (int i = 0; i < pieceDragUGame20.Count; i++)
            {
                if (idsUsed.Contains(pieceDragUGame20[i].id))
                {
                    pieceDragUGame20[i].ShadowImage.sprite = shadowSprites[pieceDragUGame20[i].id - 1];
                    pieceDragUGame20[i].pieceImage.sprite = spritesLvUsed[pieceDragUGame20[i].id - 1].sprite;
                   // if (idPieceRotated.Contains(pieceDragUGame20[i].id))
                    {
                        pieceDragUGame20[i].rotateButton.gameObject.SetActive(true);
                        pieceDragUGame20[i].pieceRotate.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                    }
                }
                else
                {
                    spritesLvUsed[pieceDragUGame20[i].id - 1].gameObject.SetActive(true);
                    spritesLvUsed[pieceDragUGame20[i].id - 1].gameObject.transform.DOScale(1, 0.5f)
                        .SetEase(Ease.OutBack)
                        .From(0);
                    spritesLvUsed[pieceDragUGame20[i].id - 1].color = new Color32(255, 255, 255, 255);
                    pieceDragUGame20[i].gameObject.SetActive(false);
                }
            }

            yield return new WaitForSeconds(4f);
            if (!tutorialGame20.isDoneTut20)
            {
                string tempId = "";

                for (int i = 0; i < pieceDragUGame20.Count; i++)
                {
                    if (pieceDragUGame20[i].gameObject.activeSelf)
                    {
                        tutorialGame20.handImage.transform.position =
                            pieceDragUGame20[i].transform.position;
                        tutorialGame20.transparentImage.sprite = pieceDragUGame20[i].pieceImage.sprite;
                        tutorialGame20.handImage.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
                        tempId = pieceDragUGame20[i].id.ToString();
                        break;
                    }
                }

                for (int i = 0; i < spritesLvUsed.Count; i++)
                {
                    if (spritesLvUsed[i].name == tempId)
                    {
                        Vector3 startPos = tutorialGame20.handImage.transform.position;
                        tutorialGame20.handImage.transform.DOMove(spritesLvUsed[i].transform.position, 1.5f).OnComplete(
                            () =>
                            {
                                // tutorialGame20.transparentImage.gameObject.SetActive(false);
                                tutorialGame20.handImage.transform.DOMove(startPos, 1.5f).SetLoops(-1, LoopType.Yoyo);
                            });
                    }
                }
            }
        }

        private void PlayerSelect(object obj)
        {
            isOnHand = true;
            currentPiece = (int)obj;
            
            for (int i = 0; i < pieceDragUGame20s.Count; i++)
            {
                if (pieceDragUGame20s[i].id == currentPiece)
                {
                    rotZ  = pieceDragUGame20s[i].ShadowImage.transform.rotation.eulerAngles;
                    currentPieceRotated = pieceDragUGame20s[i].isRotated;
                }
            }
            pieceOnHand.GetComponent<SpriteRenderer>().sprite = spritesLvUsed[currentPiece - 1].sprite;
            pieceOnHand.transform.DOScale(1.1f, 0.1f);
            pieceOnHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotZ.z));
            for (int i = 0; i < pieceDragUGame20.Count; i++)
            {
                if (pieceDragUGame20[i].id == currentPiece)
                {
                    pieceDragUGame20Current = pieceDragUGame20[i];
                }
            }
        }

        private void PlayerEndSelect(object obj)
        {
            isOnHand = false;
            if (pieceDragUGame20Current != null)
            {
                if (hittedPiece != null)
                {
                    if (currentPiece.ToString() == hittedPiece.name && pieceDragUGame20Current.isRotated)
                    {
                        //  Debug.Log("Have the samme");
                        starExplosion.transform.position = spritesLvUsed[currentPiece - 1].transform.position;
                        Duck.PlayParticle(starExplosion);
                        pieceDragUGame20Current.HidePieceImage();
                        spritesLvUsed[currentPiece - 1].gameObject.SetActive(true);
                        spritesLvUsed[currentPiece - 1].transform.DOScale(1f, 0.3f).From(0).SetEase(Ease.OutBack);
                        SpriteRenderer temp = hittedPiece;
                        
                        temp.color = new Color32(255, 255, 255, 255);
                        
                        DOVirtual.DelayedCall(0.2f, () =>
                        {
                            temp.DOFade(0, 0.2f);
                        });
                       // hittedPiece.color = new Color32(255, 255, 255, 0);
                        CheckWin();
                    }
                    else
                    {
                        //  Debug.Log("dONT the samme");
                        pieceDragUGame20Current.ShowPieceImage();
                        if (pieceDragUGame20Current.isRotated)
                        {
                            pieceDragUGame20Current.rotateButton.gameObject.SetActive(false);
                        }
                        else
                        {
                        pieceDragUGame20Current.rotateButton.gameObject.SetActive(true);
                        }
                    }
                }
                else
                {
                    if (pieceDragUGame20Current.pieceRotate.transform.rotation.z > 0)
                    {
                        if (pieceDragUGame20Current.isRotated)
                        {
                            pieceDragUGame20Current.rotateButton.gameObject.SetActive(false);
                        }
                        else
                        {
                            pieceDragUGame20Current.rotateButton.gameObject.SetActive(true);
                        }
                    }

                    pieceDragUGame20Current.ShowPieceImage();
                }
            }

            pieceDragUGame20Current = null;
            hittedPiece = null;
            currentPiece = (int)obj;
            pieceOnHand.GetComponent<SpriteRenderer>().sprite = null;
            pieceOnHand.transform.GetChild(0).gameObject.SetActive(false);
        }

        public void CheckWin()
        {
            countPieceExist--;
            if (countPieceExist == 0)
            {
                settingsButton.gameObject.SetActive(false);
                uiWinLose.ShowWin3Star();
                Observer.Notify(EventAction.EVENT_GET_COINS, 30);
            }
        }
    }
}