using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spitball : MonoBehaviour
{
    public float InitialVelocity = 50.0f;
    public float MaxDistance = 2000.0f;

    private enum State
    {
        Fly,
        Splat
    }

    private Vector3 m_velocity;
    private Vector3 m_startPosition;
    private State m_state;
    private Rigidbody m_rigidBody;

    void Start()
    {
        m_startPosition = transform.position;
        m_state = State.Fly;

        //Apply initial impulse
        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.AddForce((transform.rotation * Vector3.forward) * InitialVelocity, ForceMode.Impulse);
    }

    void Update()
    {
        //Destroy if out of bounds
        if ((transform.position - m_startPosition).sqrMagnitude > MaxDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
        {
            //Hit enemy
        }
        else
        {
            //Assume hit floor, become splatter
            m_state = State.Splat;

            //Stop
            m_rigidBody.velocity = Vector3.zero;
            m_rigidBody.angularVelocity = Vector3.zero;
        }
    }
}
