using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Create a mesh inside bottle
/// Author: Meranote, mimi~~
/// </summary>
namespace ChemistRecipe.Utility
{
    public class LiquidMeshFillableGenerator
    {
        private int floorCount;
        private int degCount;
        private Dictionary<int, Vector3>[] fVerties;

        private Dictionary<int, List<Mesh>> cacheMeshes;
        private List<Mesh> cacheBottomMeshes;

        public LiquidMeshFillableGenerator(int floorCount, int degCount, Dictionary<int, Vector3>[] fVerties)
        {
            this.floorCount = floorCount;
            this.degCount = degCount;
            this.fVerties = fVerties;

            cacheMeshes = new Dictionary<int, List<Mesh>>();
            cacheBottomMeshes = new List<Mesh>();

            GenerateCacheMeshPerFloor();
            GenerateCacheMeshBottomFloor();
        }

        private void GenerateCacheMeshPerFloor()
        {
            for (int floor = 0; floor < floorCount - 1; floor++)
            {
                List<Mesh> meshes = new List<Mesh>();

                for (int cDeg = 0; cDeg < degCount; cDeg++)
                {
                    Poly2Mesh.Polygon poly = new Poly2Mesh.Polygon();
                    if (cDeg + 1 == degCount)
                    {
                        poly.outside = new List<Vector3>()
                    {
                        fVerties[cDeg][floor],
                        fVerties[cDeg][floor + 1],
                        fVerties[0][floor + 1],
                        fVerties[0][floor],
                    };
                    }
                    else
                    {
                        poly.outside = new List<Vector3>()
                    {
                        fVerties[cDeg][floor],
                        fVerties[cDeg][floor + 1],
                        fVerties[cDeg + 1][floor + 1],
                        fVerties[cDeg + 1][floor],
                    };
                    }

                    meshes.Add(Poly2Mesh.CreateMesh(poly));
                }

                cacheMeshes.Add(floor, meshes);
            }
        }

        private void GenerateCacheMeshBottomFloor()
        {
            Poly2Mesh.Polygon poly = new Poly2Mesh.Polygon();
            List<Vector3> polyVecs = new List<Vector3>();
            foreach (Dictionary<int, Vector3> degVec in fVerties)
            {
                polyVecs.Add(degVec[0]);
            };
            poly.outside = polyVecs;

            cacheBottomMeshes.Add(Poly2Mesh.CreateMesh(poly));
        }

        public Mesh GenerateMesh(Transform transform, Vector3[] cVerties)
        {
            List<Mesh> meshes = new List<Mesh>();

            // Make meshes if current Y is over floor zero Y
            if (cVerties[0].y > fVerties[0][0].y)
            {
                // Check floor for use in cache
                int useFloor = -1;
                for (int floor = 0; floor < floorCount - 1; floor++)
                {
                    if (fVerties[0][floor + 1].y <= cVerties[0].y)
                    {
                        useFloor++;
                    }
                    else
                    {
                        break;
                    }
                }

                // Add cache meshes at useFloor
                for (int floor = 0; floor <= useFloor; floor++)
                {
                    meshes.AddRange(cacheMeshes[floor]);
                }

                // Generate live plane
                // useFloor > -1 mean current Y is over 1st floor, then use cache
                if (useFloor > -1)
                {
                    for (int cDeg = 0; cDeg < degCount; cDeg++)
                    {
                        Poly2Mesh.Polygon poly = new Poly2Mesh.Polygon();
                        if (cDeg + 1 == degCount)
                        {
                            poly.outside = new List<Vector3>()
                {
                    fVerties[cDeg][useFloor + 1],
                    cVerties[cDeg],
                    cVerties[0],
                    fVerties[0][useFloor + 1],
                };
                        }
                        else
                        {
                            poly.outside = new List<Vector3>()
                {
                    fVerties[cDeg][useFloor + 1],
                    cVerties[cDeg],
                    cVerties[cDeg + 1],
                    fVerties[cDeg + 1][useFloor + 1],
                };
                        }

                        meshes.Add(Poly2Mesh.CreateMesh(poly));
                    }
                }
                // this is userFloor <= -1 mean current Y is below 1st floor, then generate with floor=0
                else
                {
                    for (int cDeg = 0; cDeg < degCount; cDeg++)
                    {
                        Poly2Mesh.Polygon poly = new Poly2Mesh.Polygon();
                        if (cDeg + 1 == degCount)
                        {
                            poly.outside = new List<Vector3>()
                {
                    fVerties[cDeg][0],
                    cVerties[cDeg],
                    cVerties[0],
                    fVerties[0][0],
                };
                        }
                        else
                        {
                            poly.outside = new List<Vector3>()
                {
                    fVerties[cDeg][0],
                    cVerties[cDeg],
                    cVerties[cDeg + 1],
                    fVerties[cDeg + 1][0],
                };
                        }

                        meshes.Add(Poly2Mesh.CreateMesh(poly));
                    }
                }

                // Generate Top
                Poly2Mesh.Polygon poly2 = new Poly2Mesh.Polygon();
                List<Vector3> poly2Vecs = new List<Vector3>(cVerties);
                poly2Vecs.Reverse();
                poly2.outside = poly2Vecs;
                meshes.Add(Poly2Mesh.CreateMesh(poly2));

                // Add bottom cache meshes
                meshes.AddRange(cacheBottomMeshes);
            }

            // Combine meshes
            CombineInstance[] combine = new CombineInstance[meshes.Count];
            int i = 0;
            foreach (Mesh mesh in meshes)
            {
                combine[i].mesh = mesh;
                combine[i].transform = transform.localToWorldMatrix;
                i++;
            }

            Mesh oMesh = new Mesh();
            oMesh.CombineMeshes(combine);

            // Recalculate verties to match object
            List<Vector3> combineVerties = new List<Vector3>();

            foreach (Vector3 vertex in oMesh.vertices)
            {
                Vector3 vecTest = transform.InverseTransformPoint(vertex);
                combineVerties.Add(vecTest);
            }

            oMesh.vertices = combineVerties.ToArray();
            oMesh.RecalculateBounds();
            oMesh.RecalculateNormals();

            return oMesh;
        }

    }
}
