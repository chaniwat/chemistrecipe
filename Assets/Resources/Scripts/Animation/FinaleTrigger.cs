using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinaleTrigger : MonoBehaviour {

    public Animator beaker;
    public Animator soap1;
    public Animator soap2;

	// Use this for initialization
	void Start () {
        StopAnimation();

    }

    public void PlayAnimation() {
        beaker.enabled = true;
        soap1.enabled = true;
        soap2.enabled = true;
    }

    public void StopAnimation() {
        beaker.enabled = false;
        soap1.enabled = false;
        soap2.enabled = false;
    }


}
