using ChemistRecipe.Experiment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistRecipe
{
    public class LiquidParticle : MonoBehaviour
    {
        public struct LiquidParticleParam
        {
            public Experiment.Material material;
            public Volume volume;
        }

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

            FillableEquipment targetFillableObject = target.GetComponentInParent<FillableEquipment>();
            
            if(targetFillableObject)
            {
                Collider targetFillableArea = targetFillableObject.FillableArea;

                ParticleSystem.Particle[] particles = new ParticleSystem.Particle[part.particleCount];
                part.GetParticles(particles);

                for(int i = 0; i < part.particleCount; i++)
                {
                    if(targetFillableArea.bounds.Contains(particles[i].position))
                    {
                        // Get material of the particle
                        FillableEquipment sourceFillableObject = part.gameObject.GetComponentInParent<FillableEquipment>();
                        LiquidParticleParam particleParam = sourceFillableObject.GetParticleData(particles[i]);
                        
                        // Fill the other fillable
                        targetFillableObject.Fill(particleParam.material, particleParam.volume);

                        // Kill particle
                        particles[i].remainingLifetime = 0;
                    }
                }

                part.SetParticles(particles, part.particleCount);
            }
        }

    }
}
