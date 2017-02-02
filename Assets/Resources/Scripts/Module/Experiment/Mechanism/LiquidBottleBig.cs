using ChemistRecipe.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistRecipe.Mechanism
{
    public class LiquidBottleBig : LiquidMeshFillable
    {

        #region verties settings

        private Vector3[] floorZeroVerties = new Vector3[]
        {
            new Vector3(0.11083f, 0.00540f, 0.02205f),
            new Vector3(0.10645f, 0.00540f, 0.04409f),
            new Vector3(0.09396f, 0.00540f, 0.06278f),
            new Vector3(0.08147f, 0.00540f, 0.08147f),
            new Vector3(0.06278f, 0.00540f, 0.09396f),
            new Vector3(0.04409f, 0.00540f, 0.10645f),
            new Vector3(0.02205f, 0.00540f, 0.11083f),
            new Vector3(0.00000f, 0.00540f, 0.11522f),
            new Vector3(-0.02205f, 0.00540f, 0.11083f),
            new Vector3(-0.04409f, 0.00540f, 0.10645f),
            new Vector3(-0.06278f, 0.00540f, 0.09396f),
            new Vector3(-0.08147f, 0.00540f, 0.08147f),
            new Vector3(-0.09396f, 0.00540f, 0.06278f),
            new Vector3(-0.10645f, 0.00540f, 0.04409f),
            new Vector3(-0.11083f, 0.00540f, 0.02205f),
            new Vector3(-0.11522f, 0.00540f, 0.00000f),
            new Vector3(-0.11083f, 0.00540f, -0.02205f),
            new Vector3(-0.10645f, 0.00540f, -0.04409f),
            new Vector3(-0.09396f, 0.00540f, -0.06278f),
            new Vector3(-0.08147f, 0.00540f, -0.08147f),
            new Vector3(-0.06278f, 0.00540f, -0.09396f),
            new Vector3(-0.04409f, 0.00540f, -0.10645f),
            new Vector3(-0.02205f, 0.00540f, -0.11083f),
            new Vector3(0.00000f, 0.00540f, -0.11522f),
            new Vector3(0.02205f, 0.00540f, -0.11083f),
            new Vector3(0.04409f, 0.00540f, -0.10645f),
            new Vector3(0.06278f, 0.00540f, -0.09396f),
            new Vector3(0.08147f, 0.00540f, -0.08147f),
            new Vector3(0.09396f, 0.00540f, -0.06278f),
            new Vector3(0.10645f, 0.00540f, -0.04409f),
            new Vector3(0.11083f, 0.00540f, -0.02205f),
            new Vector3(0.11522f, 0.00540f, 0.00000f),
        };

        private Vector3[] floorOneVerties = new Vector3[]
        {
            new Vector3(0.11153f, 0.00820f, 0.02218f),
            new Vector3(0.10711f, 0.00820f, 0.04437f),
            new Vector3(0.09455f, 0.00820f, 0.06317f),
            new Vector3(0.08198f, 0.00820f, 0.08198f),
            new Vector3(0.06317f, 0.00820f, 0.09455f),
            new Vector3(0.04437f, 0.00820f, 0.10711f),
            new Vector3(0.02218f, 0.00820f, 0.11153f),
            new Vector3(0.00000f, 0.00820f, 0.11594f),
            new Vector3(-0.02218f, 0.00820f, 0.11153f),
            new Vector3(-0.04437f, 0.00820f, 0.10711f),
            new Vector3(-0.06317f, 0.00820f, 0.09455f),
            new Vector3(-0.08198f, 0.00820f, 0.08198f),
            new Vector3(-0.09455f, 0.00820f, 0.06317f),
            new Vector3(-0.10711f, 0.00820f, 0.04437f),
            new Vector3(-0.11153f, 0.00820f, 0.02218f),
            new Vector3(-0.11594f, 0.00820f, 0.00000f),
            new Vector3(-0.11153f, 0.00820f, -0.02218f),
            new Vector3(-0.10711f, 0.00820f, -0.04437f),
            new Vector3(-0.09455f, 0.00820f, -0.06317f),
            new Vector3(-0.08198f, 0.00820f, -0.08198f),
            new Vector3(-0.06317f, 0.00820f, -0.09455f),
            new Vector3(-0.04437f, 0.00820f, -0.10711f),
            new Vector3(-0.02218f, 0.00820f, -0.11153f),
            new Vector3(0.00000f, 0.00820f, -0.11594f),
            new Vector3(0.02218f, 0.00820f, -0.11153f),
            new Vector3(0.04437f, 0.00820f, -0.10711f),
            new Vector3(0.06317f, 0.00820f, -0.09455f),
            new Vector3(0.08198f, 0.00820f, -0.08198f),
            new Vector3(0.09455f, 0.00820f, -0.06317f),
            new Vector3(0.10711f, 0.00820f, -0.04437f),
            new Vector3(0.11153f, 0.00820f, -0.02218f),
            new Vector3(0.11594f, 0.00820f, 0.00000f),
        };

        #endregion

        void Start()
        {
            Dictionary<int, Vector3>[] fVerties = new Dictionary<int, Vector3>[RayCastingHelper.DEFAULT_DEG_COUNT];

            for (int i = 0; i < RayCastingHelper.DEFAULT_DEG_COUNT; i++)
            {
                fVerties[i] = new Dictionary<int, Vector3>()
                {
                    { 0, floorZeroVerties[i] },
                    { 1, floorOneVerties[i] },
                };
            }

            SetupMeshGenerator(2, fVerties, 0.267f);
        }

        void Update()
        {
            GenerateMesh();
        }

    }
}
