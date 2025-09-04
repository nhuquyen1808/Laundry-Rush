using DevDuck;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace dinhvt.Level18
{
    public class GameManager : MonoBehaviour
    {
        public List<Slot> slots;
        public List<Block> blockList;

        public int score = 0;
        public DataLoader dataLoader;

        public static GameManager instance;

        private int listLength;
        private int count = 0;
        private const int startSortingOrder = 5;

        public UiWinLose UiWinLose;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }
        }

        private void Start()
        {
            listLength = slots.Count;
            blockList = new List<Block>();
        }

        public void SelectBlock(Block block)
        {
            if (IsFullSlot()) return;

            blockList.Add(block);
            List<Block> tempB = new List<Block>();
            int c = blockList.Count;
            while (tempB.Count < c)
            {
                tempB.Add(blockList[0]);
                blockList.RemoveAt(0);
                for (int i = 0; i < blockList.Count; ++i)
                {
                    if (tempB[tempB.Count - 1].id == blockList[i].id)
                    {
                        tempB.Add(blockList[i]);
                        blockList.RemoveAt(i);
                        --i;
                    }
                }
            }

            blockList = tempB;

            for (int i = 0; i < blockList.Count; i++)
            {
                if (blockList[i] == block) continue;
                blockList[i].MoveToSlot(slots[i], startSortingOrder + i);
            }

            int index = blockList.IndexOf(block);
            block.AddToSlot(slots[index], startSortingOrder + index, () => { UpdateBlockCount(block); });
        }


        public void UpdateBlockCount(Block block)
        {
            count++;
            if (blockList.Count < 3) return;

            List<Block> blocksToDisable = new List<Block>();
            for (int i = 1; i < blockList.Count - 1; i++)
            {
                if (blockList[i - 1].id == blockList[i].id && blockList[i].id == blockList[i + 1].id)
                {
                    if (blockList[i - 1].isMoving || blockList[i].isMoving || blockList[i + 1].isMoving) continue;

                    count -= 3;
                    // Anim
                    blockList[i].DisableAnim(blockList[i]);
                    blockList[i - 1].DisableAnim(blockList[i], () =>
                    {
                        DoRemoveVisual(blocksToDisable);
                        score += 3;
                        CheckWinGame();
                    });
                    blockList[i + 1].DisableAnim(blockList[i]);

                    blocksToDisable.Add(blockList[i - 1]);
                    blocksToDisable.Add(blockList[i]);
                    blocksToDisable.Add(blockList[i + 1]);

                    foreach (Block b in blocksToDisable)
                    {
                        blockList.Remove(b);
                    }
                }
            }

            CheckGameOver();
        }

        private void CheckGameOver()
        {
            if (count == listLength)
            {
                UiWinLose.ShowLosePanel();
                Debug.Log("GAME OVER");
            }
        }

        public void CheckWinGame()
        {
            if (score == dataLoader.numOfItems)
            {
                PlayerPrefs.SetInt(GameConfig.CURRENT_LEVEL, PlayerPrefs.GetInt(GameConfig.CURRENT_LEVEL, 0) + 1);
                PlayerPrefs.Save();
                UiWinLose.ShowWin3Star();
                Debug.Log("WIN");
            }
        }

        public void DoRemoveVisual(List<Block> blocksToDisable)
        {
            foreach (Block b in blocksToDisable)
            {
                blockList.Remove(b);
            }

            for (int j = 0; j < blockList.Count; j++)
            {
                blockList[j].MoveToSlot(slots[blockList.IndexOf(blockList[j])], startSortingOrder + j);
            }
        }


        public bool IsFullSlot()
        {
            return blockList.Count == listLength;
        }
    }
}