using ChemistRecipe.Experiment;
using System;

namespace Chemistrecipe.Experiment.Utility
{
    public static class MeasurementUtils
    {
        public static Volume convertToGram(Volume litreVolume)
        {
            if(litreVolume.metric != Volume.Metric.ANY && litreVolume.metric != Volume.Metric.mL)
            {
                throw new Exception("Invalid metric");
            }

            return new Volume(litreVolume.volume, Volume.Metric.g);
        }

        public static Volume convertToMiliLitre(Volume gramVolume)
        {
            if (gramVolume.metric != Volume.Metric.ANY && gramVolume.metric != Volume.Metric.g)
            {
                throw new Exception("Invalid metric");
            }

            return new Volume(gramVolume.volume, Volume.Metric.mL);
        }
    }
}
