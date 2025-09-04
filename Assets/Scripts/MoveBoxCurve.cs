using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class MoveBoxCurve : MonoBehaviour
{
    /*public Vector3 startPoint;
    public Vector3 endPoint;
    public float heightOffset = 3f; // Độ cao để tạo đường cong
    public float duration = 2f;

    void Start()
    {
        transform.DOJump(endPoint, 5, 1, 1,false);
    }*/
    
    public List<GameObject> cubes = new List<GameObject>();
    public  List<Vector3> positions = new List<Vector3>();

    private void Start()
    
    { 
        
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                positions.Add(new Vector3(i,  0,j));
            }
        }
        for (int i = 0; i < cubes.Count; i++)
        {
            for (int j = 0; j < positions.Count; j++)
            {
                if (i == j)
                {
                    var a = i;
                    cubes[a].transform.DOJump(positions[i], 5, 1, 1,false).SetDelay(a*0.1f);
                }
            }
           
        }
    }
}