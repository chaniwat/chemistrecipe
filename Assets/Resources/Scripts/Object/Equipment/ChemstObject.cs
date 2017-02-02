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
                case ChemstObject.BOTTLE:
                    {
                        return "Bottle";
                    }
                case ChemstObject.BEAKER:
                    {
                        return "Beaker";
                    }
            }

            return null;
        }
    }

    public enum ChemstObject
    {
        BEAKER,
        BOTTLE
    }
}
