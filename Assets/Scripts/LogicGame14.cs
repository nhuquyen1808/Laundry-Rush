using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class LogicGame14 : MonoBehaviour
    {
        RaycastHit2D hit;
        RaycastHit2D[] hits;
        Camera cam;
        [SerializeField] List<Vector3> defaultPositons = new List<Vector3>();
        [SerializeField] List<PieceGame14Below> pieceGame14Below = new List<PieceGame14Below>();
        [SerializeField] List<PieceGame14Below> pieceGame14Below2 = new List<PieceGame14Below>();
        [SerializeField] List<PieceGame14Below> pieceGame14Below3 = new List<PieceGame14Below>();
        [SerializeField] List<PieceGame14Upper> pieceGame14Upper = new List<PieceGame14Upper>();
        private PieceGame14Upper currentPiece;
        private Vector3 tempPos;
        bool isDragging = false;
        float timer = 0f, minDistance;
        private int posMin;
        private int level = 1;
        [Header("UI Elements : ")] public Button previewButton;
        public Image previewImage;
        bool isPreviewing = false;

        [HideInInspector] public List<int> id = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        [HideInInspector] public List<int> idPlay = new List<int>();
        [SerializeField] GameObject magazineShadow1, magazineShadow2, magazineShadow3, magazinePlay;
        public int _countPiecesRemaining = 9;
        [SerializeField] ObjectPool objectPool;
        public ParticleSystem lightningParticles;
        [SerializeField] private GameObject fireWorkParticles;
        [SerializeField] LogicUiGame14 uiGame14;
        [SerializeField] UiWinLose uiWinLose;

        public List<PositionGame14> positionGame14s = new List<PositionGame14>();
        
        void Start()
        {
            cam = Camera.main;
            previewButton.onClick.AddListener(OnClickPreviewButton);
            objectPool.SetupPool();
            GetDefaultPosition();
            SetDataGame(1);
            uiGame14.AnimStartGame();
            // ShowPlayDone();
        }

        private void SetDataGame(int level)
        {
            MixPositionPieceUpper();
            SetPreviewImage(level);
            magazinePlay.transform.localPosition = new Vector3(0, 0, 0);
            for (int i = 0; i < pieceGame14Below.Count; i++)
            {
                pieceGame14Below[i].SetSprite(level);
            }

            for (int i = 0; i < pieceGame14Upper.Count; i++)
            {
                pieceGame14Upper[i].SetSprite(level);
                pieceGame14Upper[i].gameObject.SetActive(true);
                pieceGame14Upper[i].EnableBoxCollider();
            }
        }

        private void MixPositionPieceUpper()
        {
            idPlay = Duck.GenerateDerangement(id);
            for (int j = 0; j < pieceGame14Upper.Count; j++)
            {
                pieceGame14Upper[j].transform.position = defaultPositons[idPlay[j] - 1] + new Vector3(0, 0, -10);
            }
        }

        private void OnClickPreviewButton()
        {
            if (!isPreviewing)
            {
                isPreviewing = true;
                previewImage.rectTransform.DOAnchorPosY(100, .7f).SetEase(Ease.OutBack);
            }
            else
            {
                isPreviewing = false;
                previewImage.rectTransform.DOAnchorPosY(2000, .7f).SetEase(Ease.InBack);
            }
        }

        private void GetDefaultPosition()
        {
            foreach (PieceGame14Below spr in pieceGame14Below)
            {
                defaultPositons.Add(spr.transform.position);
            }
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && uiGame14.isCanInstantiate)
            {
                Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
                hit = Physics2D.Raycast(mousePosition, Vector3.forward);
                if (hit.collider != null)
                {
                    currentPiece = hit.collider.gameObject.GetComponent<PieceGame14Upper>();
                    if (currentPiece == null) return;
                    tempPos = currentPiece.transform.position;
                    isDragging = true;
                    currentPiece.BoxCollider2D.enabled = false;
                    /*currentPiece.transform
                        .DOMove(cam.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 95), 0.07f)
                        .OnComplete(() => isDragging = true);*/
                    
                    
                }
            }

            if (isDragging && currentPiece != null)
            {
                timer += Duck.TimeMod;
                // currentPiece.transform.position = cam.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 95);
                currentPiece.transform.position = Vector3.Lerp(currentPiece.transform.position,
                    cam.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 95), 0.2f);
            }

            if (Input.GetMouseButtonUp(0) && currentPiece != null)
            {
                float distance =
                    Duck.GetDistance(currentPiece.transform.position, defaultPositons[currentPiece.id - 1]);
                Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
                hits = Physics2D.RaycastAll(mousePosition, Vector3.forward);

                if (hits.Length >= 2)
                {
                    Vector2 tempHits1 = hits[1].collider.gameObject.transform.position;
                    PieceGame14Upper piece1 = hits[0].collider.GetComponent<PieceGame14Upper>();
                    PieceGame14Upper piece2 = hits[1].collider.GetComponent<PieceGame14Upper>();
                    hits[0].transform.position = tempHits1;
                    hits[1].collider.gameObject.transform.DOMove(tempPos, 0.2f).OnComplete(() =>
                    {
                        if (Duck.GetDistance(piece1.transform.position, defaultPositons[piece1.id - 1]) < 1.4f)
                        {
                            piece1.HidePiece();
                            CheckGameOver();
                            PlayStarParticle(piece1.gameObject);
                            ChangeStatePositon(defaultPositons[piece1.id - 1]);
                            piece1 = null;
                        }
                        else
                        {
                            piece1.ShowFaild();
                            piece1.BoxCollider2D.enabled = true;
                        }
                        if (Duck.GetDistance(piece2.transform.position, defaultPositons[piece2.id - 1]) < 1.4f)
                        {
                            piece2.HidePiece();
                            CheckGameOver();
                            PlayStarParticle(piece2.gameObject);
                            ChangeStatePositon(defaultPositons[piece2.id - 1]);
                            piece2 = null;
                        }
                        else
                        {
                            piece2.ShowFaild();
                            piece2.BoxCollider2D.enabled = true;
                        }
                    });
                }
                else
                {
                    if (distance < 1.4f)
                    {
                        ChangeStatePositon( defaultPositons[currentPiece.id - 1]);
                        currentPiece.transform.position = defaultPositons[currentPiece.id - 1] + new Vector3(0, 0, -10);
                        currentPiece.HidePiece();
                        PlayStarParticle(currentPiece.gameObject);
                        CheckGameOver();
                    }
                    else
                    {
                        ChangeStatePositon(currentPiece.transform.position);
                        GetListDistance();
                        GetMinDistance(currentPiece);
                        currentPiece.gameObject.transform
                            .DOMove(positionGame14s[ idPosEmpty[posMin]].transform.position + new Vector3(0, 0, -5), 0.2f)
                            .SetEase(Ease.Linear);
                        currentPiece.ShowFaild();
                        currentPiece.BoxCollider2D.enabled = true;
                        
                    }
                }

                isDragging = false;
                currentPiece = null;
            }
        }
        List<float> distances = new List<float>();
        List<int> idPosEmpty = new List<int>();
        public void GetListDistance()
        {
            distances.Clear();
            idPosEmpty.Clear();
            for (int i = 0; i < positionGame14s.Count; i++)
            {
                if (!positionGame14s[i].isHasPiece)
                {
                   distances.Add(Duck.GetDistance(currentPiece.transform.position, positionGame14s[i].transform.position));
                   idPosEmpty.Add(positionGame14s[i].id);
                   Debug.Log("pos added :     "  + positionGame14s[i].id);
                }
            }
        }

        public float GetMinDistance(PieceGame14Upper piece)
        {
            posMin = 0;
            minDistance = distances[0];

            if (distances.Count == 1)
            {
                
                posMin = 0;
            }
            else
            {
                for (int i = 0; i < distances.Count; i++)
                {
                    if (distances[i] <= minDistance)
                    {
                        minDistance = distances[i];
                        posMin = positionGame14s[i].id;
                        Debug.Log(positionGame14s[i].id  + "     ////////////////");
                    }
                }
            }
            return posMin;
        }

        public void SetPreviewImage(int level)
        {
            previewImage.sprite = Resources.Load<Sprite>($"Art/game14/magazine_{level}");
        }

        public void ShowPlayDone()
        {
            Duck.PlayParticle(lightningParticles);
            fireWorkParticles.gameObject.SetActive(true);
            for (int i = 0; i < pieceGame14Below.Count; i++)
            {
                pieceGame14Below[i].TurnOffShadow();
            }

            if (level == 2)
            {
                pieceGame14Below.Clear();
                pieceGame14Below = pieceGame14Below2;
            }
            else
            {
                pieceGame14Below.Clear();
                pieceGame14Below = pieceGame14Below3;
            }
        }

        public void CheckGameOver()
        {
            _countPiecesRemaining--;
            if (_countPiecesRemaining == 0 && level < 4)
            {
                level++;
                DOVirtual.DelayedCall(1, () => ShowPlayDone());
                HideMagazine(level);
                if (level == 4)
                {
                    Debug.Log("Show Win game");
                    uiWinLose.ShowWin3Star();
                }
            }
        }

        public void HideMagazine(int level)
        {
            _countPiecesRemaining = 9;
            if (level == 2)
            {
                magazineShadow1.transform.DOMoveX(magazineShadow1.transform.position.x + 10, 1f).SetEase(Ease.OutBack)
                    .SetDelay(3f);
                magazineShadow2.transform.DOLocalMove(new Vector3(0, 0, 10), 1f).SetEase(Ease.OutBack).SetDelay(3f)
                    .OnComplete(() => SetDataGame(level)).OnStart(() =>
                    {
                        fireWorkParticles.gameObject.SetActive(false);
                        lightningParticles.Stop();
                    });
            }
            else if (level == 3)
            {
                magazineShadow2.transform.DOMoveX(magazineShadow1.transform.position.x + 10, 1f).SetEase(Ease.OutBack)
                    .SetDelay(3f);
                magazineShadow3.transform.DOLocalMove(new Vector3(0, 0, 10), 1f).SetEase(Ease.OutBack).SetDelay(3f)
                    .OnComplete(() => { SetDataGame(level); }).OnStart(() =>
                    {
                        fireWorkParticles.gameObject.SetActive(false);
                        lightningParticles.Stop();
                    });
            }
        }

        public void PlayStarParticle(GameObject ob)
        {
            PooledObject o = objectPool.GetPooledObject(ob.transform.position, new Quaternion(-90, 0, 0, 0));
            Duck.PlayParticle(o.GetComponent<ParticleSystem>());
            StartCoroutine(ReleaseParticles(o));
        }

        IEnumerator ReleaseParticles(PooledObject pooledObject)
        {
            yield return new WaitForSeconds(2f);
            pooledObject.ReturnToPool();
        }

        public void ChangeStatePositon(Vector3 piece)
        {
            for (int i = 0; i < positionGame14s.Count; i++)
            {
               positionGame14s[i].HitSomething();
                /*if (Duck.GetDistance(positionGame14s[i].transform.position, piece) < 0.1f)
                {
                    Debug.Log("Cai deo gi 222222222");

                    positionGame14s[i].isHasPiece = true;
                }*/
            }
        }
    }
}