using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sucker : MonoBehaviour
{
    public float SuckForce = 200.0f;
    public float TextureScrollSpeed = 1.0f;
    public Material MeshMaterial;

    public int ObjectCount
    {
        get { return m_affectedObjects.Count; }
    }

    BoxCollider m_collision;
    Mesh m_mesh;
    MeshFilter m_meshFilter;
    MeshRenderer m_meshRenderer;
    List<Suckable> m_affectedObjects = new List<Suckable>();
    float m_textureScroll;

    public void StartSuck()
    {
        m_collision.enabled = true;
        m_meshRenderer.enabled = true;
    }

    public void EndSuck()
    {
        m_collision.enabled = false;
        m_meshRenderer.enabled = false;
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

        CreateMesh();
        
        m_collision.enabled = false;
        m_meshRenderer.enabled = false;
    }

    void CreateMesh()
    {
        Vector3 size = m_collision.size;

        m_mesh = new Mesh();
        m_meshFilter = gameObject.AddComponent<MeshFilter>();
        m_meshRenderer = gameObject.AddComponent<MeshRenderer>();

        Vector3[] verts = new Vector3[5];
        Vector2[] uvs = new Vector2[5];
        int[] tris = new int[6];

        verts[0] = new Vector3(-size.x / 2.0f, 0.0f, size.z / 2.0f);
        verts[1] = new Vector3(0.0f, 0.0f, size.z / 2.0f);
        verts[2] = new Vector3(size.x / 2.0f, 0.0f, size.z / 2.0f);
        verts[3] = new Vector3(0.0f, 0.0f, -size.z / 2.0f);
        verts[4] = new Vector3(0.0f, 0.0f, -size.z / 2.0f);

        uvs[0] = new Vector2(0.0f, 1.0f);
        uvs[1] = new Vector2(0.5f, 1.0f);
        uvs[2] = new Vector2(1.0f, 1.0f);
        uvs[3] = new Vector2(1.0f, 0.0f);
        uvs[4] = new Vector2(0.0f, 0.0f);

        tris[0] = 0;
        tris[1] = 1;
        tris[2] = 4;
        tris[3] = 1;
        tris[4] = 2;
        tris[5] = 3;

        m_mesh.name = "Sucker_Generated";
        m_mesh.vertices = verts;
        m_mesh.uv = uvs;
        m_mesh.triangles = tris;

        m_meshFilter.mesh = m_mesh;
        m_meshRenderer.material = MeshMaterial;
        m_meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
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

        //Scroll UV
        m_textureScroll += TextureScrollSpeed * Time.deltaTime;
        m_meshRenderer.material.mainTextureOffset = new Vector2(0.0f, m_textureScroll);
    }
}
