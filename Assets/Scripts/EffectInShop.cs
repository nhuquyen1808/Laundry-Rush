using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class EffectInShop : MonoBehaviour
    {
        public ParticleSystem sparkleBody,starExplosion;

        public void PlaySparkle()
        {
            Duck.PlayParticle(sparkleBody);
        }

        public void PlayStarExplodet(Vector3 position)
        {
            starExplosion.transform.position = position;
            Duck.PlayParticle(starExplosion);
        }
       
    }
}
