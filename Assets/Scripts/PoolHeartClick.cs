using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class PoolHeartClick : ObjectPool
    {
        public static PoolHeartClick ins;
        private void Awake()
        {
            if (ins == null)
            {
                ins = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject); 
            }
        }
        public override void Start()
        {
            base.Start();
            SetupPool();
        }
    }
}
