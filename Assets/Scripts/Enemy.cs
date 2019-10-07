using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Sprite SpriteNormal;
    public Sprite SpriteGooified;
    public Player TargetPlayer;
    public int Energy = 1;
    public float MoveSpeed;
    public Vector2 Drag = new Vector2(10.0f, 10.0f);

    public enum State
    {
        Living,
        Goo
    }

    public State CurrentState
    {
        get { return m_state; }
    }

    State m_state;
    Rigidbody m_rigidBody;
    SpriteRenderer m_sprite;
    Vector3 m_velocity;
    Suckable m_suckable;

    void Start()
    {
        m_state = State.Living;
        m_sprite = GetComponentInChildren<SpriteRenderer>();
        m_rigidBody = GetComponent<Rigidbody>();
        m_suckable = GetComponentInChildren<Suckable>();

        m_sprite.sprite = SpriteNormal;
    }
    
    void Update()
    {
        if(m_state == State.Living)
        {
            //Target player
            Vector3 direction = (TargetPlayer.transform.position - transform.position).normalized;

            //Apply velocity (scaled by current evolution)
            m_velocity += new Vector3(direction.x * MoveSpeed, 0.0f, direction.z * MoveSpeed);

            //Apply drag and gravity
            m_velocity.x /= 1.0f + Drag.x * Time.deltaTime;
            m_velocity.y = m_rigidBody.velocity.y;
            m_velocity.z /= 1.0f + Drag.y * Time.deltaTime;

            m_rigidBody.velocity = m_velocity;
        }
    }
    
    public void Gooify()
    {
        m_state = State.Goo;
        m_sprite.sprite = SpriteGooified;
    }

    void OnTriggerEnter(Collider collision)
    {
        //If collided with spitball, turn to goo
        if (collision.gameObject.tag == "Spitball")
        {
            Spitball spitball = collision.gameObject.GetComponent<Spitball>();
            if(spitball)
            {
                spitball.Splat();

                if (CurrentState == Enemy.State.Living)
                {
                    //Deal damage
                    m_suckable.Health -= spitball.Damage;

                    if (m_suckable.Health <= 0)
                    {
                        //Turn enemy into goo
                        Gooify();
                    }
                }
            }
        }
    }
}
