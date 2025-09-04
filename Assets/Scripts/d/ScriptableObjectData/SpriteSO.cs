using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dinhvt.Level18
{
    [Serializable]
    public class VisualData
    {
        public int index;
        public Sprite sprite;
    }

    [CreateAssetMenu(fileName = "SpriteSO", menuName = "ScriptableObjects/SpriteSO")]
    public class SpriteSO : ScriptableObject
    {
        public List<VisualData> spriteDataList = new List<VisualData>();

        public Sprite GetSprite(int id)
        {
            Sprite sprite = null;
            for (int i = 0; i <= spriteDataList.Count; i++)
            {
                if (id == spriteDataList[i].index)
                {
                    sprite = spriteDataList[i].sprite;
                    return sprite;
                }
            }

            return null;
        }
    }
}
