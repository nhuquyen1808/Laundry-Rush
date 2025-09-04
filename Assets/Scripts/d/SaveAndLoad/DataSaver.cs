using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.IO;

namespace dinhvt.Level18
{   

    public class DataSaver : MonoBehaviour
    {   
        public string levelName;
        public GridBuilder gridBuilder;
        public List<SpriteData> spriteDataList;

        public static string filePath = "Assets/Resources/Data/Game18";

        public void Save()
        {
            LevelData levelData = new LevelData();

            int totalCount = 0;
            for (int i = 0; i < gridBuilder.grids.Count; i++)
            {
                GridData gridData = new GridData();
                gridData.width = gridBuilder.grids[i].width;
                gridData.height = gridBuilder.grids[i].height;
                levelData.grids.Add(gridData);
                for (int j = 0; j < gridBuilder.grids[i].blockList.Count; j++)
                {   
                    
                    Block block = gridBuilder.grids[i].blockList[j];
                    if (!block.gameObject.activeSelf) continue;
                    
                    BlockData blockData = new BlockData();
                    blockData.isVisible = block.canSelect;
                    blockData.row = block.row;
                    blockData.col = block.col;
                    levelData.grids[i].blocks.Add(blockData);

                    totalCount++;
                }
            }

            if (spriteDataList != null) levelData.spriteDatas = spriteDataList;

            if (totalCount % 3 != 0)
            {
                Debug.LogError("Total count of blocks is not a multiple of 3. Please check the grid data.");
                return;
            }

            if (levelName.Equals(""))
            {
                Debug.LogError("Level name is empty. Please set a valid level name.");
                return;
            }

            string json = JsonUtility.ToJson(levelData, true);
            File.WriteAllText($"{filePath}/Level_{levelName}.json", json);

            levelName = "";

            Debug.Log("Level data saved: " + json);
        }
    }


    [Serializable]
    public class BlockData
    {
        public int row;
        public int col;
        public bool isVisible;
    }

    [Serializable]
    public class GridData
    {
        public int width;
        public int height;
        public List<BlockData> blocks = new List<BlockData>();
    }

    [Serializable]
    public class SpriteData
    {
        //public BlockSO BlockSO;
        public int idItem;
        public int numOfItems;
    }

    [Serializable]
    public class LevelData
    {
        public List<GridData> grids = new List<GridData>();
        public List<SpriteData> spriteDatas = new List<SpriteData>();
    }

    /*
    [CustomEditor(typeof(DataSaver))]
    public class DataSaverEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DataSaver dataSaverEditor = (DataSaver)target;
            if (GUILayout.Button("Save Data"))
            {
                dataSaverEditor.Save();
            }

        }
    }*/
}
