using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Sprite SpriteNormal;
    public Sprite SpriteGooified;
    public int Energy = 1;

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
    
    void Start()
    {
        m_state = State.Living;
        m_sprite = GetComponentInChildren<SpriteRenderer>();
        m_rigidBody = GetComponent<Rigidbody>();

        m_sprite.sprite = SpriteNormal;
    }
    
    void Update()
    {
        
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
                    Suckable suckable = GetComponentInChildren<Suckable>();
                    if (suckable)
                    {
                        suckable.Health -= spitball.Damage;

                        if (suckable.Health <= 0)
                        {
                            //Turn enemy into goo
                            Gooify();
                        }
                    }
                }
            }
        }
    }
}
