using System;

namespace ChemistRecipe.Experiment
{
    [Serializable]
    public class Material : CourseObject
    {
        public string name;
        public Type type;
        public string formula;
        public string description;
        
        public Material(string name, Type type, string formula, string description)
        {
            this.name = name;
            this.type = type;
            this.formula = formula;
            this.description = description;
        }
        public Material(string name, Type type, string description) : this(name, type, "", description)
        { }
        public Material(string name, Type type) : this(name, type, "")
        { }
        public Material() : this("", Type.LIQUID)
        { }
    }

    public enum Type
    {
        LIQUID,
        SOLID,
        POWDER,
        GAS
    }

}
