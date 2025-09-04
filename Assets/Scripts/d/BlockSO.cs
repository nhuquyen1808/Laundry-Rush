using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Block_", menuName = "ScriptableObjects/BlockSO")]
public class BlockSO : ScriptableObject
{
    public int id;
    public Sprite sprite;
}
