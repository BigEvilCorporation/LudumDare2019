using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spitball : MonoBehaviour
{
    public float InitialVelocity = 50.0f;
    public float MaxDistance = 2000.0f;
    public float FadeStartTime = 2.0f;
    public float FadeSpeed = 0.3f;
    public int Damage = 20;
    public Sprite FloorSplatSprite;

    private enum State
    {
        Fly,
        Splat
    }

    private Vector3 m_velocity;
    private Vector3 m_startPosition;
    private State m_state;
    private float m_fadeTimer;
    private Rigidbody m_rigidBody;
    private SphereCollider m_collision;
    private SpriteRenderer m_sprite;

    public void Splat()
    {
        //Become splatter
        m_state = State.Splat;

        //Stop
        m_rigidBody.velocity = Vector3.zero;
        m_rigidBody.angularVelocity = Vector3.zero;

        //Un-physicalise
        m_rigidBody.isKinematic = true;
        m_collision.enabled = false;

        //Start fade timer
        m_fadeTimer = FadeStartTime;

        //Switch sprite
        m_sprite.sprite = FloorSplatSprite;
    }

    void Start()
    {
        m_startPosition = transform.position;
        m_state = State.Fly;
        m_rigidBody = GetComponent<Rigidbody>();
        m_collision = GetComponent<SphereCollider>();
        m_sprite = GetComponentInChildren<SpriteRenderer>();

        //Apply initial impulse
        m_rigidBody.AddForce((transform.rotation * Vector3.forward) * InitialVelocity, ForceMode.Impulse);
    }

    void Update()
    {
        if(m_state == State.Splat)
        {
            //Fade out start timer
            m_fadeTimer -= Time.deltaTime;

            //Fade if timer elapsed
            if(m_fadeTimer <= 0.0f)
            {
                //Fade
                Color colour = m_sprite.color;
                colour.a -= FadeSpeed * Time.deltaTime;
                m_sprite.color = colour;

                //Destroy if faded out
                if (colour.a <= 0.0f)
                {
                    Destroy(gameObject);
                }
            }
        }

        //Destroy if out of bounds
        if ((transform.position - m_startPosition).sqrMagnitude > MaxDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Enemy")
        {
            //Assume hit floor, become splatter
            Splat();
        }
    }
}
