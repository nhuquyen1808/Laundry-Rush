using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class PlayParticleInGame : MonoBehaviour
    {
        public List<ParticleSystem> positiveParticles = new List<ParticleSystem>();
        public List<ParticleSystem> negativeParticles = new List<ParticleSystem>();

        public void PlayParticle(bool isPositive, Vector3 position)
        {
            int index = 0;
            if (isPositive)
            {
                index = Random.Range(0, positiveParticles.Count);
                positiveParticles[index].transform.position = position;
                Duck.PlayParticle(positiveParticles[index]);
            }
            else
            {
                index = Random.Range(0, negativeParticles.Count);
                negativeParticles[index].transform.position = position;
                Duck.PlayParticle(negativeParticles[index]);
            }
        }
        
        
    }
}
