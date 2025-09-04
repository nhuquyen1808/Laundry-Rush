using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dinhvt.Level18
{

    public class Board : MonoBehaviour
    {
        [SerializeField] int width;
        [SerializeField] int height;
        [SerializeField] Transform center;
        public Block block;
        [SerializeField] List<BlockData> blockDatas;

        private Vector2 blockSize = new Vector2(0.75f, 0.8f);
        private Vector2 blockOffset;

        private void Start()
        {
            blockOffset = new Vector2(blockSize.x * (width - 1) / 2f, blockSize.y * (height - 1) / 2f);

            SpawnBlocks();
        }

        private void SpawnBlocks()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (blockDatas.Count == 0) return;

                    Block b = Instantiate(block);
                    int index = Random.Range(0, blockDatas.Count);
                    //int index = 0;

                    int id = blockDatas[index].id;
                    int count = blockDatas[index].count;
                    Sprite sprite = blockDatas[index].sprite;

                    count++;
                    blockDatas[index] = new BlockData(id, sprite, count);
                    if (count == 3)
                    {
                        blockDatas.RemoveAt(index);
                    }

                    b.Init(id, sprite);
                    b.transform.position = new Vector3(center.position.x + x * blockSize.x - blockOffset.x, center.position.y + y * blockSize.y - blockOffset.y, 0);
                    b.transform.name = "Block_" + x + "_" + y;
                }
            }
        }

        [System.Serializable]
        public struct BlockData
        {
            public int id;
            public Sprite sprite;
            public int count;

            public BlockData(int id, Sprite sprite, int count)
            {
                this.id = id;
                this.sprite = sprite;
                this.count = count;
            }
        }
    }

}