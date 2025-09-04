using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dinhvt.Level18
{
    public class GridBuilder : MonoBehaviour
    {
        public Transform center;
        public List<Grid> grids;

        public static GridBuilder instance;

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
            //GenerateGrids();
            //CheckOverlap();
        }


        public void GenerateGrids()
        {
            for (int i = 0; i < grids.Count; i++)
            {
                grids[i].GenerateBlocks(center, i);
            }

            //CheckOverlap();
            CheckBlockOverlap();
        }

        //public void CheckOverlap()
        //{
        //    for (int i = 0; i < grids.Count - 1; i++)
        //    {
        //        CheckBlockOverlap(i);
        //    }
        //}

        //public void CheckBlockOverlap(int index)
        //{
        //    foreach (Block block in grids[index].blockList)
        //    {
        //        block.CheckOverlap();
        //    }
        //}

        public void CheckBlockOverlap()
        {
            for (int i = 0; i < grids.Count - 1; i++)
            {
                foreach (Block block in grids[i].blockList)
                {
                    block.upperBlocks.Clear();
                    for (int j = i + 1; j < grids.Count; j++)
                    {
                        foreach (Block upperBlock in grids[j].blockList)
                        {
                            if (HelperUtilities.IsOverLapping(block.col2D, upperBlock.col2D))
                            {
                                block.upperBlocks.Add(upperBlock);
                            }
                        }

                        if (block.upperBlocks.Count == 0)
                        {
                            block.canSelect = true;
                            block.spriteRenderer.color = block.visibleColor;
                        }
                        else
                        {
                            block.canSelect = false;
                            block.spriteRenderer.color = block.invisibleColor;
                        }
                    }

                    
                }
            }

            foreach (Block block in grids[grids.Count - 1].blockList)
            {
                block.canSelect = true;
                block.spriteRenderer.color = block.visibleColor;
            }
        }

        public void UpdateBlockOverlap()
        {
            foreach (Grid grid in grids)
            {
                foreach (Block block in grid.blockList)
                {
                    if (block.isSelected) continue;

                    bool isVisible = true;
                    foreach (Block upperBlock in block.upperBlocks)
                    {
                        if (!upperBlock.isSelected) isVisible = false;
                    }

                    if (isVisible)
                    {
                        block.canSelect = true;
                        block.spriteRenderer.color = block.visibleColor;
                    }
                    else
                    {
                        block.canSelect = false;
                        block.spriteRenderer.color = block.invisibleColor;
                    }
                }
            }
        }

        public void ScaleAndCheckOverlap()
        {
            StartCoroutine(ScaleAndCheckOverlapCoroutine());
        }

        IEnumerator ScaleAndCheckOverlapCoroutine()
        {
            foreach (Grid grid in grids)
            {
                foreach (Block block in grid.blockList)
                {
                    block.transform.DOScale(Vector3.one, 0.1f);
                    yield return new WaitForSeconds(0.02f);
                }
            }

            yield return new WaitForEndOfFrame();

            foreach (Grid grid in grids)
            {
                foreach (Block block in grid.blockList)
                {
                    block.col2D.enabled = true;
                }
            }
            yield return new WaitForEndOfFrame();

            CheckBlockOverlap();
        }
    }
}


