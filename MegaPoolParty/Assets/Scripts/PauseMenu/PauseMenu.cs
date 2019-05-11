using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public bool pausable = true;

    private bool m_isAxisInUse = false;
    private GameManager gm;

    public GameObject pauseMenu;
    public GameObject mainMenu;
    public GameObject endScreen;

    public Button mainMenuButton, pauseMenuButton, endButton;

    public TextMeshProUGUI endtext;

    /// <summary>
    /// Start der Szene
    /// </summary>
    private void Start()
    {
        gm = GetComponent<GameManager>();
        gm.SetPauseMenu(this);
        mainMenuButton.Select();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetAxisRaw("Cancel") == 1 || Input.GetAxisRaw("Submit") == 1 ) && m_isAxisInUse == false && pausable)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }

            m_isAxisInUse = true;
        }
        if (Input.GetAxisRaw("Cancel") == 0 && Input.GetAxisRaw("Submit") == 0)
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
        if(! endScreen.activeSelf && ! mainMenu.activeSelf && pausable)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            GameIsPaused = true;
            pauseMenuButton.Select();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void HideMainMenu()
    {
        mainMenu.SetActive(false);
    }

    /// <summary>
    /// Startet das Game
    /// </summary>
    public void PlayGame()
    {
        gm.InitializeGame();
        HideMainMenu();
        gm.StartRandomGame();
    }

    /// <summary>
    /// Startet das Game
    /// </summary>
    public void GoBackToMainMenu()
    {
        endScreen.SetActive(false);
        mainMenu.SetActive(true);
    }

    /// <summary>
    /// Prüft ob das gesamte Spiel vorbei ist
    /// </summary>
    /// <param name="winner">Schüler = true, Lehrer = false</param>
    public void ShowEndScreen(bool winner)
    {
        pauseMenu.SetActive(false);

        endButton.Select();

        if(endtext != null)
        {
            if (winner)
            {
                endtext.text = "Der Schüler hat gewonnen!";
            }
            else
            {
                endtext.text = "Der Lehrer hat gewonnen!";
            }
        } 
        endScreen.SetActive(true);
        Time.timeScale = 1;
    }
}