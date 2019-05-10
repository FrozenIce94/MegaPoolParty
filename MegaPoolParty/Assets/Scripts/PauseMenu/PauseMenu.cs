using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    private bool m_isAxisInUse = false;

    public GameObject pauseMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Cancel") == 1 && m_isAxisInUse == false)
        {
            if (GameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }

            m_isAxisInUse = true;
        }
        if(Input.GetAxisRaw("Cancel") == 0)
        {
            m_isAxisInUse = false;
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        GameIsPaused = true;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
