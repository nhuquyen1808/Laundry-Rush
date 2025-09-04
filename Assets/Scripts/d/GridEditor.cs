using UnityEditor;
using UnityEngine;

namespace dinhvt.Level18
{
    public class GridEditor : MonoBehaviour
    {   
        public GridBuilder gridBuilder;

        public void CheckBlockOverlap()
        {
            for (int i = 0; i < gridBuilder.grids.Count - 1; i++)
            {
                foreach (Block block in gridBuilder.grids[i].blockList)
                {
                    block.upperBlocks.Clear();

                    for (int j = i + 1; j < gridBuilder.grids.Count; j++)
                    {
                        foreach (Block upperBlock in gridBuilder.grids[j].blockList)
                        {   
                            if (!upperBlock.gameObject.activeSelf) continue;
                            Debug.Log("Block " + block.id + " overlaps with upper block " + upperBlock.id);
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
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            CheckBlockOverlap();
        }
#endif
    }

    /*[CustomEditor(typeof(GridEditor))]
    public class GridBuilderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GridEditor gridEditor = (GridEditor)target;
            if (GUILayout.Button("Load Grids"))
            {
                gridEditor.gridBuilder.GenerateGrids();
            }
            if (GUILayout.Button("Load Block Overlap"))
            {
                gridEditor.CheckBlockOverlap();
            }
        }
    }*/
}
