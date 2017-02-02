using System;
using UnityEngine;

namespace ChemistRecipe.Experiment
{
    [Serializable]
    public class Volume
    {
        public enum Metric {
            mL,
            g,
            ANY
        }

        public float volume;
        public float tempature = 25f;
        public Metric metric;
        
        public Volume(float volume, float tempature, Metric metric)
        {
            this.volume = volume;
            this.tempature = tempature;
            this.metric = metric;
        }
        public Volume(float volume, Metric metric) : this(volume, 25f, metric)
        { }
        public Volume() : this(0, Metric.ANY)
        { }
    }
}
