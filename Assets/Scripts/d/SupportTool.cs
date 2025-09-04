using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace dinhvt.Level18
{
    public class SupportTool : MonoBehaviour
    {
        public Button shuffleBtn;
        public Button findBtn;
        public Button undoBtn;
        [Space(10)]
        public GridBuilder gridBuilder;
        public GameManager gameManager;
        //public DataLoader dataLoader;


        private void Awake()
        {
            shuffleBtn?.onClick.AddListener(OnShuffleButtonClicked);
            findBtn?.onClick.AddListener(OnFindButtonClicked);
            undoBtn?.onClick.AddListener(OnUndoButtonClicked);
        }

        private void Update()
        {

        }

        private void OnUndoButtonClicked()
        {           
            if (gameManager.blockList.Count == 0) return;

            SetInteractableButtons(false);

            int lastIndex = gameManager.blockList.Count - 1;
            gameManager.blockList[lastIndex].ResetTransform(() => { gridBuilder.CheckBlockOverlap(); SetInteractableButtons(true); });
            gameManager.blockList.RemoveAt(lastIndex);
        }

        private void OnFindButtonClicked()
        {
            SetInteractableButtons(false);

            Dictionary<int, MatchBlock> matchBlocksDict = new Dictionary<int, MatchBlock>();

            foreach (Grid grid in gridBuilder.grids)
            {
                foreach (Block block in grid.blockList)
                {   
                    if (block.gameObject.activeSelf == false) continue;

                    if (!matchBlocksDict.ContainsKey(block.id))
                    {
                        MatchBlock matchBlock = new MatchBlock
                        {
                            visibleBlocks = new List<Block>(),
                            invisibleBlocks = new List<Block>()
                        };
                        matchBlocksDict.Add(block.id, matchBlock);
                    }

                    if (block.canSelect || gameManager.blockList.Contains(block))
                    {
                        matchBlocksDict[block.id].visibleBlocks.Add(block);
                    }
                    else
                    {
                        matchBlocksDict[block.id].invisibleBlocks.Add(block);
                    }
                }
            }

            int key = -1;
            int maxVisibleBlock = -1;
            if (gameManager.blockList.Count != 0)
            {
                foreach (Block block in gameManager.blockList)
                {
                    int numOfVisibleBlocks = matchBlocksDict[block.id].visibleBlocks.Count;
                    if (numOfVisibleBlocks > maxVisibleBlock)
                    {
                        maxVisibleBlock = numOfVisibleBlocks;
                        key = block.id;
                    }
                }
            }
            else
            {
                foreach (var kvp in matchBlocksDict)
                {
                    int numOfVisibleBlocks = kvp.Value.visibleBlocks.Count;
                    if (numOfVisibleBlocks > maxVisibleBlock)
                    {
                        maxVisibleBlock = numOfVisibleBlocks;
                        key = kvp.Key;
                    }
                }
            }

            StartCoroutine(FindCoroutine(matchBlocksDict, key, () => { SetInteractableButtons(true); }));
            
        }

        private void OnShuffleButtonClicked()
        {
            SetInteractableButtons(false);

            List<BlockData> blockSOList = new List<BlockData>();
            foreach (Grid grid in gridBuilder.grids)
            {
                foreach (Block block in grid.blockList)
                {
                    if (gameManager.blockList.Contains(block) || !block.gameObject.activeSelf) continue;

                    BlockData blockSO = new BlockData();
                    blockSO.id = block.id;
                    blockSO.sprite = block.spriteRenderer.sprite;

                    blockSOList.Add(blockSO);
                }
            }

            if (blockSOList.Count == 0) return;


            foreach (Grid grid in gridBuilder.grids)
            {
                foreach (Block block in grid.blockList)
                {
                    if (gameManager.blockList.Contains(block) || !block.gameObject.activeSelf) continue;

                    int randomIndex = UnityEngine.Random.Range(0, blockSOList.Count);


                    BlockData blockSO = blockSOList[randomIndex];
                    block.id = blockSO.id;
                    block.spriteRenderer.sprite = blockSO.sprite;
                    blockSOList.Remove(blockSO);
                } 
            }

            SetInteractableButtons(true);
        }

        public void SetInteractableButtons(bool isInteractable)
        {
            shuffleBtn.interactable = isInteractable;
            findBtn.interactable = isInteractable;
            undoBtn.interactable = isInteractable;
        }

        IEnumerator FindCoroutine(Dictionary<int, MatchBlock> matchBlocksDict, int key, Action action)
        {
            int count = 0;
            foreach (Block block in matchBlocksDict[key].visibleBlocks)
            {
                count++;
                if (block.canSelect) block.OnDeselect();
                yield return new WaitForSeconds(0.2f);
                if (count == 3) break;
            }

            if (count < 3)
            {
                foreach (Block block in matchBlocksDict[key].invisibleBlocks)
                {
                    count++;
                    block.SetCanSelect(true);
                    block.OnDeselect();
                    yield return new WaitForSeconds(0.2f);

                    if (count == 3) break;
                }
            }

            yield return new WaitForSeconds(0.6f);
            action?.Invoke();
        }

        public struct MatchBlock
        {   
            public List<Block> visibleBlocks;
            public List<Block> invisibleBlocks;
        }

        public struct BlockData
        {
            public int id;
            public Sprite sprite;
        }
    }
}
