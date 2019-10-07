using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStreamer : MonoBehaviour
{
    [System.Serializable]
    public struct Track
    {
        public AudioClip[] Clips;
    }

    public Track CurrentTrack;

    private int m_numAudioSources = 2;
    private AudioSource[] m_audioSources;
    private int m_nextSourceIdx;
    private AudioClip m_currentClip;
    private double m_nextStartTime;
    private bool m_playing = false;

    public void BeginPlayback()
    {
        //Get initial start time
        m_nextStartTime = AudioSettings.dspTime + 0.2;

        //Schedule two clips
        ScheduleRandomClip();
        ScheduleRandomClip();

        m_playing = true;
    }

    void ScheduleRandomClip()
    {
        //Get next clip
        m_currentClip = CurrentTrack.Clips[(int)Random.Range(0, CurrentTrack.Clips.Length)];

        //Schedule playback
        m_audioSources[m_nextSourceIdx].clip = m_currentClip;
        m_audioSources[m_nextSourceIdx].PlayScheduled(m_nextStartTime);

        //Next source
        m_nextSourceIdx = (m_nextSourceIdx + 1) % m_numAudioSources;
        m_nextStartTime += (double)m_currentClip.samples / m_currentClip.frequency;

    }
    
    void Start()
    {
        m_audioSources = new AudioSource[m_numAudioSources];
        for (int i = 0; i < m_numAudioSources; i++)
        {
            m_audioSources[i] = gameObject.AddComponent<AudioSource>();
        }
    }
    
    void Update()
    {
        if (m_playing && AudioSettings.dspTime > m_nextStartTime - 1)
        {
            ScheduleRandomClip();
        }
    }
}
