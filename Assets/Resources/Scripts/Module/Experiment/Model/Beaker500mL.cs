using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chemistrecipe.experiment.model
{
    public class Beaker500mL : FillableEquipment
    {
        public Beaker500mL() : base("beaker", "beaker", new Volume(500, Volume.Metric.mL)) { }
    }
}
