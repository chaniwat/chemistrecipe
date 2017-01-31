using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChemistRecipe.Experiment
{
    public class Powder : Solid
    {
        public Powder(string name, string formula, string description) : base(name, formula, description)
        {
            type = Type.POWDER;
        }
        public Powder(string name, string description) : this(name, "", description)
        { }
        public Powder(string name) : this(name, "")
        { }
    }
}
