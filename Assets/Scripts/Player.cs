using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 MoveSpeed = new Vector2(6.0f, 6.0f);
    public Vector2 Drag = new Vector2(10.0f, 10.0f);
    public Vector2 DeadZoneLeft = new Vector2(0.4f, 0.4f);
    public float DeadZoneRight = 0.1f;
    public float DeadZoneFire = 0.1f;
    public float Gravity = 9.8f;
    public float FireTime = 0.5f;
    public GameObject BulletPrefab;

    public Vector3 Velocity
    {
        get { return m_velocity; }
    }

    public Vector2 StickInputLeft
    {
        get { return m_stickInputLeft; }
    }

    public Vector2 StickInputRight
    {
        get { return m_stickInputRight; }
    }

    private Vector2 m_stickInputLeft;
    private Vector2 m_stickInputRight;
    private Vector3 m_velocity;
    private float m_fireTimer;
    private CharacterController m_characterController;
    
    void Start()
    {
        m_characterController = GetComponent<CharacterController>();
    }
    
    void Update()
    {
        //Grab input
        Vector2 stickInputLeft = new Vector2(Input.GetAxis("MoveX"), -Input.GetAxis("MoveY"));
        Vector2 stickInputRight = new Vector2(Input.GetAxisRaw("RotateX"), Input.GetAxisRaw("RotateY"));

        //Apply deadzone
        if (Mathf.Abs(stickInputLeft.x) < DeadZoneLeft.x)
            stickInputLeft.x = 0.0f;
        if (Mathf.Abs(stickInputLeft.y) < DeadZoneLeft.y)
            stickInputLeft.y = 0.0f;

        m_stickInputLeft = stickInputLeft;
        m_stickInputRight = stickInputRight;

        //Apply velocity
        m_velocity += new Vector3(m_stickInputLeft.x, -Gravity * Time.deltaTime, m_stickInputLeft.y);

        //Apply drag
        m_velocity.x /= 1.0f + Drag.x * Time.deltaTime;
        m_velocity.z /= 1.0f + Drag.y * Time.deltaTime;

        m_characterController.Move(Velocity * Time.deltaTime);

        //Zero Y velocity if on floor
        if((m_characterController.collisionFlags & CollisionFlags.Below) != 0)
        {
            m_velocity.y = 0.0f;
        }

        //Calc rotation direction
        if(m_stickInputRight.SqrMagnitude() > DeadZoneRight)
        {
            Vector2 directionVector = m_stickInputRight.normalized;
            Quaternion directionQuat = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1.0f), new Vector3(-directionVector.x, 0.0f, directionVector.y));

            //Apply rotation
            transform.rotation = directionQuat;
        }

        //Fire bullets
        m_fireTimer -= Time.deltaTime;

        if (m_fireTimer <= 0.0f && Input.GetAxis("Fire1") > DeadZoneFire)
        {
            m_fireTimer = FireTime;

            if(BulletPrefab)
            {
                Instantiate(BulletPrefab, transform.position, transform.rotation);
            }
        }
    }
}
