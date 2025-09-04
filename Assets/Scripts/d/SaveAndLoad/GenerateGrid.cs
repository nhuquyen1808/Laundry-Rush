using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dinhvt.Level18
{
    public class GenerateGrid : MonoBehaviour
    {
        //public int width;
        //public int height;
        //[Space(10)]
        //public Block blockPrefab;
        //public List<Block> blockList = new List<Block>();

        //private Vector2 blockSize = new Vector2(0.75f, 0.8f);
        //private Vector3 startPosition;

        //private void Start()
        //{
        //    //GenerateBlocks(0);
        //    InitBlock();
        //}
        //public void InitBlock()
        //{
        //    for (int x = 0; x < width; x++)
        //    {
        //        for (int y = 0; y < height; y++)
        //        {
        //            Block b = Instantiate(blockPrefab, transform);
        //            b.gameObject.SetActive(false);
        //            b.row = x;
        //            b.col = y;
        //            blockList.Add(b);
        //            b.transform.name = "Block_" + x + "_" + y;
        //        }
        //    }
        //}

        //public void GenerateBlocks(Transform center, int sortingOrder)
        //{
        //    startPosition = new Vector3(center.position.x - ((width - 1) * blockSize.x / 2f), center.position.y + ((height - 1) * blockSize.y / 2f), 0.1f * (-sortingOrder));

        //    int index = 0;
        //    for (int x = 0; x < width; x++)
        //    {
        //        for (int y = 0; y < height; y++)
        //        {
        //            blockList[index].gameObject.SetActive(true);
        //            blockList[index].SetSortingOrder(sortingOrder);
        //            blockList[index].transform.position = startPosition + new Vector3(x * blockSize.x, -y * blockSize.y);

        //            index++;
        //        }
        //    }
        //}
    }
}
