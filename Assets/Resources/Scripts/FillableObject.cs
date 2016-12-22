

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chemistrecipe
{
    public class FillableObject : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (transform.forward.y < -0.2f)
            {
                Debug.Log(gameObject.name + " is Facing down " + transform.forward);
            }
        }

    }
}
