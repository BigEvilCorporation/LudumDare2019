using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOverlay : MonoBehaviour
{
    Player m_player;
    Sucker m_sucker;

    void Start()
    {
        m_player = FindObjectOfType<Player>();
        m_sucker = FindObjectOfType<Sucker>();
    }

    void OnGUI()
    {
        float x = 10.0f;
        float y = 10.0f;
        float lineWidth = Screen.width;
        float lineHeight = 20.0f;
        Rect rect = new Rect(x, y, lineWidth, lineHeight);

        GUI.Label(rect, "Input left: " + m_player.StickInputLeft.ToString());
        rect.y += lineHeight;
        GUI.Label(rect, "Input right: " + m_player.StickInputRight.ToString());
        rect.y += lineHeight;
        GUI.Label(rect, "Player pos: " + m_player.transform.position.ToString());
        rect.y += lineHeight;
        GUI.Label(rect, "Player vel: " + m_player.Velocity.ToString());
        rect.y += lineHeight;
        GUI.Label(rect, "Objects being sucked: " + m_sucker.ObjectCount);
    }
}
