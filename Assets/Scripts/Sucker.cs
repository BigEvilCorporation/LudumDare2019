using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sucker : MonoBehaviour
{
    public float SuckForce = 200.0f;
    public int ObjectCount
    {
        get { return m_affectedObjects.Count; }
    }

    BoxCollider m_collision;
    SpriteRenderer m_sprite;
    List<Suckable> m_affectedObjects = new List<Suckable>();

    public void StartSuck()
    {
        m_collision.enabled = true;
        m_sprite.enabled = true;
    }

    public void EndSuck()
    {
        m_collision.enabled = false;
        m_sprite.enabled = false;
        m_affectedObjects.Clear();
    }

    public void AddAffectedObject(Suckable suckable)
    {
        m_affectedObjects.Add(suckable);
    }

    public void RemoveAffectedObject(Suckable suckable)
    {
        m_affectedObjects.Remove(suckable);
    }

    void Start()
    {
        m_collision = GetComponent<BoxCollider>();
        m_sprite = GetComponent<SpriteRenderer>();
        m_collision.enabled = false;
        m_sprite.enabled = false;
    }
    
    void Update()
    {
        //Pull in enemies
        foreach(Suckable suckable in m_affectedObjects)
        {
            if(suckable)
            {
                float distance = (transform.parent.position - suckable.transform.position).magnitude;
                Vector3 force = (transform.parent.position - suckable.transform.position).normalized * (1.0f / distance) * SuckForce;
                force.y = 0.0f;
                suckable.ApplySuckForce(force);
            }
        }
    }
}
