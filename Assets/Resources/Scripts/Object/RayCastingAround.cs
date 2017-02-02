using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RayCastingAround : MonoBehaviour {

    public float dynamicY;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;

        for (int counter = 0; counter < 64; counter++)
        {
            float deg = (360f / 64) * (counter + 1);
            float radian = deg * Mathf.Deg2Rad;
            float cos = Mathf.Cos(radian);
            float sin = Mathf.Sin(radian);
            Vector3 direction = new Vector3(1 * cos - 0 * sin, 0, 1 * sin + 0 * cos);

            Vector3 castPosition = transform.TransformPoint(new Vector3(transform.localPosition.x, transform.localPosition.y + dynamicY, transform.localPosition.z));

            if (Physics.Raycast(castPosition, transform.parent.TransformDirection(direction), out hit))
            {
                // Vector3 magicVector = new Vector3(hit.point.x - transform.position.x, hit.point.y - transform.position.y, hit.point.z - transform.position.z);
                // Vector3 localHitPoint = hit.transform.InverseTransformPoint(hit.point);

                Debug.DrawLine(castPosition, hit.point, Color.green);

                //world
                //print(theDistance + " " + string.Format("({0}, {1}, {2})", hit.point.x, hit.point.y, hit.point.z) + " " + hit.collider.gameObject.name);

                //local
                //print(string.Format("({0}, {1}, {2})", localHitPoint.x, localHitPoint.y, localHitPoint.z) + " " + hit.collider.gameObject.name);
            }
        }
    }

}
