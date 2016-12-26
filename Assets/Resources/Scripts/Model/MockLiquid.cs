using chemistrecipe.element;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace chemistrecipe.element
{
    public class MockLiquid: Liquid
    {
        public MockLiquid(string name, Color color)
        {
            this.name = name;
            this.color = color;
        }   
    }
}
