using ChemistRecipe.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistRecipe.Mechanism
{
    public class LiquidMeshFillable : MonoBehaviour
    {
        public float currentY = 0f;
        public bool drawDebugCast = false;

        private MeshFilter mf;
        private Vector3[] cVerties;
        private LiquidMeshFillableGenerator meshGenerator;
        private float _highestY = 0f;
        public float highestY
        {
            get
            {
                return _highestY;
            }
        }
        
        public void SetupMeshGenerator(int floor, Dictionary<int, Vector3>[] fVerties, float highestY)
        {
            mf = GetComponent<MeshFilter>();
            this._highestY = highestY;

            meshGenerator = new LiquidMeshFillableGenerator(floor, RayCastingHelper.DEFAULT_DEG_COUNT, fVerties);
        }

        public void GenerateMesh()
        {
            float yOffset;
            if (currentY > highestY)
            {
                yOffset = highestY;
            }
            else if (currentY < 0)
            {
                yOffset = 0;
            }
            else
            {
                yOffset = currentY;
            }

            cVerties = RayCastingHelper.CastingAround(transform, yOffset, drawDebugCast);

            mf.mesh.Clear();
            mf.mesh = meshGenerator.GenerateMesh(transform, cVerties);
        }

    }
}
