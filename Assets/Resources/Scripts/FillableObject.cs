using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chemistrecipe
{
    public class FillableObject : MonoBehaviour
    {

        // Set liquid color
        public Color liquidColor = Color.white;

        // Liquid particle system
        private ParticleSystem liquidParticle;
        private ParticleSystem.EmissionModule lEmission; 

        // Use this for initialization
        void Start()
        {
            liquidParticle = GetComponentInChildren<ParticleSystem>();
            var lMain = liquidParticle.main;
            lEmission = liquidParticle.emission;

            lMain.startColor = new ParticleSystem.MinMaxGradient(liquidColor, liquidColor);        
        }

        // Update is called once per frame
        void Update()
        {
            if (transform.forward.y < -0.2f)
            {
                lEmission.enabled = true;
            }
            else
            {
                lEmission.enabled = false;
            }
        }        

    }
}
