using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chemistrecipe.experiment.model
{
    public class Volume
    {
        public enum Metric {
            mL,
            g,
            ANY
        }

        public float volume;
        public float tempature;
        public Metric metric;
        
        public Volume(float volume, float tempature, Metric metric)
        {
            this.volume = volume;
            this.tempature = tempature;
            this.metric = metric;
        }
        public Volume(float volume, Metric metric) : this(volume, 25f, metric) { }
    }
}
