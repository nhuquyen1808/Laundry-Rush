using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dinhvt.Level18 
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] int width;
        [SerializeField] int height;
        [SerializeField] Block blockPrefab;

        private void Start()
        {
            GenerateGrid();
        }

        void GenerateGrid()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 position = new Vector3(x, y, 0);
                    Block block = Instantiate(blockPrefab, position, Quaternion.identity);
                    block.name = $"Block {x} {y}";
                }
            }

            Camera.main.transform.position = new Vector3(width / 2f - 0.5f, height / 2f - 0.5f, -10);
        }
    }
}


