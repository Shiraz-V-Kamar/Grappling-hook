using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[System.Serializable]

public class Sound 
{
    // to appear all below property in editor make it serlizable
    public string name;
    public AudioClip clip;
    //range is creating slider 
    [Range(0f, 1f)]
    public float volume;
    [Range(1f, 3f)]
    public float pitch =1;
    [HideInInspector]
    public AudioSource source;
    public bool loop;
    public AudioMixerGroup mixerGroup;
}

