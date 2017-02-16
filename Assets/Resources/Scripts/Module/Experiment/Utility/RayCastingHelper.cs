using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistRecipe.Utility
{
    public static class RayCastingHelper
    {
        public const int DEFAULT_DEG_COUNT = 32;

        /// <summary>
        /// Return the hit verties in the local position of hit object
        /// </summary>
        public static Vector3[] CastingAround(Transform originTransform, float yOffset, int degCount, bool drawDebugCast)
        {
            Vector3[] cVerties = new Vector3[degCount];

            RaycastHit hit;
            Vector3 castPosition = originTransform.TransformPoint(
                new Vector3(originTransform.localPosition.x, originTransform.localPosition.y + yOffset, originTransform.localPosition.z)
            );

            // Casting 360*
            for (int counter = 0; counter < 32; counter++)
            {
                float deg = (360f / 32) * (counter + 1);
                float radian = deg * Mathf.Deg2Rad;
                float cos = Mathf.Cos(radian);
                float sin = Mathf.Sin(radian);
                Vector3 direction = originTransform.parent.TransformDirection(new Vector3(1 * cos - 0 * sin, 0, 1 * sin + 0 * cos));

                if (Physics.Raycast(castPosition, direction, out hit))
                {
                    Vector3 localHitPoint = hit.transform.InverseTransformPoint(hit.point);

                    if (drawDebugCast)
                    {
                        Debug.DrawLine(castPosition, hit.point, Color.green);
                    }

                    cVerties[counter] = localHitPoint;
                }
            }

            return cVerties;
        }

        /// <summary>
        /// Return the hit verties in the local position of hit object
        /// </summary>
        public static Vector3[] CastingAround(Transform originTransform, float yOffset, int degCount)
        {
            return CastingAround(originTransform, yOffset, degCount, false);
        }

        /// <summary>
        /// Return the hit verties in the local position of hit object (use default degCount)
        /// </summary>
        public static Vector3[] CastingAround(Transform originTransform, float yOffset, bool drawDebugCast)
        {
            return CastingAround(originTransform, yOffset, DEFAULT_DEG_COUNT, drawDebugCast);
        }
    }
}
