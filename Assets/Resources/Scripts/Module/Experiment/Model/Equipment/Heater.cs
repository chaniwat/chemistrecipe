using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChemistRecipe.Experiment
{
    public class Heater : Equipment
    {
        public bool heating = false;
        public FillableEquipment targetEquipment;

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
