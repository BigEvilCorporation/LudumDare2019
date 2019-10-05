using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOverlay : MonoBehaviour
{
    Player Player1;

    void Start()
    {
        Player1 = FindObjectOfType<Player>();
    }

    void OnGUI()
    {
        float x = 10.0f;
        float y = 10.0f;
        float lineWidth = Screen.width;
        float lineHeight = 20.0f;
        Rect rect = new Rect(x, y, lineWidth, lineHeight);

        GUI.Label(rect, "Input left: " + Player1.StickInputLeft.ToString());
        rect.y += lineHeight;
        GUI.Label(rect, "Input right: " + Player1.StickInputRight.ToString());
        rect.y += lineHeight;
        GUI.Label(rect, "Player pos: " + Player1.transform.position.ToString());
        rect.y += lineHeight;
        GUI.Label(rect, "Player vel: " + Player1.Velocity.ToString());
    }
}
