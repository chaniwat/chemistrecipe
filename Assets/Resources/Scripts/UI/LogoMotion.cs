using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoMotion : MonoBehaviour {

    public Texture[] textures;
    public float framesPerSecond = 0.33F;
    public Renderer rend;


	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (textures.Length == 0)
            return;

        int index = Mathf.FloorToInt(Time.time / framesPerSecond);
        index = index % textures.Length;
        rend.material.mainTexture = textures[index];
        
    }
}
