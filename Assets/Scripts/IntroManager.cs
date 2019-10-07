using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public GameObject m_image02;
    public GameObject m_image03;
    public AudioClip Sound1;
    public AudioClip Sound2;
    public AudioClip Sound3;

    float m_timer = 0f;

    bool isActive01 = false;
    bool isActive02 = false;
    bool isChangingScene = false;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = Sound1;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;

        if (m_timer >= 1f && !isActive01)
        {
            isActive01 = true;
            m_image02.SetActive(true);
            audioSource.clip = Sound2;
            audioSource.Play();

            return;
        }

        if (m_timer >= 2f && !isActive02)
        {
            isActive02 = true;
            m_image03.SetActive(true);
            audioSource.clip = Sound3;
            audioSource.Play();

            return;
        }

        if (m_timer >= 5f && !isChangingScene)
        {
            isChangingScene = true;
            SceneManager.LoadScene(1);
        }
    }
}
