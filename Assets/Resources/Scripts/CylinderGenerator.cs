using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderGenerator : MonoBehaviour {

    // Settings
    public int sideCount;
    public float[] baseHeightBreakPoint;
    public float currentYNormalize = 1f;

    // Variables
    private int previousFloor = 0;
    float[] heightBreakPoint;

    private Vector3[] verties;
    private Vector3[] normals;
    private Vector2[] uvs;
    private int[] triangles;

    MeshFilter filter;
    Mesh mesh;

    // Getter & Setter
    private float LowestY
    {
        get
        {
            return baseHeightBreakPoint[0];
        }
    }
    private float HighestY
    {
        get
        {
            return baseHeightBreakPoint[baseHeightBreakPoint.Length - 1];
        }
    }
    private float CurrentY
    {
        get
        {
            return Mathf.Lerp(baseHeightBreakPoint[0], baseHeightBreakPoint[baseHeightBreakPoint.Length - 1], currentYNormalize);
        }
    }

    // Use this for initialization
    void Start()
    {
        filter = gameObject.GetComponent<MeshFilter>();
        mesh = filter.mesh;

        int currentFloor = determinedFloor(currentYNormalize);
        previousFloor = currentFloor;

        heightBreakPoint = new float[currentFloor + 1];
        for (int i = 0; i < currentFloor + 1; i++)
        {
            heightBreakPoint[i] = baseHeightBreakPoint[i];
        }

        verties = ConstructVerties(heightBreakPoint);
        normals = ConstructNormals(verties.Length, heightBreakPoint.Length - 1);
        uvs = ConstructUVs(verties.Length, heightBreakPoint, CurrentY);
        triangles = ConstructTriangles(heightBreakPoint);

        mesh.Clear();

        mesh.vertices = verties;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
    }

    private Vector3[] ConstructVerties(float[] heightBreakPoint)
    {
        // Setup RayCaster
        RaycastHit hit;
        Vector3 castPosition;
        bool setCenterPoint = false;

        Vector3[] verties = new Vector3[
            sideCount + 1 // Top
            + sideCount + 1 // Bottom
            + sideCount * (heightBreakPoint.Length - 1) * 2 + (2 * (heightBreakPoint.Length - 1)) // Sides
        ];

        int counter = 0;

        #region Casting 360* Bottom side
        castPosition = transform.TransformPoint(
            new Vector3(transform.localPosition.x, heightBreakPoint[0], transform.localPosition.z)
        );

        counter++; // Skip center point, will set inside
        for (int r = 0; r < sideCount; r++)
        {
            float deg = (360f / sideCount) * (r + 1);
            float radian = deg * Mathf.Deg2Rad;
            float cos = Mathf.Cos(radian);
            float sin = Mathf.Sin(radian);
            Vector3 direction = transform.parent.TransformDirection(new Vector3(1 * cos - 0 * sin, 0, 1 * sin + 0 * cos));

            if (Physics.Raycast(castPosition, direction, out hit))
            {
                Vector3 localHitPoint = hit.transform.InverseTransformPoint(hit.point);

                verties[counter++] = localHitPoint;

                if (!setCenterPoint)
                {
                    verties[0] = hit.transform.InverseTransformPoint(castPosition);
                    setCenterPoint = true;
                }
            }
        }
        #endregion

        #region Casting 360* Top side
        setCenterPoint = false;

        castPosition = transform.TransformPoint(
            new Vector3(transform.localPosition.x, heightBreakPoint[heightBreakPoint.Length - 1], transform.localPosition.z)
        );

        counter++; // Skip center point, will set inside
        for (int r = 0; r < sideCount; r++)
        {
            float deg = (360f / sideCount) * (r + 1);
            float radian = deg * Mathf.Deg2Rad;
            float cos = Mathf.Cos(radian);
            float sin = Mathf.Sin(radian);
            Vector3 direction = transform.parent.TransformDirection(new Vector3(1 * cos - 0 * sin, 0, 1 * sin + 0 * cos));

            if (Physics.Raycast(castPosition, direction, out hit))
            {
                Vector3 localHitPoint = hit.transform.InverseTransformPoint(hit.point);

                verties[counter++] = localHitPoint;

                if (!setCenterPoint)
                {
                    verties[sideCount + 1] = hit.transform.InverseTransformPoint(castPosition);
                    setCenterPoint = true;
                }
            }
        }
        #endregion

        #region Casting 360* each side per height break point (except lowest and highest)

        if (heightBreakPoint.Length > 2)
        {
            Vector3 startAbovePoint = Vector3.zero, startBelowPoint = Vector3.zero;
            Vector3[] prevFloorVerties = new Vector3[sideCount];

            for (int floor = 1; floor < heightBreakPoint.Length - 1; floor++)
            {
                castPosition = transform.TransformPoint(
                    new Vector3(transform.localPosition.x, heightBreakPoint[floor], transform.localPosition.z)
                );

                for (int r = 0; r < sideCount; r++)
                {
                    float deg = (360f / sideCount) * (r + 1);
                    float radian = deg * Mathf.Deg2Rad;
                    float cos = Mathf.Cos(radian);
                    float sin = Mathf.Sin(radian);
                    Vector3 direction = transform.parent.TransformDirection(new Vector3(1 * cos - 0 * sin, 0, 1 * sin + 0 * cos));

                    if (Physics.Raycast(castPosition, direction, out hit))
                    {
                        Vector3 localHitPoint = hit.transform.InverseTransformPoint(hit.point);

                        Vector3 belowVertex;
                        if (floor == 1)
                        {
                            belowVertex = verties[r + 1];
                        }
                        else
                        {
                            belowVertex = prevFloorVerties[r];
                        }

                        verties[counter++] = new Vector3(localHitPoint.x, localHitPoint.y, localHitPoint.z);
                        verties[counter++] = new Vector3(belowVertex.x, belowVertex.y, belowVertex.z);

                        // Start point
                        if (r == 0)
                        {
                            startAbovePoint = new Vector3(localHitPoint.x, localHitPoint.y, localHitPoint.z);
                            startBelowPoint = new Vector3(belowVertex.x, belowVertex.y, belowVertex.z);
                        }

                        // End point
                        if (r == sideCount - 1)
                        {
                            verties[counter++] = new Vector3(startAbovePoint.x, startAbovePoint.y, startAbovePoint.z);
                            verties[counter++] = new Vector3(startBelowPoint.x, startBelowPoint.y, startBelowPoint.z);
                        }

                        prevFloorVerties[r] = new Vector3(localHitPoint.x, localHitPoint.y, localHitPoint.z);
                    }
                }
                
            }

            // Connect highest <-> highest - 1 floor
            for (int r = 0; r < sideCount; r++)
            {
                Vector3 highestVertex = verties[sideCount + 2 + r];
                Vector3 belowVertex = prevFloorVerties[r];

                verties[counter++] = new Vector3(highestVertex.x, highestVertex.y, highestVertex.z);
                verties[counter++] = new Vector3(belowVertex.x, belowVertex.y, belowVertex.z);

                // Start point
                if (r == 0)
                {
                    startAbovePoint = new Vector3(highestVertex.x, highestVertex.y, highestVertex.z);
                    startBelowPoint = new Vector3(belowVertex.x, belowVertex.y, belowVertex.z);
                }

                // End point
                if (r == sideCount - 1)
                {
                    verties[counter++] = new Vector3(startAbovePoint.x, startAbovePoint.y, startAbovePoint.z);
                    verties[counter++] = new Vector3(startBelowPoint.x, startBelowPoint.y, startBelowPoint.z);
                }
            }

        }

        #endregion

        return verties;
    }

    private Vector3[] ConstructNormals(int vertexCount, int floorCount)
    {
        Vector3[] normals = new Vector3[
            vertexCount
        ];

        int counter = 0;

        #region Bottom side
        while (counter <= sideCount)
        {
            normals[counter++] = Vector3.down;
        }
        #endregion

        #region Top side
        while (counter <= (sideCount * 2) + 1)
        {
            normals[counter++] = Vector3.up;
        }
        #endregion

        #region Side, per floor
        if (floorCount > 1)
        {
            for (int floor = 1; floor <= floorCount; floor++)
            {
                Vector3 startPointDirection = Vector3.zero;

                for (int r = 0; r < sideCount; r++)
                {
                    float deg = (360f / sideCount) * (r + 1);
                    float radian = deg * Mathf.Deg2Rad;
                    float cos = Mathf.Cos(radian);
                    float sin = Mathf.Sin(radian);
                    Vector3 direction = transform.parent.TransformDirection(new Vector3(1 * cos - 0 * sin, 0, 1 * sin + 0 * cos));

                    normals[counter++] = new Vector3(direction.x, direction.y, direction.z);
                    normals[counter++] = new Vector3(direction.x, direction.y, direction.z);

                    // Start point
                    if (r == 0)
                    {
                        startPointDirection = new Vector3(direction.x, direction.y, direction.z);
                    }

                    // End point
                    if (r == sideCount - 1)
                    {
                        normals[counter++] = new Vector3(startPointDirection.x, startPointDirection.y, startPointDirection.z);
                        normals[counter++] = new Vector3(startPointDirection.x, startPointDirection.y, startPointDirection.z);
                    }
                }
            }
        }
        #endregion

        return normals;
    }

    private Vector2[] ConstructUVs(int vertexCount, float[] heightBreakPoint, float currentY)
    {
        float _2pi = Mathf.PI * 2;

        Vector2[] uvs = new Vector2[
            vertexCount
        ];

        int counter = 0;

        #region Bottom side
        uvs[counter++] = new Vector2(0.5f, 0.5f);
        while (counter <= sideCount)
        {
            float rad = (float)counter / sideCount * _2pi;
            uvs[counter] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
            counter++;
        }
        #endregion

        #region Top side
        uvs[counter++] = new Vector2(0.5f, 0.5f);
        while (counter <= (sideCount * 2) + 1)
        {
            float rad = (float)counter / sideCount * _2pi;
            uvs[counter] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
            counter++;
        }
        #endregion

        #region Side, per floor
        if (heightBreakPoint.Length > 2)
        {
            float prevYFloor = 0f;

            for (int floor = 1; floor < heightBreakPoint.Length; floor++)
            {
                Vector2 startAboveUV = Vector2.zero, startBelowUV = Vector2.zero;

                float aboveY;
                if (floor == heightBreakPoint.Length - 1)
                {
                    aboveY = (currentY - LowestY) / (currentY - LowestY);
                }
                else
                {
                    aboveY = ((heightBreakPoint[floor] - LowestY)) / (currentY - LowestY);
                }
                
                float belowY;
                if (floor == 1)
                {
                    belowY = 0f;
                }
                else
                {
                    belowY = ((heightBreakPoint[floor - 1] - LowestY)) / (currentY - LowestY);
                }
                prevYFloor = aboveY;

                for (int r = 0; r < sideCount; r++)
                {
                    float x = (float) r / sideCount;

                    uvs[counter++] = new Vector2(x, aboveY);
                    uvs[counter++] = new Vector2(x, belowY);

                    // Start point
                    if (r == 0)
                    {
                        startAboveUV = new Vector2(x, aboveY);
                        startBelowUV = new Vector2(x, belowY);
                    }

                    // End point
                    if (r == sideCount - 1)
                    {
                        uvs[counter++] = new Vector2(1f, startAboveUV.y);
                        uvs[counter++] = new Vector2(1f, startBelowUV.y);
                    }
                }
            }
        }
        #endregion

        return uvs;
    }

    private int[] ConstructTriangles(float[] heightBreakPoint)
    {
        int[] triangles = new int[
            (sideCount * 3) * 2 // Top & Bottom
            + (sideCount * 6) * (heightBreakPoint.Length - 1) + (6 * factorial(heightBreakPoint.Length - 1)) // Side
        ];

        int counter = 0;

        #region Bottom side
        for (int i = 1; i <= sideCount; i++)
        {
            triangles[counter++] = 0;
            triangles[counter++] = i;
            triangles[counter++] = i + 1 > sideCount ? 1 : i + 1;
        }
        #endregion

        #region Top side
        for (int i = sideCount + 2; i <= (sideCount * 2) + 1; i++)
        {
            triangles[counter++] = i + 1 > (sideCount * 2) + 1 ? sideCount + 2 : i + 1;
            triangles[counter++] = i;
            triangles[counter++] = sideCount + 1;
        }
        #endregion

        #region Side, per floor
        if (heightBreakPoint.Length > 2)
        {
            int startCounterFloor = (sideCount * 2) + 2;

            for (int floor = 1; floor < heightBreakPoint.Length; floor++)
            {
                for (int r = 0; r < sideCount; r++)
                {
                    triangles[counter++] = startCounterFloor + 2;
                    triangles[counter++] = startCounterFloor + 1;
                    triangles[counter++] = startCounterFloor + 0;
                    startCounterFloor++;
                    
                    triangles[counter++] = startCounterFloor + 1;
                    triangles[counter++] = startCounterFloor + 2;
                    triangles[counter++] = startCounterFloor + 0;
                    startCounterFloor++;
                    
                    // End point
                    if (floor > 1 && r == sideCount - 1)
                    {
                        triangles[counter++] = startCounterFloor + 2;
                        triangles[counter++] = startCounterFloor + 1;
                        triangles[counter++] = startCounterFloor + 0;
                        startCounterFloor++;

                        triangles[counter++] = startCounterFloor + 1;
                        triangles[counter++] = startCounterFloor + 2;
                        triangles[counter++] = startCounterFloor + 0;
                        startCounterFloor++;
                    }
                }
            }
        }
        #endregion

        return triangles;
    }

    private int factorial(int n)
    {
        if (n == 0) return 1;

        return (n * factorial(n - 1));
    }

    public void ForceGenerate()
    {
        int currentFloor = determinedFloor(currentYNormalize);

        mesh.Clear();

        if (currentFloor == 0)
        {
            // Clear and skip mesh
        }
        else
        {
            heightBreakPoint = new float[currentFloor + 1];
            for (int i = 0; i < currentFloor + 1; i++)
            {
                heightBreakPoint[i] = baseHeightBreakPoint[i];
            }

            verties = ConstructVerties(heightBreakPoint);
            normals = ConstructNormals(verties.Length, heightBreakPoint.Length - 1);
            uvs = ConstructUVs(verties.Length, heightBreakPoint, CurrentY);
            triangles = ConstructTriangles(heightBreakPoint);

            // Recalculate Y
            for (int i = sideCount + 1; i < (sideCount * 2) + 2; i++)
            {
                verties[i].y = CurrentY;
            }

            int startFloorCounter = ((sideCount + 1) * 2) * (currentFloor);
            int endFloorCounter = ((sideCount + 1) * 2) * (currentFloor + 1);

            for (int i = startFloorCounter; i < endFloorCounter; i += 2)
            {
                verties[i].y = CurrentY;
            }

            mesh.vertices = verties;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.triangles = triangles;

            mesh.RecalculateBounds();
        }
    }

    bool filpflop = false;

    void Update()
    {
        if (!filpflop && GetComponent<MeshRenderer>().enabled)
        {
            ForceGenerate();
            filpflop = true;
        }
        else if(filpflop && !GetComponent<MeshRenderer>().enabled)
        {
            filpflop = false;
        }

        int currentFloor = determinedFloor(currentYNormalize);

        if (currentFloor != previousFloor)
        {
            ForceGenerate();

            previousFloor = currentFloor;
        }
        else
        {
            if (currentFloor == 0)
            {
                // skip mesh
            }
            else
            {
                for (int i = sideCount + 1; i < (sideCount * 2) + 2; i++)
                {
                    verties[i].y = CurrentY;
                }

                int startFloorCounter = ((sideCount + 1) * 2) * (currentFloor);
                int endFloorCounter = ((sideCount + 1) * 2) * (currentFloor + 1);

                for (int i = startFloorCounter; i < endFloorCounter; i += 2)
                {
                    verties[i].y = CurrentY;
                }

                mesh.vertices = verties;
                mesh.uv = ConstructUVs(verties.Length, heightBreakPoint, CurrentY);
            }
        }
    }

    private int determinedFloor(float currentYNormalize)
    {
        float currentY = Mathf.Lerp(baseHeightBreakPoint[0], baseHeightBreakPoint[baseHeightBreakPoint.Length - 1], currentYNormalize);

        for (int i = baseHeightBreakPoint.Length - 1; i >= 0; i--)
        {
            if (currentY > baseHeightBreakPoint[i])
            {
                return i + 1;
            }
        }

        return 0;
    }

}
