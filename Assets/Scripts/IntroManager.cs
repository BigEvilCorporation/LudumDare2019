using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public GameObject m_image02;
    public GameObject m_image03;

    float m_timer = 0f;

    bool isActive01 = false;
    bool isActive02 = false;
    bool isChangingScene = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;

        if (m_timer >= 1f && !isActive01)
        {
            isActive01 = true;
            m_image02.SetActive(true);

            return;
        }

        if (m_timer >= 2f && !isActive02)
        {
            isActive02 = true;
            m_image03.SetActive(true);

            return;
        }

        if (m_timer >= 5f && !isChangingScene)
        {
            isChangingScene = true;
            SceneManager.LoadScene(1);
        }
    }
}
