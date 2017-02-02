using Assets.Resources.Scripts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chemistrecipe.data
{
    public class Course
    {
        public string id;
        public string name;
        public string description;
        public int diff;
        public int secounds;
        public Url url;
        
        public string assetBundlePath;
        public string markerBundlePath;
    }
}
