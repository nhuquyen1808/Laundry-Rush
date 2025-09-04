using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class LevelManagerTrippleTile : MonoBehaviour
    {
        [SerializeField] List<LevelButton> levelButtons = new List<LevelButton>();

        public int currentLevelUnlock;
        
        void Start()
        {
            SetUpId();
            ShowData();
        }

        private void SetUpId()
        {
            for (int i = 0; i < levelButtons.Count; i++)
            {
                levelButtons[i].id = i + 1;
            }
        }

        private void ShowData()
        {
            for (int i = 0; i < levelButtons.Count; i++)
            {
                if (levelButtons[i].id == currentLevelUnlock)
                {
                    levelButtons[i].SetCurrentLevel();
                }
                else if (levelButtons[i].id < currentLevelUnlock)
                {
                    levelButtons[i].SetLevelUnLock();
                }
                else
                {
                    levelButtons[i].SetLevelLock();
                }
            }
        }

    }
}
