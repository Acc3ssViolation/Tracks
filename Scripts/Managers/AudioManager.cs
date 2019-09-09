using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager main { get; private set; }

    public int maxSources = 14;

    void Awake()
    {
        if(main == null)
            main = this;

    }

    public void PlayClip(AudioClip clip, float pitch, float volume)
    {

    }
}
