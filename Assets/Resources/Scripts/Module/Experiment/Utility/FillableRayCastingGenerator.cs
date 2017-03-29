using ChemistRecipe.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistRecipe.Utility
{
    [ExecuteInEditMode]
    public class FillableRayCastingGenerator : MonoBehaviour
    {
        public float currentY = 0f;
        private Vector3[] cVerties;

        void Update()
        {
            cVerties = RayCastingHelper.CastingAround(transform, currentY, true);
        }

        public void copyVerties()
        {
            string stringBuffer = "";

            foreach (Vector3 vertex in cVerties)
            {
                stringBuffer += "new Vector3(" + vertex.x.ToString("0.00000") + "f, " + vertex.y.ToString("0.00000") + "f, " + vertex.z.ToString("0.00000") + "f),\n";
            }

            Debug.Log(stringBuffer);
        }
    }
}

