using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChemistRecipe.Experiment
{
    public class Gas : Material
    {
        public Gas(string name, string formula, string description) : base(name, Type.GAS, formula, description)
        { }
        public Gas(string name, string description) : this(name, "", description)
        { }
        public Gas(string name) : this(name, "")
        { }
    }
}
