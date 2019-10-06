using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EvolutionStage
    {
        public string Name;
        public int AttackDamage;
        public float ScaleMin;
        public float ScaleMax;
        public float RandSpawnTimeMin;
        public float RandSpawnTimeMax;
        public Sprite SpriteNormal;
        public Sprite SpriteGooified;
    }

    public Camera GameCamera;
    public GameObject EnemyPrefab;
    public EvolutionStage[] EvolutionStages;

    public EvolutionStage CurrentEvolution
    {
        get { return EvolutionStages[m_currentStageIdx]; }
    }

    float m_nextSpawnTime;
    int m_currentStageIdx;

    public void SetEvolutionStage(int index)
    {
        m_currentStageIdx = index;
        EvolutionStage stage = CurrentEvolution;
        m_nextSpawnTime = CalculateSpawnTime(stage);
    }

    float CalculateSpawnTime(EvolutionStage stage)
    {
        return stage.RandSpawnTimeMin + (Random.value * (stage.RandSpawnTimeMax - stage.RandSpawnTimeMin));
    }

    void Spawn()
    {
        //Calc random position outside of camera view
        float cameraViewHeight = GameCamera.orthographicSize * 2.0f;
        float cameraViewWidth = cameraViewHeight * ((float)Screen.width / (float)Screen.height);
        float cameraTop = GameCamera.transform.position.z - (cameraViewHeight / 2.0f);
        float cameraBottom = GameCamera.transform.position.z + (cameraViewHeight / 2.0f);
        float cameraLeft = GameCamera.transform.position.x - (cameraViewWidth / 2.0f);
        float cameraRight = GameCamera.transform.position.x + (cameraViewWidth / 2.0f);

        Vector3 position = Vector3.zero;
        position.y = transform.position.y;

        float perimeter = (cameraViewWidth * 2.0f) + (cameraViewHeight * 2.0f);
        float perimeterDistanceNormalised = Random.value;
        float perimeterDistance = perimeter * perimeterDistanceNormalised;

        float topEdgeStart = 0.0f;
        float rightEdgeStart = topEdgeStart + cameraViewWidth;
        float bottomEdgeStart = rightEdgeStart + cameraViewHeight;
        float leftEdgeStart = bottomEdgeStart + cameraViewWidth;

        if (perimeterDistance > leftEdgeStart)
        {
            //On left edge
            position.x = cameraLeft;
            position.z = cameraTop + perimeterDistance - leftEdgeStart;
        }
        else if(perimeterDistance > bottomEdgeStart)
        {
            //On bottom edge
            position.x = cameraLeft + perimeterDistance - bottomEdgeStart;
            position.z = cameraBottom;
        }
        else if (perimeterDistance > rightEdgeStart)
        {
            //On right edge
            position.x = cameraRight;
            position.z = cameraTop + perimeterDistance - rightEdgeStart;
        }
        else
        {
            //On top edge
            position.x = cameraLeft + perimeterDistance;
            position.z = cameraTop;
        }

        //Create enemy
        GameObject enemyObj = Instantiate(EnemyPrefab, position, Quaternion.identity) as GameObject;
        Enemy enemy = enemyObj.GetComponent<Enemy>();

        //Set sprites
        enemy.SpriteNormal = CurrentEvolution.SpriteNormal;
        enemy.SpriteGooified = CurrentEvolution.SpriteGooified;
    }

    void Start()
    {
        SetEvolutionStage(0);
    }
    
    void Update()
    {
        m_nextSpawnTime -= Time.deltaTime;
        if(m_nextSpawnTime <= 0.0f)
        {
            Spawn();
            m_nextSpawnTime = CalculateSpawnTime(CurrentEvolution);
        }
    }
}
