using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseUISoundController : MonoBehaviour {

    
    public AudioClip instructionUpdate;

    private AudioSource _audioSource;
    public AudioSource audioSource
    {
        get
        {
            return _audioSource;
        }
    }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void playInstructionUpdate()
    {
        audioSource.PlayOneShot(instructionUpdate);
    }

}
