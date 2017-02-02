using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChemistRecipe.Experiment
{
    public class Solid : Material
    {
        public Solid(string name, string formula, string description) : base(name, Type.SOLID, formula, description)
        { }
        public Solid(string name, string description) : this(name, "", description)
        { }
        public Solid(string name) : this(name, "")
        { }
    }
}