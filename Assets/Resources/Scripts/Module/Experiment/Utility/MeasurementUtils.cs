using chemistrecipe.experiment.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chemistrecipe.experiment.utility
{
    public static class MeasurementUtils
    {
        public static Volume convertToGram(Volume litreVolume)
        {
            if(litreVolume.metric != Volume.Metric.mL)
            {
                throw new Exception("Invalid metric");
            }

            return new Volume(litreVolume.volume, Volume.Metric.g);
        }

        public static Volume convertToMiliLitre(Volume gramVolume)
        {
            if (gramVolume.metric != Volume.Metric.g)
            {
                throw new Exception("Invalid metric");
            }

            return new Volume(gramVolume.volume, Volume.Metric.mL);
        }
    }
}
