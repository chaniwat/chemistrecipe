using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chemistrecipe.experiment.model
{
    public class Beaker1000mL : FillableEquipment
    {
        public Beaker1000mL() : base("beaker", "beaker", new Volume(1000, Volume.Metric.mL)) { }
    }
}
