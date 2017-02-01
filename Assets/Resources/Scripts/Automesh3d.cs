using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automesh3d : MonoBehaviour
{
    public float currentY = 120f;

    private MeshFilter mf;
    private LiquidInBottle meshGenerator;

    private int floorCount = 3;
    private int degCount = 32;

    void Start()
    {
        mf = GetComponent<MeshFilter>();

        // Generate circle for cylinder
        Dictionary<int, Vector3>[] sVerties = new Dictionary<int, Vector3>[degCount];
        for (int floor = 0; floor < floorCount; floor++)
        {
            for (int counter = 0; counter < degCount; counter++)
            {
                float deg = (360f / degCount) * (counter + 1);
                float radian = deg * Mathf.Deg2Rad;
                float cos = Mathf.Cos(radian);
                float sin = Mathf.Sin(radian);

                float currentYi = 50f * floor;
                float additionalRadius = 50 * (currentYi / 50);
                Vector3 newDirection = new Vector3((1 * cos - 0 * sin) * (50), currentYi, (1 * sin + 0 * cos) * (50));

                if (sVerties[counter] == null)
                {
                    sVerties[counter] = new Dictionary<int, Vector3>()
                    {
                        { floor, newDirection }
                    };
                }
                else
                {
                    sVerties[counter].Add(floor, newDirection);
                }
            }
        }

        meshGenerator = new LiquidInBottle(floorCount, degCount, sVerties);
    }

    void Update()
    {
        List<Mesh> meshes = new List<Mesh>();

        // Generate circle for cylinder (current Y position)
        Vector3[] cVerties = new Vector3[degCount];
        for (int counter = 0; counter < degCount; counter++)
        {
            float deg = (360f / degCount) * (counter + 1);
            float radian = deg * Mathf.Deg2Rad;
            float cos = Mathf.Cos(radian);
            float sin = Mathf.Sin(radian);

            float additionalRadius = 50 * (currentY / 50);
            Vector3 newDirection = new Vector3((1 * cos - 0 * sin) * (50), currentY, (1 * sin + 0 * cos) * (50));

            cVerties[counter] = newDirection;
        }

        mf.mesh = meshGenerator.GenerateMesh(transform, cVerties);
    }

}
