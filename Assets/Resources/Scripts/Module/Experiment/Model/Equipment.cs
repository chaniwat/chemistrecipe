using System;

namespace chemistrecipe.experiment.model
{
    public abstract class Equipment
    {
        public string name;
        public string description;

        public Equipment(string name, string description)
        {
            this.name = name;
            this.description = description;
        }
    }
}
