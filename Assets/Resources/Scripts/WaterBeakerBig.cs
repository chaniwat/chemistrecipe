using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBeakerBig : MonoBehaviour {

    private MeshFilter mf;
    private List<Vector3> ls_vector3 = new List<Vector3>();
    private LiquidInBottle meshGenerator;

    public float waterLevel = 0f;

    #region verties settings

    private Vector3[] floorZeroVerties = new Vector3[]
    {
        new Vector3(0.11876f, 0.00724f, 0.01170f),
        new Vector3(0.11572f, 0.00724f, 0.02302f),
        new Vector3(0.11273f, 0.00724f, 0.03420f),
        new Vector3(0.10971f, 0.00724f, 0.04544f),
        new Vector3(0.10662f, 0.00724f, 0.05699f),
        new Vector3(0.09981f, 0.00724f, 0.06669f),
        new Vector3(0.09145f, 0.00724f, 0.07505f),
        new Vector3(0.08325f, 0.00724f, 0.08325f),
        new Vector3(0.07505f, 0.00724f, 0.09145f),
        new Vector3(0.06669f, 0.00724f, 0.09981f),
        new Vector3(0.05699f, 0.00724f, 0.10662f),
        new Vector3(0.04544f, 0.00724f, 0.10971f),
        new Vector3(0.03420f, 0.00724f, 0.11273f),
        new Vector3(0.02302f, 0.00724f, 0.11572f),
        new Vector3(0.01170f, 0.00724f, 0.11876f),
        new Vector3(0.00000f, 0.00724f, 0.12189f),
        new Vector3(-0.01170f, 0.00724f, 0.11876f),
        new Vector3(-0.02302f, 0.00724f, 0.11572f),
        new Vector3(-0.03420f, 0.00724f, 0.11273f),
        new Vector3(-0.04544f, 0.00724f, 0.10971f),
        new Vector3(-0.05699f, 0.00724f, 0.10662f),
        new Vector3(-0.06669f, 0.00724f, 0.09981f),
        new Vector3(-0.07505f, 0.00724f, 0.09145f),
        new Vector3(-0.08325f, 0.00724f, 0.08325f),
        new Vector3(-0.09145f, 0.00724f, 0.07505f),
        new Vector3(-0.09981f, 0.00724f, 0.06669f),
        new Vector3(-0.10662f, 0.00724f, 0.05699f),
        new Vector3(-0.10971f, 0.00724f, 0.04544f),
        new Vector3(-0.11273f, 0.00724f, 0.03420f),
        new Vector3(-0.11572f, 0.00724f, 0.02302f),
        new Vector3(-0.11876f, 0.00724f, 0.01170f),
        new Vector3(-0.12189f, 0.00724f, 0.00000f),
        new Vector3(-0.11876f, 0.00724f, -0.01170f),
        new Vector3(-0.11572f, 0.00724f, -0.02302f),
        new Vector3(-0.11273f, 0.00724f, -0.03420f),
        new Vector3(-0.10971f, 0.00724f, -0.04544f),
        new Vector3(-0.10662f, 0.00724f, -0.05699f),
        new Vector3(-0.09981f, 0.00724f, -0.06669f),
        new Vector3(-0.09145f, 0.00724f, -0.07505f),
        new Vector3(-0.08325f, 0.00724f, -0.08325f),
        new Vector3(-0.07505f, 0.00724f, -0.09145f),
        new Vector3(-0.06669f, 0.00724f, -0.09981f),
        new Vector3(-0.05699f, 0.00724f, -0.10662f),
        new Vector3(-0.04544f, 0.00724f, -0.10971f),
        new Vector3(-0.03420f, 0.00724f, -0.11273f),
        new Vector3(-0.02302f, 0.00724f, -0.11572f),
        new Vector3(-0.01170f, 0.00724f, -0.11876f),
        new Vector3(0.00000f, 0.00724f, -0.12189f),
        new Vector3(0.01170f, 0.00724f, -0.11876f),
        new Vector3(0.02302f, 0.00724f, -0.11572f),
        new Vector3(0.03420f, 0.00724f, -0.11273f),
        new Vector3(0.04544f, 0.00724f, -0.10971f),
        new Vector3(0.05699f, 0.00724f, -0.10662f),
        new Vector3(0.06669f, 0.00724f, -0.09981f),
        new Vector3(0.07505f, 0.00724f, -0.09145f),
        new Vector3(0.08325f, 0.00724f, -0.08325f),
        new Vector3(0.09145f, 0.00724f, -0.07505f),
        new Vector3(0.09981f, 0.00724f, -0.06669f),
        new Vector3(0.10662f, 0.00724f, -0.05699f),
        new Vector3(0.10971f, 0.00724f, -0.04544f),
        new Vector3(0.11273f, 0.00724f, -0.03420f),
        new Vector3(0.11572f, 0.00724f, -0.02302f),
        new Vector3(0.11876f, 0.00724f, -0.01170f),
        new Vector3(0.12189f, 0.00724f, 0.00000f),
    };

    private Vector3[] floorOneVerties = new Vector3[]
    {
        new Vector3(0.12140f, 0.18624f, 0.01196f),
        new Vector3(0.11830f, 0.18624f, 0.02353f),
        new Vector3(0.11523f, 0.18624f, 0.03496f),
        new Vector3(0.11215f, 0.18624f, 0.04646f),
        new Vector3(0.10899f, 0.18624f, 0.05826f),
        new Vector3(0.10203f, 0.18624f, 0.06818f),
        new Vector3(0.09349f, 0.18624f, 0.07672f),
        new Vector3(0.08510f, 0.18624f, 0.08510f),
        new Vector3(0.07672f, 0.18624f, 0.09349f),
        new Vector3(0.06818f, 0.18624f, 0.10203f),
        new Vector3(0.05826f, 0.18624f, 0.10899f),
        new Vector3(0.04646f, 0.18624f, 0.11216f),
        new Vector3(0.03496f, 0.18624f, 0.11524f),
        new Vector3(0.02353f, 0.18624f, 0.11830f),
        new Vector3(0.01196f, 0.18624f, 0.12140f),
        new Vector3(0.00000f, 0.18624f, 0.12461f),
        new Vector3(-0.01196f, 0.18624f, 0.12140f),
        new Vector3(-0.02353f, 0.18624f, 0.11830f),
        new Vector3(-0.03496f, 0.18624f, 0.11523f),
        new Vector3(-0.04646f, 0.18624f, 0.11215f),
        new Vector3(-0.05826f, 0.18624f, 0.10899f),
        new Vector3(-0.06818f, 0.18624f, 0.10203f),
        new Vector3(-0.07672f, 0.18624f, 0.09349f),
        new Vector3(-0.08510f, 0.18624f, 0.08510f),
        new Vector3(-0.09349f, 0.18624f, 0.07672f),
        new Vector3(-0.10203f, 0.18624f, 0.06818f),
        new Vector3(-0.10899f, 0.18624f, 0.05826f),
        new Vector3(-0.11215f, 0.18624f, 0.04646f),
        new Vector3(-0.11523f, 0.18624f, 0.03496f),
        new Vector3(-0.11830f, 0.18624f, 0.02353f),
        new Vector3(-0.12140f, 0.18624f, 0.01196f),
        new Vector3(-0.12460f, 0.18624f, 0.00000f),
        new Vector3(-0.12140f, 0.18624f, -0.01196f),
        new Vector3(-0.11830f, 0.18624f, -0.02353f),
        new Vector3(-0.11523f, 0.18624f, -0.03496f),
        new Vector3(-0.11215f, 0.18624f, -0.04646f),
        new Vector3(-0.10899f, 0.18624f, -0.05826f),
        new Vector3(-0.10203f, 0.18624f, -0.06818f),
        new Vector3(-0.09349f, 0.18624f, -0.07672f),
        new Vector3(-0.08510f, 0.18624f, -0.08510f),
        new Vector3(-0.07672f, 0.18624f, -0.09349f),
        new Vector3(-0.06818f, 0.18624f, -0.10203f),
        new Vector3(-0.05826f, 0.18624f, -0.10899f),
        new Vector3(-0.04646f, 0.18624f, -0.11215f),
        new Vector3(-0.03496f, 0.18624f, -0.11523f),
        new Vector3(-0.02353f, 0.18624f, -0.11830f),
        new Vector3(-0.01196f, 0.18624f, -0.12140f),
        new Vector3(0.00000f, 0.18624f, -0.12460f),
        new Vector3(0.01196f, 0.18624f, -0.12140f),
        new Vector3(0.02353f, 0.18624f, -0.11830f),
        new Vector3(0.03496f, 0.18624f, -0.11523f),
        new Vector3(0.04646f, 0.18624f, -0.11215f),
        new Vector3(0.05826f, 0.18624f, -0.10899f),
        new Vector3(0.06818f, 0.18624f, -0.10203f),
        new Vector3(0.07672f, 0.18624f, -0.09349f),
        new Vector3(0.08510f, 0.18624f, -0.08510f),
        new Vector3(0.09349f, 0.18624f, -0.07672f),
        new Vector3(0.10203f, 0.18624f, -0.06818f),
        new Vector3(0.10899f, 0.18624f, -0.05826f),
        new Vector3(0.11215f, 0.18624f, -0.04646f),
        new Vector3(0.11523f, 0.18624f, -0.03496f),
        new Vector3(0.11830f, 0.18624f, -0.02353f),
        new Vector3(0.12140f, 0.18624f, -0.01196f),
        new Vector3(0.12460f, 0.18624f, 0.00000f),
    };

    #endregion

    void Start()
    {
        mf = GetComponent<MeshFilter>();

        ls_vector3 = new List<Vector3>();

        Dictionary<int, Vector3>[] fVerties = new Dictionary<int, Vector3>[64];

        for (int i = 0; i < 64; i++)
        {
            fVerties[i] = new Dictionary<int, Vector3>()
            {
                { 0, floorZeroVerties[i] },
                { 1, floorOneVerties[i] },
            };
        }

        meshGenerator = new LiquidInBottle(2, 64, fVerties);
    }

    void Update()
    {
        ls_vector3.Clear();
        
        RaycastHit hit;

        Vector3[] cVerties = new Vector3[64];

        for (int counter = 0; counter < 64; counter++)
        {
            float deg = (360f / 64) * (counter + 1);
            float radian = deg * Mathf.Deg2Rad;
            float cos = Mathf.Cos(radian);
            float sin = Mathf.Sin(radian);
            Vector3 direction = new Vector3(1 * cos - 0 * sin, 0, 1 * sin + 0 * cos);

            Vector3 castPosition = transform.TransformPoint(new Vector3(transform.localPosition.x, transform.localPosition.y + waterLevel, transform.localPosition.z));

            if (Physics.Raycast(castPosition, transform.parent.TransformDirection(direction), out hit))
            {
                Vector3 magicVector = new Vector3(hit.point.x - transform.position.x, hit.point.y - transform.position.y, hit.point.z - transform.position.z);
                Vector3 localHitPoint = hit.collider.gameObject.transform.InverseTransformPoint(hit.point);

                Debug.DrawLine(castPosition, hit.point, Color.green);

                ls_vector3.Add(localHitPoint);
                cVerties[counter] = localHitPoint;
            }
        }

        mf.mesh.Clear();
        mf.mesh = meshGenerator.GenerateMesh(transform, cVerties);
    }

    public void copyVerties()
    {
        string stringBuffer = "";

        foreach (Vector3 vertex in ls_vector3)
        {
            stringBuffer += "new Vector3(" + vertex.x.ToString("0.00000") + "f, " + vertex.y.ToString("0.00000") + "f, " + vertex.z.ToString("0.00000") + "f),\n";
        }

        Debug.Log(stringBuffer);
    }

}
