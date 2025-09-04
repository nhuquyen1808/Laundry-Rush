using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace dinhvt.Level18
{
    public class DataLoader : MonoBehaviour
    {
        //public int levelIndex = 0;
        public Block blockPrefab;
        public GridBuilder gridBuilder;
        public Grid gridPrefab;
        public Transform center;

        public List<SpriteData> spriteDataList;
        public int numOfItems = 0;
        public SpriteSO SpriteSO;

        public float scaleValue;

        [SerializeField] private LevelData levelData;
        [SerializeField] private List<SpriteData> blockSOList = new List<SpriteData>();
        public List<int> listIntInGame;


        private void Start()
        {
            InitData();

            ResizeCamera();

            Load();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerPrefs.SetInt(GameConfig.CURRENT_LEVEL, 0);
                PlayerPrefs.Save();
                Debug.Log("Reset level to 0");
            }
        }

        private void InitData()
        {
            int levelIndex = PlayerPrefs.GetInt(GameConfig.CURRENT_LEVEL, 0);
            Debug.Log("Current level: " + levelIndex);
            levelIndex %= 8;

            string filePath = $"Data/Game18/Level_{levelIndex}";
            Debug.Log("Loading level data from: " + filePath);
            string json = Resources.Load<TextAsset>(filePath).ToString();
            if (!json.Equals(""))
            {
                levelData = JsonUtility.FromJson<LevelData>(json);
            }
            else
            {
                Debug.LogError("No data found for level: " + levelIndex);
                return;
            }
        }

        public void Load()
        {
            spriteDataList = levelData.spriteDatas;

            //foreach (SpriteData spriteData in spriteDataList)
            //{
            //    listIntInGame.Add(spriteData.idItem);
            //}
            foreach (SpriteData spriteData in spriteDataList)
            {
                for (int i = 0; i < spriteData.numOfItems; i++)
                {
                    listIntInGame.Add(spriteData.idItem);
                    //blockSOList.Add(spriteData);
                }
            }

            gridBuilder.grids.Clear();
            for (int i = 0; i < levelData.grids.Count; i++)
            {
                GridData gridData = levelData.grids[i];
                Grid grid = Instantiate(gridPrefab, transform);
                grid.width = gridData.width;
                grid.height = gridData.height;
                grid.center = center;
                grid.CalculateStartPosition();
                for (int j = 0; j < gridData.blocks.Count; j++)
                {
                    BlockData blockData = gridData.blocks[j];

                    Block block = Instantiate(blockPrefab, grid.transform);

                    block.col2D.enabled = false;

                    block.row = blockData.row;
                    block.col = blockData.col;
                    block.SetCanSelect(blockData.isVisible);
                    block.transform.localScale = Vector3.zero;
                    float gridZ = 0.1f * (-i);
                    float blockZ = -0.01f * (block.row + block.col);
                    block.transform.position = grid.startPosition + new Vector3(block.row * grid.blockSize.x, -block.col * grid.blockSize.y, gridZ + blockZ);

                    int randomIndex = UnityEngine.Random.Range(0, listIntInGame.Count);
                    int index = listIntInGame[randomIndex];

                    Debug.Log("index: " + index);
                    Sprite sp = SpriteSO.GetSprite(index);
                    block.Init(index, sp);
                    listIntInGame.Remove(index);

                    grid.blockList.Add(block);
                }

                grid.SetSortingLayer(i);

                gridBuilder.grids.Add(grid);
            }

            gridBuilder.ScaleAndCheckOverlap();
        }

        private void ResizeCamera()
        {
            int maxColumn = 0;
            int maxRow = 0;
            for (int i = 0; i < levelData.grids.Count; i++)
            {
                GridData gridData = levelData.grids[i];

                if (gridData.height > maxRow)
                {
                    maxRow = gridData.height;
                }

                if (gridData.width > maxColumn)
                {
                    maxColumn = gridData.width;
                }

                for (int j = 0; j < gridData.blocks.Count; j++)
                {
                    numOfItems++;
                }
            }

            // Default column size is 6, one column increases the camera size by 0.5f
            int max = Mathf.Max(maxColumn, maxRow);
            Camera.main.orthographicSize += (max - 6) * 0.5f;

            scaleValue = Camera.main.orthographicSize / 5f;

            Debug.Log("Total count of blocks: " + numOfItems);
        }
    }


    /*
    [CustomEditor(typeof(DataLoader))]
    public class DataLoaderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DataLoader dataSaverEditor = (DataLoader)target;
            if (GUILayout.Button("Load Data"))
            {
                dataSaverEditor.Load();
            }

            //if (GUILayout.Button("Check Overlap"))
            //{
            //    dataSaverEditor.gridBuilder.CheckBlockOverlap();
            //}
        }
    }*/
}
