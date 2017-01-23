using System;

namespace chemistrecipe.experiment.model
{
    public class Material
    {
        public string name;
        public MaterialType type;
        public string formula;
        public string description;
        
        public Material(string name, MaterialType type, string formula, string description)
        {
            this.name = name;
            this.type = type;
            this.formula = formula;
            this.description = description;
        }
        public Material(string name, MaterialType type, string description)
            : this(name, type, "", description) { }
        public Material(string name, MaterialType type)
            : this(name, type, "") { }
    }

    public enum MaterialType
    {
        POWDER,
        SOLID,
        LIQUID,
        GAS
    }
}
