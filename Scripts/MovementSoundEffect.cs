using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(TrainController))]
public class MovementSoundEffect : MonoBehaviour
{
    public AudioClip m_clip;
    public float m_minVolume;
    public float m_minPitch;
    public float m_maxPitch;
    public float m_volumeScale;
    public float m_pitchScale;

    private TrainController m_train;
    private AudioSource m_source;

    void Awake()
    {
        m_train = GetComponent<TrainController>();
        m_source = GetComponent<AudioSource>();
        m_source.playOnAwake = false;
        m_source.loop = true;
        m_source.clip = m_clip;
    }

    void Update()
    {
        if(!m_source.isPlaying)
        {
            m_source.PlayDelayed(Random.Range(0.0f, 0.1f));
        }
        m_source.volume = Mathf.Min(m_minVolume + m_train.AbsVelocity * m_volumeScale, 1.0f);
        m_source.pitch = Mathf.Min(m_minPitch + m_train.AbsVelocity * m_pitchScale, m_maxPitch);
    }
}
