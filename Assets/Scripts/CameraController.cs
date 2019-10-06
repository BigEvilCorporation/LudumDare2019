using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Player TargetPlayer;
    public float BoomHeight = 10.0f;
    public float BoomDistance = 20.0f;
    public float BoomAngle = 30.0f;
    public float ZoomTime = 2.0f;

    private Camera m_camera;
    float m_initialSize;
    float m_zoomTime = 1.0f;
    float m_zoomStart = 1.0f;
    float m_zoomTarget = 1.0f;

    void Start()
    {
        m_camera = GetComponent<Camera>();
        m_initialSize = m_camera.orthographicSize;
    }

    void Update()
    {
        if(TargetPlayer)
        {
            Vector3 position = new Vector3(TargetPlayer.transform.position.x, TargetPlayer.transform.position.y + BoomHeight, TargetPlayer.transform.position.z - BoomDistance);
            transform.position = position;
            transform.rotation = Quaternion.AngleAxis(BoomAngle, Vector3.right);

            //If evolution scale changed
            float cameraZoom = m_initialSize * TargetPlayer.CurrentEvolution.ScaleMax;
            if(cameraZoom != m_zoomTarget)
            {
                //Start new zoom lerp
                m_zoomStart = m_zoomTarget;
                m_zoomTarget = cameraZoom;
                m_zoomTime = 0.0f;
            }

            //Lerp camera
            m_zoomTime = Mathf.Clamp(m_zoomTime + Time.deltaTime, 0.0f, 1.0f);
            m_camera.orthographicSize = Mathf.SmoothStep(m_zoomStart, m_zoomTarget, m_zoomTime);
        }
    }
}
