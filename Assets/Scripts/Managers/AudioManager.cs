using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource aS;

    public static AudioManager aMSingleton;

    void Awake()
    {
        aMSingleton = this;
    }

    public void PlayPitchedSound(AudioClip clip)
    {
        RandomizePitch();
        aS.PlayOneShot(clip);
    }

    public void PlayNormalSound(AudioClip clip)
    {
        aS.pitch = 1;
        aS.PlayOneShot(clip);
    }

    public void RandomizePitch()
    {
        aS.pitch = Random.Range(.85f,1.15f);
    }
}
