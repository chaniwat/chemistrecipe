using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chemistrecipe
{
    /// <summary>
    /// Experiment for rendering water in bottle.
    /// (Mesh Editting & Vertex Creating)
    /// </summary>
    public class MeshTest : MonoBehaviour
    {

        // Model to test vertex
        public MeshFilter model;

        // Target to edit mesh
        public MeshFilter targetModel;

        // Use this for initialization
        void Start()
        {            
            List<Vector3> centerVerties = new List<Vector3>();

            foreach (Vector3 vertex in model.mesh.vertices)
            {
                Debug.Log(vertex);

                /*
                if (vertex.x == 0 && vertex.z == 0)
                {
                    centerVerties.Add(vertex);
                }
                */
            }

            foreach (Vector3 vertex in centerVerties)
            {
                Debug.Log(vertex);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void createCube()
        {

        }
    }
}
