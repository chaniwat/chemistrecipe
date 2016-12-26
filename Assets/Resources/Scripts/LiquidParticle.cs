using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chemistrecipe
{
    public class LiquidParticle : MonoBehaviour
    {

        private ParticleSystem part;
        private List<ParticleCollisionEvent> collisionEvents;

        void Start()
        {
            part = GetComponent<ParticleSystem>();
            collisionEvents = new List<ParticleCollisionEvent>();
        }

        void OnParticleCollision(GameObject target)
        {
            int numCollisionEvents = part.GetCollisionEvents(target, collisionEvents);

            FillableObject targetFillableObject = target.GetComponentInParent<FillableObject>();
            int i = 0;

            while (i < numCollisionEvents)
            {
                targetFillableObject.capacity += .250f;
                i++;
            }
        }

    }
}
