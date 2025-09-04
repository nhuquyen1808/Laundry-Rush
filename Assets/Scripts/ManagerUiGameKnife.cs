using DG.Tweening;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class ManagerUiGameKnife : MonoBehaviour
    {

        [SerializeField] List<Image> botIcons = new List<Image>();
        [SerializeField] List<Image> playerIcons = new List<Image>();
        [SerializeField] List<Image> waveIcon = new List<Image>();
        [SerializeField] List<Image> botShadow = new List<Image>();
        [SerializeField] List<Image> playerShadow = new List<Image>();

        [SerializeField] private Sprite shadowWave2, shadowWave3, iconWave2, iconWave3;

        [SerializeField] private GameObject losePanel;
        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject finishText;
        
        public void DisableItemUsed(int id, bool isBot)
        {
            if (isBot)
            {
                for (int i = 0; i < botIcons.Count; i++)
                {
                    if (botIcons[i].transform.name == $"bot{id}" )
                    {
                        botIcons[i].enabled = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < playerIcons.Count; i++)
                {
                    if (playerIcons[i].transform.name == $"player{id}")
                    {
                        playerIcons[i].enabled = false;
                    }
                }
            }
        }

        public void Reset(int level)
        {
            for (int i = 0; i < botShadow.Count; i++)
            {
                if (level == 2) 
                { 
                    botShadow[i].sprite = shadowWave2; 
                }
                else if (level == 3) 
                { 
                    botShadow[i].sprite = shadowWave3;
                }
            }
            for (int i = 0; i < playerShadow.Count; i++)
            {
                if (level == 2)
                { 
                    playerShadow[i].sprite = shadowWave2;
                }
                else if (level == 3)
                {
                    playerShadow[i].sprite = shadowWave3;
                }
            }
            for (int i = 0; i < playerIcons.Count; i++)
            {
                if (level == 2) 
                {
                    playerIcons[i].sprite = iconWave2; 
                }
                else if (level == 3) 
                {
                    playerIcons[i].sprite = iconWave3;
                }
            }
            for (int i = 0; i < botIcons.Count; i++)
            {
                if (level == 2) 
                {
                    botIcons[i].sprite = iconWave2; 
                }
                else if (level == 3) 
                {
                    botIcons[i].sprite = iconWave3; 
                }
            }
            ShowListObjUi(playerIcons);
            ShowListObjUi(botIcons);
        }

        public void DisableWaveIcon(int id)
        {
            for (int i = 0; i < waveIcon.Count; i++)
            {
                if (waveIcon[i].transform.name == $"Wave{id}" && waveIcon[i].gameObject.activeSelf == true)
                {
                    waveIcon[i].DOFade(0, 0.3f).SetEase(Ease.InBack);
                }
            }
        }

        public void ShowListObjUi(List<Image> _list)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                _list[i].gameObject.SetActive(true);
                _list[i].enabled = true;
                Sequence showItemUI = DOTween.Sequence();
                showItemUI.AppendInterval(i * 0.1f);
                showItemUI.Append(_list[i].transform.DOScale(new Vector3(0.55f, 0.55f, 0.55f), 0.2f).SetEase(Ease.OutBack));
                showItemUI.Append(_list[i].DOFade(1, 0.2f).SetEase(Ease.OutBack));
            }
        }

        public void ShowWinPanel()
        {
            finishText.transform.DOScale(1,0.3f).SetDelay(0.4f) .From(0).SetEase(Ease.OutBack).OnComplete(()=> { winPanel.SetActive(true); });
        }

        public void ShowLosePanel()
        {
            finishText.transform.DOScale(1,0.3f) .SetDelay(0.4f).From(0).SetEase(Ease.OutBack).OnComplete(()=> { losePanel.SetActive(true); });
        }
        
    }
}
