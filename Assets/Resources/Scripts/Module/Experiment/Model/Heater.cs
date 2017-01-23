using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chemistrecipe.experiment.model
{
    public class Heater : Equipment
    {
        public bool heating = false;
        public FillableEquipment targetEquipment;

        public Heater(string name, string description) : base(name, description) { }

        public void startHeat()
        {
            if (targetEquipment == null) Console.WriteLine("Equipment not set to heater");
            heating = true;
        }

        public void endHeat()
        {
            if (targetEquipment == null) Console.WriteLine("Equipment not set to heater");
            heating = false;
        }
    }
}
