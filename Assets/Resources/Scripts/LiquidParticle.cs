using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chemistrecipe
{
    public class LiquidParticle : MonoBehaviour
    {

        // TODO Make more accurate liquid (mL per particle)
        // TODO Kill particle when hit FillableObject (by script or trigger?)

        private ParticleSystem part;
        private List<ParticleCollisionEvent> collisionEvents;

        void Start()
        {
            part = GetComponent<ParticleSystem>();
            collisionEvents = new List<ParticleCollisionEvent>();
        }

        void OnParticleCollision(GameObject target)
        {
            part.GetCollisionEvents(target, collisionEvents);

            FillableObject targetFillableObject = target.GetComponentInParent<FillableObject>();
            
            if(targetFillableObject)
            {
                Collider targetFillableArea = targetFillableObject.FillableArea;

                ParticleSystem.Particle[] particles = new ParticleSystem.Particle[part.particleCount];
                part.GetParticles(particles);

                for(int i = 0; i < part.particleCount; i++)
                {
                    if(targetFillableArea.bounds.Contains(particles[i].position))
                    {
                        particles[i].remainingLifetime = 0;
                        targetFillableObject.capacity += .25f;
                    }
                }

                part.SetParticles(particles, part.particleCount);
            }
        }

    }
}
