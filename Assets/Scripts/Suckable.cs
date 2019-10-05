using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suckable : MonoBehaviour
{
    public int Health = 100;

    Rigidbody m_rigidBody;
    
    void Start()
    {
        m_rigidBody = GetComponentInParent<Rigidbody>();
        m_rigidBody.sleepThreshold = 0.0f;
    }
    
    void Update()
    {
        
    }

    public void ApplySuckForce(Vector3 force)
    {
        m_rigidBody.AddForce(force);
        m_rigidBody.WakeUp();
    }

    void OnTriggerEnter(Collider collision)
    {
        if(Health <= 0)
        {
            if (collision.gameObject.tag == "Sucker")
            {
                Sucker sucker = collision.gameObject.GetComponent<Sucker>();
                sucker.AddAffectedObject(this);
            }
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if(Health <= 0)
        {
            if (collision.gameObject.tag == "Sucker")
            {
                Sucker sucker = collision.gameObject.GetComponent<Sucker>();
                sucker.RemoveAffectedObject(this);
            }
        }
    }
}
