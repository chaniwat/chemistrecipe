using ChemistRecipe.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistRecipe.Mechanism
{
    public class LiquidBeakerBig : LiquidMeshFillable
    {
        
        #region verties settings

        private Vector3[] floorZeroVerties = new Vector3[]
        {
            new Vector3(0.11205f, 0.00363f, 0.02229f),
            new Vector3(0.10623f, 0.00363f, 0.04400f),
            new Vector3(0.09665f, 0.00363f, 0.06458f),
            new Vector3(0.08061f, 0.00363f, 0.08061f),
            new Vector3(0.06458f, 0.00363f, 0.09665f),
            new Vector3(0.04400f, 0.00363f, 0.10623f),
            new Vector3(0.02229f, 0.00363f, 0.11205f),
            new Vector3(0.00000f, 0.00363f, 0.11802f),
            new Vector3(-0.02229f, 0.00363f, 0.11205f),
            new Vector3(-0.04400f, 0.00363f, 0.10623f),
            new Vector3(-0.06458f, 0.00363f, 0.09665f),
            new Vector3(-0.08061f, 0.00363f, 0.08061f),
            new Vector3(-0.09665f, 0.00363f, 0.06458f),
            new Vector3(-0.10623f, 0.00363f, 0.04400f),
            new Vector3(-0.11205f, 0.00363f, 0.02229f),
            new Vector3(-0.11802f, 0.00363f, 0.00000f),
            new Vector3(-0.11205f, 0.00363f, -0.02229f),
            new Vector3(-0.10623f, 0.00363f, -0.04400f),
            new Vector3(-0.09665f, 0.00363f, -0.06458f),
            new Vector3(-0.08061f, 0.00363f, -0.08061f),
            new Vector3(-0.06458f, 0.00363f, -0.09665f),
            new Vector3(-0.04400f, 0.00363f, -0.10623f),
            new Vector3(-0.02229f, 0.00363f, -0.11205f),
            new Vector3(0.00000f, 0.00363f, -0.11802f),
            new Vector3(0.02229f, 0.00363f, -0.11205f),
            new Vector3(0.04400f, 0.00363f, -0.10623f),
            new Vector3(0.06458f, 0.00363f, -0.09665f),
            new Vector3(0.08061f, 0.00363f, -0.08061f),
            new Vector3(0.09665f, 0.00363f, -0.06458f),
            new Vector3(0.10623f, 0.00363f, -0.04400f),
            new Vector3(0.11205f, 0.00363f, -0.02229f),
            new Vector3(0.11802f, 0.00363f, 0.00000f),
        };

        private Vector3[] floorOneVerties = new Vector3[]
        {
            new Vector3(0.11799f, 0.00949f, 0.02347f),
            new Vector3(0.11187f, 0.00949f, 0.04634f),
            new Vector3(0.10177f, 0.00949f, 0.06800f),
            new Vector3(0.08489f, 0.00949f, 0.08489f),
            new Vector3(0.06800f, 0.00949f, 0.10177f),
            new Vector3(0.04634f, 0.00949f, 0.11187f),
            new Vector3(0.02347f, 0.00949f, 0.11799f),
            new Vector3(0.00000f, 0.00949f, 0.12428f),
            new Vector3(-0.02347f, 0.00949f, 0.11799f),
            new Vector3(-0.04634f, 0.00949f, 0.11187f),
            new Vector3(-0.06800f, 0.00949f, 0.10177f),
            new Vector3(-0.08489f, 0.00949f, 0.08489f),
            new Vector3(-0.10177f, 0.00949f, 0.06800f),
            new Vector3(-0.11187f, 0.00949f, 0.04634f),
            new Vector3(-0.11799f, 0.00949f, 0.02347f),
            new Vector3(-0.12428f, 0.00949f, 0.00000f),
            new Vector3(-0.11799f, 0.00949f, -0.02347f),
            new Vector3(-0.11187f, 0.00949f, -0.04634f),
            new Vector3(-0.10177f, 0.00949f, -0.06800f),
            new Vector3(-0.08489f, 0.00949f, -0.08489f),
            new Vector3(-0.06800f, 0.00949f, -0.10177f),
            new Vector3(-0.04634f, 0.00949f, -0.11187f),
            new Vector3(-0.02347f, 0.00949f, -0.11799f),
            new Vector3(0.00000f, 0.00949f, -0.12428f),
            new Vector3(0.02347f, 0.00949f, -0.11799f),
            new Vector3(0.04634f, 0.00949f, -0.11187f),
            new Vector3(0.06800f, 0.00949f, -0.10177f),
            new Vector3(0.08489f, 0.00949f, -0.08489f),
            new Vector3(0.10177f, 0.00949f, -0.06800f),
            new Vector3(0.11187f, 0.00949f, -0.04634f),
            new Vector3(0.11799f, 0.00949f, -0.02347f),
            new Vector3(0.12428f, 0.00949f, 0.00000f),
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

            SetupMeshGenerator(2, fVerties, 0.36f);
        }

        void Update()
        {
            GenerateMesh();
        }

    }
}
