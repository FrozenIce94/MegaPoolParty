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
    public int currentfield;

    [Header("CheatModus MainMenu")]
    public int CheatNextGame = 0;
    private static bool cheatexecuded = false;

    private static int playfield;
    private static Games lastGame;

    public static PauseMenu pm;
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

            int gameCounts = Enum.GetNames(typeof(Games)).Length; ;
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
    public void EndMinigame(bool winner)
    {

        if (winner)
        {
            currentfield += 1;
        } else
        {
            currentfield -= 1;
        }

        DebugCurrentData();

        CheckGameEnd();
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
            ShowEndScreen(false);
        }

        if(currentfield == 7)
        {
            Debug.Log("GameManager: Schüler gewonnen");
            ShowEndScreen(true);
        }

        CloseSceneAndShowHub();
    }

    /// <summary>
    /// Prüft ob das gesamte Spiel vorbei ist
    /// </summary>
    /// <param name="winner">Schüler = true, Lehrer = false</param>
    private void ShowEndScreen(bool winner)
    {

        //Scene menu = SceneManager.GetSceneAt(0);
        //GameObject[] objects = menu.GetRootGameObjects();
        //Canvas cv;
        //for (int i = 0; i < objects.Length - 1; i++)
        //{
        //    //if(objects[i].name == "UICanvas")
        //    //{
        //    //    (objects[i].SendMessage(""));
        //    //}
        //}

        //pm.ShowEndScreen(winner);
    }

    /// <summary>
    /// zeigt den Hub an
    /// </summary>
    private void CloseSceneAndShowHub()
    {
        SceneManager.UnloadSceneAsync((int)lastGame);
        StartRandomGame();

        
        //Hier Hub aufrufen mit Parameter für Änderung? oder ruft der Hub das auf
    }

    #endregion
    #region "Enum"

    public enum Games
    {
        None = 0,
        Bomberman = 1,
        Swimming = 2
    }

    #endregion

}
