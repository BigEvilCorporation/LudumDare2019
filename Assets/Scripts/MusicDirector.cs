using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicDirector : MonoBehaviour
{
    public float StartTime = 0.5f;
    public Player WaitForPlayer;

    MusicStreamer m_musicStreamer;
    float m_startTimer;

    void Start()
    {
        m_musicStreamer = GetComponent<MusicStreamer>();
        m_startTimer = StartTime;
    }
    
    void Update()
    {
        if(m_startTimer > 0.0f && WaitForPlayer.CurrentState == Player.State.Spawned)
        {
            m_startTimer -= Time.deltaTime;
            if(m_startTimer < 0.0f)
            {
                m_musicStreamer.BeginPlayback();
            }
        }
    }
}
