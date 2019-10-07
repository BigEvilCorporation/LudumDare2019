using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [System.Serializable]
    public class EvolutionStage
    {
        public string Name;
        public int AttackDamage;
        public float AttackVelocityScale;
        public int EnergyUntilNext;
        public float ScaleMin;
        public float ScaleMax;
        public float MoveSpeedScale;
        public Sprite Avatar;
    }

    public enum State
    {
        Spawning,
        Spawned
    }

    public Vector2 MoveSpeed = new Vector2(6.0f, 6.0f);
    public Vector2 Drag = new Vector2(10.0f, 10.0f);
    public Vector2 DeadZoneLeft = new Vector2(0.4f, 0.4f);
    public float DeadZoneRight = 0.1f;
    public float DeadZoneFire = 0.1f;
    public float Gravity = 9.8f;
    public float FireTime = 0.5f;
    public float StartTime = 2.0f;
    public int MaxAudioSources = 4;
    public GameObject BulletPrefab;
    public EnemySpawner EnemySpawner;
    public GameObject ExperienceUI;
    public EvolutionStage[] EvolutionStages;

    public AudioClip[] SFX_Strain;
    public AudioClip[] SFX_Pop;
    public AudioClip[] SFX_Spit;
    public AudioClip[] SFX_LevelUp;

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

    public EvolutionStage CurrentEvolution
    {
        get { return EvolutionStages[m_currentStageIdx]; }
    }

    public State CurrentState
    {
        get { return m_state; }
    }

    private Vector2 m_stickInputLeft;
    private Vector2 m_stickInputRight;
    private Vector2 m_initialMoveSpeed;
    private Vector3 m_velocity;
    private float m_fireTimer;
    private State m_state = State.Spawning;
    private SpriteRenderer m_sprite;
    private CharacterController m_characterController;
    private Sucker m_sucker;
    private AudioSource[] m_audioSources;
    private AudioSource m_audioSourceSpit;
    private int m_currentStageIdx;
    private int m_currentStageEnergy;
    private float m_startTimer;
    private UnityEngine.UI.Slider m_experienceSlider;

    void Start()
    {
        m_sprite = GetComponentInChildren<SpriteRenderer>();
        m_characterController = GetComponent<CharacterController>();
        m_sucker = GetComponentInChildren<Sucker>();
        m_audioSourceSpit = gameObject.AddComponent<AudioSource>();
        m_experienceSlider = ExperienceUI.GetComponentInChildren<UnityEngine.UI.Slider>();
        m_initialMoveSpeed = MoveSpeed;
        m_state = State.Spawning;
        
        m_audioSources = new AudioSource[MaxAudioSources];
        for(int i = 0; i < MaxAudioSources; i++)
        {
            m_audioSources[i] = gameObject.AddComponent<AudioSource>();
        }

        SetEvolutionStage(0);

        //Start invisible, play strain SFX
        m_sprite.enabled = false;
        m_startTimer = StartTime;
        PlaySFX(SFX_Strain[(int)Random.Range(0, SFX_Strain.Length)]);
    }

    void SetEvolutionStage(int index)
    {
        //Play SFX
        if (m_currentStageIdx != index)
        {
            PlaySFX(SFX_LevelUp[(int)Random.Range(0, SFX_LevelUp.Length)]);
        }

        //Set stage index and reset energy counter
        m_currentStageIdx = index;
        m_currentStageEnergy = 0;

        //Set new avatar and scale
        EvolutionStage stage = CurrentEvolution;
        m_sprite.sprite = stage.Avatar;
        transform.localScale = new Vector3(stage.ScaleMin, stage.ScaleMin, stage.ScaleMin);

        //Scale move speed
        MoveSpeed = m_initialMoveSpeed * stage.MoveSpeedScale;

        //Set enemy spawner to match
        EnemySpawner.SetEvolutionStage(index);

        //Update UI
        m_experienceSlider.value = 0.0f;
    }

    void AddEnergy(int energy)
    {
        //Add to counter
        m_currentStageEnergy += energy;

        //Increase scale
        EvolutionStage stage = CurrentEvolution;
        float stageTime = (float)m_currentStageEnergy / (float)stage.EnergyUntilNext;
        float scale = Mathf.Lerp(stage.ScaleMin, stage.ScaleMax, stageTime);
        m_sprite.transform.localScale = new Vector3(scale, scale, scale);

        //If hit next evolution stage, switch
        if (m_currentStageEnergy >= EvolutionStages[m_currentStageIdx].EnergyUntilNext)
        {
            SetEvolutionStage(m_currentStageIdx + 1);
        }

        //Update UI
        m_experienceSlider.value = (float)m_currentStageEnergy / (float)stage.EnergyUntilNext;
    }

    void PlaySFX(AudioClip clip)
    {
        //Find free audio source
        for(int i = 0; i < m_audioSources.Length; i++)
        {
            if(!m_audioSources[i].isPlaying)
            {
                m_audioSources[i].clip = clip;
                m_audioSources[i].Play();
            }
        }
    }

    void Update()
    {
        if (m_startTimer > 0.0f)
        {
            m_startTimer -= Time.deltaTime;
            if (m_startTimer <= 0.0f)
            {
                PlaySFX(SFX_Pop[(int)Random.Range(0, SFX_Pop.Length)]);
                m_sprite.enabled = true;
                m_state = State.Spawned;
            }
        }
        else
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

            //Apply velocity (scaled by current evolution)
            m_velocity += new Vector3(m_stickInputLeft.x * CurrentEvolution.MoveSpeedScale, -Gravity * Time.deltaTime, m_stickInputLeft.y * CurrentEvolution.MoveSpeedScale);

            //Apply drag
            m_velocity.x /= 1.0f + Drag.x * Time.deltaTime;
            m_velocity.z /= 1.0f + Drag.y * Time.deltaTime;

            m_characterController.Move(Velocity * Time.deltaTime);

            //Zero Y velocity if on floor
            if ((m_characterController.collisionFlags & CollisionFlags.Below) != 0)
            {
                m_velocity.y = 0.0f;
            }

            //Calc rotation direction
            if (m_stickInputRight.SqrMagnitude() > DeadZoneRight)
            {
                Vector2 directionVector = m_stickInputRight.normalized;
                Quaternion directionQuat = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1.0f), new Vector3(-directionVector.x, 0.0f, directionVector.y));

                //Apply rotation
                transform.rotation = directionQuat;
            }

            //Fire spitballs
            m_fireTimer -= Time.deltaTime;

            if (m_fireTimer <= 0.0f && Input.GetAxis("Fire1") > DeadZoneFire)
            {
                m_fireTimer = FireTime;

                if (BulletPrefab)
                {
                    GameObject spitballObj = Instantiate(BulletPrefab, transform.position, transform.rotation) as GameObject;
                    Spitball spitball = spitballObj.GetComponent<Spitball>();
                    spitball.Damage = CurrentEvolution.AttackDamage;
                    spitball.VelocityScale = CurrentEvolution.AttackVelocityScale;

                    if (!m_audioSourceSpit.isPlaying)
                    {
                        m_audioSourceSpit.clip = SFX_Spit[(int)Random.Range(0, SFX_Spit.Length)];
                        m_audioSourceSpit.Play();
                    }
                }
            }

            //Suck up goo
            if (Input.GetAxis("Fire2") > DeadZoneFire)
            {
                m_sucker.StartSuck();
            }
            else
            {
                m_sucker.EndSuck();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            //If gooified enemy, consume
            if(enemy.CurrentState == Enemy.State.Goo)
            {
                //Take energy
                AddEnergy(enemy.Energy);

                //Remove enemy
                Destroy(collision.gameObject);

                //Play SFX
                PlaySFX(SFX_Pop[(int)Random.Range(0, SFX_Pop.Length)]);
            }
            else
            {
                //Not gooified, harm player
            }
        }
    }
}