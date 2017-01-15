using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chemistrecipe
{
    public static class Extensions
    {
        public static string toString(this ChemstObject chemstObject)
        {
            switch (chemstObject)
            {
                case ChemstObject.BEAKER:
                    {
                        return "beaker";
                    }
                case ChemstObject.BOILING_FLASK:
                    {
                        return "Boiling Flask";
                    }
            }

            return null;
        }
    }

    public enum ChemstObject
    {
        BOILING_FLASK,
        BEAKER
    }
}
