using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    private GameManager gm;
    private Canvas cv;

    /// <summary>
    /// Start der Szene
    /// </summary>
    private void Start()
    {
        gm = GetComponent<GameManager>();
        cv = GetComponent<Canvas>();
    }

    /// <summary>
    /// Startet das Game
    /// </summary>
    public void PlayGame()
    {
        gm.InitializeGame();
        gm.ShowHub();
    }

    /// <summary>
    /// Beendet das Spiel
    /// </summary>
    public void EndGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Öffnet die Optionen
    /// </summary>
    public void Options()
    {
        
    }
}
