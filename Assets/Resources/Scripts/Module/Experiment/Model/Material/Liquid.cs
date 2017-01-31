using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChemistRecipe.Experiment
{
    public class Liquid : Material
    {
        public Liquid(string name, string formula, string description) : base(name, Type.LIQUID, formula, description)
        { }
        public Liquid(string name, string description) : this(name, "", description)
        { }
        public Liquid(string name) : this(name, "")
        { }
    }
}
