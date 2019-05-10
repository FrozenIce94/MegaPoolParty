using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    

    private bool m_isAxisInUse = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Cancel") == 1 && m_isAxisInUse == false)
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
        if(Input.GetAxis("Cancel") == 0)
        {
            m_isAxisInUse = false;
        }
    }

    void Resume()
    {

    }

    void Pause()
    {

    }
}
