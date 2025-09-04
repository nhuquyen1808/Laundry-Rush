using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace DevDuck
{
    public class HeartClickManager : MonoBehaviour
    {
        [SerializeField] PoolHeartClick poolHeartClick;
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                AudioManager.instance.PlaySound("Click");
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                PooledObject heartClick = poolHeartClick.GetPooledObject(mousePos, quaternion.identity);
                Duck.PlayParticle(heartClick.GetComponent<ParticleSystem>());
                StartCoroutine(releaseHeartClick(heartClick));
            }
        }

        IEnumerator releaseHeartClick(PooledObject pooledObject)
        {
            yield return new WaitForSeconds(1f);
            pooledObject.ReturnToPool();
        }
    }
}
