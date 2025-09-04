using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DevDuck
{
    public class UiWinLose : MonoBehaviour
    {
        [Header("WIN : ")] public GameObject winPanel;
        public Image star1, star2, star3;
        public Image glowSmall, glowLarger, lightEffect;
        public Image ribbonWin;
        public ParticleSystem starExplosion1, starExplosion2, starExplosion3, confetiBlashLeft, confetiBlashRight;

        [Header("LOSE : ")] public GameObject losePanel;
        public Image star1Lose, star2Lose, star3Lose;
        public Image leftPieceStar, rightPieceStar;
        public ParticleSystem smokeStarBroken;
        public Image ribbonLose;

        [Header("Panel Get Coin")] public Button getCoinAdButton;
        public Button nextButton, replayButton;
        public TextMeshProUGUI coinTextGet;
        public Image arrowAds;
        public Animator arrowAdsAnim;
        public List<GameObject> objWinPanel = new List<GameObject>();
        [Header("Effect get Coin")] public EffectGetCoinRemake effectGetCoinRemake;
        int coin;
        public GameObject panelGetCoin;

        private void Start()
        {
            nextButton.onClick.AddListener(OnClickNextButton);
            getCoinAdButton.onClick.AddListener(OnClickGetCoinAdButton);
            replayButton.onClick.AddListener(OnClickReplayButton);

            Observer.AddObserver(EventAction.EVENT_GET_COINS, UpdateCoinText);
          //  panelGetCoin.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Observer.RemoveObserver(EventAction.EVENT_GET_COINS, UpdateCoinText);
        }

        private void UpdateCoinText(object obj)
        {
            coin = (int)obj;
            coinTextGet.text = coin.ToString();
        }

        private void OnClickReplayButton()
        {
            arrowAdsAnim.enabled = false;
            Scene scene = SceneManager.GetActiveScene();
            GlobalData.isInGame = true;
            DOTween.KillAll();
            ManagerSceneDuck.ins.LoadScene(scene.name);
        }

        private void OnClickGetCoinAdButton()
        {
            PlayerPrefs.SetInt(PlayerPrefsManager.A_GAME_COMEPLETED,1);
            PlayerPrefs.SetInt(PlayerPrefsManager.UNLOCK_NEW_THEME,1);
            getCoinAdButton.GetComponent<Image>().raycastTarget = false;
            nextButton.GetComponent<Image>().raycastTarget = false;
            arrowAdsAnim.enabled = false;
            float x = arrowAds.rectTransform.localPosition.x;
            if (x > -100 && x < 100)
            {
                Observer.Notify(EventAction.EVENT_GET_COINS, 600);
                effectGetCoinRemake.RewardParentCoin(60, 10, LoadScene);
            }
            else if (x >= 274 || x <= -274)
            {
                Observer.Notify(EventAction.EVENT_GET_COINS, 200);
                effectGetCoinRemake.RewardParentCoin(20, 10, LoadScene);
            }
            else
            {
                Observer.Notify(EventAction.EVENT_GET_COINS, 400);
                effectGetCoinRemake.RewardParentCoin(40, 10, LoadScene);
            }

            GlobalData.isInGame = true;
        }


        private void OnClickNextButton()
        {
            PlayerPrefs.SetInt(PlayerPrefsManager.A_GAME_COMEPLETED,1);
            PlayerPrefs.SetInt(PlayerPrefsManager.UNLOCK_NEW_THEME,1);
            getCoinAdButton.GetComponent<Image>().raycastTarget = false;
            nextButton.GetComponent<Image>().raycastTarget = false;
            nextButton.transform.DOScale(0.95f, 0.2f).OnComplete((() =>
            {
                nextButton.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
                GlobalData.isInGame = true;
            }));
            arrowAdsAnim.enabled = false;
            //  Observer.Notify(EventAction.EVENT_GET_COINS, coin);
            effectGetCoinRemake.RewardParentCoin(coin / 10, 10, LoadScene);
            // coinTextGet.text = coin.ToString();
        }

        public void LoadScene()
        {
            DOTween.KillAll();
            ManagerSceneDuck.ins.LoadScene("SceneMenu");
            // SceneManager.LoadSceneAsync("SceneMenu");
        }

        public void UnLockArea()
        {
            int index = PlayerPrefs.GetInt(PlayerPrefsManager.AREA_UNLOCK);
            PlayerPrefs.SetInt(PlayerPrefsManager.AREA_UNLOCK, index + 1);
        }

        public void ShowWin3Star()
        {
            PlayerPrefs.SetInt(PlayerPrefsManager.GET_3_STARS_A_GAME,1);
            GlobalData.isInGame = false;
          //  AudioManager.instance.PlaySound("Win");
            panelGetCoin.gameObject.SetActive(true);
            UnLockArea();
            replayButton.gameObject.SetActive(false);
            winPanel.SetActive(true);
            Sequence sequence = DOTween.Sequence();
            star1.rectTransform.DOAnchorPos(new Vector2(-140, -191), 1f);
            star1.transform.DORotate(new Vector3(0, 0, 731.7f), 1f, RotateMode.FastBeyond360).OnComplete(() =>
            {
                Duck.PlayParticle(starExplosion1);
            });
            star2.rectTransform.DOAnchorPos(new Vector2(0, -171), 1f).SetDelay(0.5f);
            star2.transform.DORotate(new Vector3(0, 0, -720), 1f, RotateMode.FastBeyond360).SetDelay(0.5f).OnComplete(
                () =>
                {
                    Duck.PlayParticle(starExplosion2);
                    ShowElemtsWinUi();
                });
            star3.rectTransform.DOAnchorPos(new Vector2(144, -192), 1f).SetDelay(1f);
            star3.transform.DORotate(new Vector3(0, 0, -728.7f), 1f, RotateMode.FastBeyond360).SetDelay(1f).OnComplete(
                () =>
                {
                    Duck.PlayParticle(starExplosion3);
                    Duck.PlayParticle(confetiBlashLeft);
                    Duck.PlayParticle(confetiBlashRight);
                });

            ribbonWin.transform.DOScale(1, 0.3f).SetDelay(0.5f).From(0);
            glowLarger.transform.DOScale(1.2f, 0.3f).SetDelay(0.5f).From(0);
            glowSmall.transform.DOScale(1, 0.3f).SetDelay(0.5f).From(0);
            lightEffect.transform.DOScale(1.25f, 0.3f).SetDelay(0.5f).From(0);

            glowLarger.transform.DOScale(1.2f, 0.4f).SetDelay(1f).From(0).OnComplete(() =>
            {
                glowLarger.transform.DOScale(1.1f, 0.4f).SetLoops(-1, LoopType.Yoyo);
            });
            glowSmall.transform.DOScale(1, 0.3f).SetDelay(1f).From(0).OnComplete(() =>
            {
                glowSmall.transform.DOScale(0.9f, 0.3f).SetLoops(-1, LoopType.Yoyo);
            });
        }

        public void ShowWin2Star()
        {
            GlobalData.isInGame = false;
          //  AudioManager.instance.PlaySound("Win");
            panelGetCoin.gameObject.SetActive(true);
            UnLockArea();
            replayButton.gameObject.SetActive(false);
            winPanel.SetActive(true);
            Sequence sequence = DOTween.Sequence();
            star1.rectTransform.DOAnchorPos(new Vector2(-140, -191), 1f);
            star1.transform.DORotate(new Vector3(0, 0, 731.7f), 1f, RotateMode.FastBeyond360).OnComplete(() =>
            {
                Duck.PlayParticle(starExplosion1);
                ShowElemtsWinUi();
            });
            star2.rectTransform.DOAnchorPos(new Vector2(0, -171), 1f).SetDelay(0.5f);
            star2.transform.DORotate(new Vector3(0, 0, -720), 1f, RotateMode.FastBeyond360).SetDelay(0.5f).OnComplete(
                () =>
                {
                    Duck.PlayParticle(starExplosion2);
                    Duck.PlayParticle(confetiBlashLeft);
                    Duck.PlayParticle(confetiBlashRight);
                });

            ribbonWin.transform.DOScale(1, 0.3f).SetDelay(0.5f).From(0);
            glowLarger.transform.DOScale(1.2f, 0.3f).SetDelay(0.5f).From(0);
            glowSmall.transform.DOScale(1, 0.3f).SetDelay(0.5f).From(0);
            lightEffect.transform.DOScale(1.25f, 0.3f).SetDelay(0.5f).From(0);

            glowLarger.transform.DOScale(1.2f, 0.4f).SetDelay(1f).From(0).OnComplete(() =>
            {
                glowLarger.transform.DOScale(1.1f, 0.4f).SetLoops(-1, LoopType.Yoyo);
            });
            glowSmall.transform.DOScale(1, 0.3f).SetDelay(1f).From(0).OnComplete(() =>
            {
                glowSmall.transform.DOScale(0.9f, 0.3f).SetLoops(-1, LoopType.Yoyo);
            });
        }

        public void ShowLosePanel()
        {
            GlobalData.isInGame = false;
          //  AudioManager.instance.PlaySound("Lose");
            panelGetCoin.gameObject.SetActive(true);
            losePanel.SetActive(true);
            replayButton.gameObject.SetActive(true);
            ribbonLose.transform.DOScale(1, 0.3f).SetDelay(0.5f).From(0);
            star1Lose.rectTransform.DOAnchorPos(new Vector2(-140, 100), 1f);
            star1Lose.transform.DORotate(new Vector3(0, 0, 731.7f), 1f, RotateMode.FastBeyond360);
            star2Lose.rectTransform.DOAnchorPos(new Vector2(0, 120), 1f).SetDelay(0.5f);
            star2Lose.transform.DORotate(new Vector3(0, 0, 710), 1f, RotateMode.FastBeyond360).SetDelay(0.5f)
                .OnStart((() => ShowElemtsWinUi()));
            star3Lose.rectTransform.DOAnchorPos(new Vector2(144, 100), 1f).SetDelay(1f);
            star3Lose.transform.DORotate(new Vector3(0, 0, -728.7f), 1f, RotateMode.FastBeyond360).SetDelay(1f)
                .OnComplete(() =>
                {
                    Duck.PlayParticle(smokeStarBroken);
                    leftPieceStar.gameObject.SetActive(true);
                    rightPieceStar.gameObject.SetActive(true);
                    star2Lose.gameObject.SetActive(false);
                    rightPieceStar.transform.DOJump(rightPieceStar.transform.position + new Vector3(300, -2500), 1100,
                        1,
                        1.5f).SetDelay(0.3f);
                    star3Lose.transform.DOJump(star3Lose.transform.position + new Vector3(350, -2500), 1100, 1,
                        1.5f).SetDelay(0.5f);
                });
        }

        IEnumerator ShowNextButton()
        {
            yield return new WaitForSeconds(2f);
            nextButton.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
        }

        public void ShowElemtsWinUi()
        {
            for (int i = 0; i < objWinPanel.Count; i++)
            {
                var a = i;
                objWinPanel[a].transform.DOScale(1, 0.3f).SetDelay(a * 0.15f).From(0);
            }

            StartCoroutine(ShowNextButton());
        }
    }
}