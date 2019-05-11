using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region "Variables"

    //Static Vars
    [Header("Common")]
    public bool gamerunning;

    [Header("CheatModus MainMenu")]
    public int CheatNextGame = 0;
    private static bool cheatexecuded = false;

    private static int playfield;
    private static Games lastGame;
    private static int currentfield, lastfield;

    private static bool gamefinished;

    public static StartCounter timer;
    #endregion
    #region "Public Methods"

    /// <summary>
    /// Initialisiert das Basisspiel
    /// </summary>
    public void InitializeGame()
    {
        playfield = 7;
        currentfield = 4;
        lastGame = Games.None;
        gamerunning = false;
        gamefinished = false;

        Debug.Log("GameManager: ----- Start Init -----");
        Debug.Log("GameManager: Playfields: " + playfield);
        Debug.Log("GameManager: Startfeld: " + currentfield);
        Debug.Log("GameManager: Game Running: " + gamerunning);
        Debug.Log("GameManager: ----- End Init -----");
    }

    /// <summary>
    /// Startet ein neues Spiel sofern keins aktiv ist
    /// </summary>
    public void StartRandomGame()
    {
        if (gamerunning) return;

        if (CheatNextGame != 0 && !cheatexecuded)
        {
            lastGame = (Games)CheatNextGame;
            StartGame(lastGame);
            cheatexecuded = true;
        }
        else
        {

            int gameCounts = Enum.GetNames(typeof(Games)).Length;
            System.Random random = new System.Random();

            int nextGame = 0;
            while (nextGame == 0 || nextGame == (int)lastGame)
            {
                int randomNumber = random.Next(1, gameCounts);
                nextGame = randomNumber;
            }

            lastGame = (Games)nextGame;
            StartGame(lastGame);
        }
    }

    /// <summary>
    /// Startet ein neues Spiel sofern keins aktiv ist
    /// </summary>
    /// <param name="winner">Schüler = true, Lehrer = false</param>
    public void EndMinigame(bool? winner)
    {
        if (winner.HasValue)
        {
            lastfield = currentfield;

            if (winner.Value)
            {
                currentfield += 1;
            }
            else
            {
                currentfield -= 1;
            }
        }

        DebugCurrentData();

        CheckGameEnd();
    }

    /// <summary>
    /// Schließt den Hub und startet das nächste Spiel
    /// </summary>
    /// <param name="winner">Schüler = true, Lehrer = false</param>
    public void EndHub()
    {
        Debug.Log("GameManager: Hub beendet");
        SceneManager.UnloadSceneAsync(5);
        StartRandomGame();
    }

    /// <summary>
    /// Initialisiert das Basisspiel neu mit den Grundlagen
    /// </summary>
    public void ResetGame()
    {
        Debug.Log("GameManager: Reset Game!");
        InitializeGame();
    }

    /// <summary>
    /// Schreibt alle aktuellen Werte in die Debug Log
    /// </summary>
    public void DebugCurrentData()
    {
        Debug.Log("GameManager: ----- Start DebugPrint -----");
        Debug.Log("GameManager: Aktuelles Feld: " + currentfield);
        Debug.Log("GameManager: Game Running: " + gamerunning);
        Debug.Log("GameManager: ----- End DebugPrint -----");
    }

    /// <summary>
    /// Setzt das Timerobjekt global
    /// </summary>
    public bool SetTimerObject(StartCounter timer)
    {
        if (timer == null)
            return false;
        GameManager.timer = timer;
        return true;
        
    }

    public bool PauseTimer()
    {
        if (timer == null)
            return false;
        timer.PauseTimer();
        return true;
    }

    public bool ResumeTimer()
    {
        if (timer == null)
            return false;
        timer.ResumeTimer();
        return true;
    }

    public bool StartTimer(StartCounter.OnTimerFinished callback, GameManager.Games game)
    {
        if (timer == null)
            return false;
        timer.StartGame(callback, game);
        return true;
    }

    public bool StopTimer()
    {
        if (timer == null)
            return false;
        timer.StopGame();
        return true;
    }

    #endregion
    #region "Private Methods"

    /// <summary>
    /// Startet ein Spiel
    /// </summary>
    private void StartGame(Games game)
    {

        switch (game)
        {
            case Games.None:
                StartRandomGame();
                break;
            case Games.Bomberman:
                SceneManager.LoadScene(1, LoadSceneMode.Additive);
                break; 
            case Games.Swimming:
                SceneManager.LoadScene(2, LoadSceneMode.Additive);
                break;
            case Games.Pong:
                SceneManager.LoadScene(3, LoadSceneMode.Additive);
                break;
            case Games.Quiz:
                SceneManager.LoadScene(4, LoadSceneMode.Additive);
                break;
        }

        Debug.Log("GameManager: Spiel " + game.ToString() + " gestartet");
    }

    /// <summary>
    /// Prüft ob das gesamte Spiel vorbei ist
    /// </summary>
    private void CheckGameEnd()
    {
        if(currentfield == 0)
        {
            Debug.Log("GameManager: Lehrer gewonnen");
            SceneManager.UnloadSceneAsync((int)lastGame);
            ShowEndScreen(false);
            return;
        }

        if(currentfield == 7)
        {
            Debug.Log("GameManager: Schüler gewonnen");
            SceneManager.UnloadSceneAsync((int)lastGame);
            ShowEndScreen(true);
            return;
        }

        CloseSceneAndShowHub();
    }

    /// <summary>
    /// Prüft ob das gesamte Spiel vorbei ist
    /// </summary>
    /// <param name="winner">Schüler = true, Lehrer = false</param>
    private void ShowEndScreen(bool winner)
    {
        if (gamefinished) return;
        gamefinished = true;
        GameObject tempObject = GameObject.Find("UICanvas");
        tempObject.SendMessage("ShowEndScreen", winner, SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    /// zeigt den Hub an
    /// </summary>
    private void CloseSceneAndShowHub()
    {
        SceneManager.UnloadSceneAsync((int)lastGame);
        StartRandomGame();
        //SceneManager.LoadScene(5, LoadSceneMode.Additive);
        //GameObject tempObject = GameObject.Find("CanvasHub");
        //tempObject.SendMessage("InitHub", new KeyValuePair<int, int>(lastfield, currentfield), SendMessageOptions.DontRequireReceiver);
    }

    #endregion
    #region "Enum"

    public enum Games
    {
        None = 0,
        Bomberman = 1,
        Swimming = 2,
        Pong = 3,
        Quiz = 4
    }

    #endregion

}
