using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Player TargetPlayer;
    public float BoomHeight = 10.0f;
    public float BoomDistance = 20.0f;
    public float BoomAngle = 30.0f;

    void Update()
    {
        if(TargetPlayer)
        {
            Vector3 position = new Vector3(TargetPlayer.transform.position.x, TargetPlayer.transform.position.y + BoomHeight, TargetPlayer.transform.position.z - BoomDistance);
            transform.position = position;
            transform.rotation = Quaternion.AngleAxis(BoomAngle, Vector3.right);
        }
    }
}
