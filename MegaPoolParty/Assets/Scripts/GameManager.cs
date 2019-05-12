﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static MusicManager;

public class GameManager : MonoBehaviour
{
    #region "Variables"

    //Static Vars
    [Header("Common")]
    public static bool gamerunning;
    public static PauseMenu pauseMenu;

    [Header("CheatModus MainMenu")]
    public int CheatNextGame = 0;
    private static bool cheatexecuded = false;

    private static int playfield;
    private static Games lastGame;
    private static int currentfield, lastfield;

    private static bool gamefinished;

    public static StartCounter timer;

    public static MusicManager musicManager;

    /// <summary>
    /// Nur, wenn die Zeit tickt, darf input gecaptured werden
    /// </summary>
    public static bool CanCaptureInput { get { return (timer != null) 
                && (timer.isActiveAndEnabled)
                && (timer.timerActive); } }
    #endregion
    #region "Start"
    private void Start()
    {

        if(this.gameObject.scene.buildIndex == 0)
        {
            SceneManager.LoadScene(6, LoadSceneMode.Additive);
        }
    }

    public void SetMusicManager(MusicManager manager)
    {
        GameManager.musicManager = manager;
    }


    #endregion
    #region "Public Methods"


    /// <summary>
    /// Initialisiert das Basisspiel
    /// </summary>
    public void InitializeGame()
    {
        playfield = 7;
        currentfield = 4;
        lastfield = 4;
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
                musicManager.ActionSound(ActionSounds.TeacherLoose);
                currentfield += 1;
            }
            else
            {
                musicManager.ActionSound(ActionSounds.StudentLoose);
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

    /// <summary>
    /// pausiere timer
    /// </summary>
    public bool PauseTimer()
    {
        if (timer == null)
            return false;
        timer.PauseTimer();
        return true;
    }

    /// <summary>
    /// Setzt den timer fort
    /// </summary>
    public bool ResumeTimer()
    {
        if (timer == null)
            return false;
        timer.ResumeTimer();
        return true;
    }

    /// <summary>
    /// Startet den timer mit callback
    /// </summary>
    public bool StartTimer(StartCounter.OnTimerFinished callback, GameManager.Games game)
    {
        if (timer == null)
            return false;
        timer.StartGame(callback, game);
        return true;
    }

    /// <summary>
    /// Stoppt den timer vorzeitig
    /// </summary>
    public bool StopTimer()
    {
        if (timer == null)
            return false;
        timer.StopGame();
        return true;
    }

    public void SetPauseMenu(PauseMenu pauseMenu)
    {
        GameManager.pauseMenu = pauseMenu;
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
                musicManager?.PlayMusic(false);
                StartRandomGame();
                break;
            case Games.Bomberman:
                musicManager?.PlayMusic(true);
                SceneManager.LoadScene(1, LoadSceneMode.Additive);
                break; 
            case Games.Swimming:
                musicManager?.PlayMusic(true);
                SceneManager.LoadScene(2, LoadSceneMode.Additive);
                break;
            case Games.Pong:
                musicManager?.PlayMusic(false);
                SceneManager.LoadScene(3, LoadSceneMode.Additive);
                break;
            case Games.Quiz:
                musicManager?.PlayMusic(false);
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
        musicManager.PlayMusic(false);
        SceneManager.UnloadSceneAsync((int)lastGame);
        SceneManager.LoadScene(5, LoadSceneMode.Additive);
    }

    public KeyValuePair<int, int> GetPositions()
    {
        return new KeyValuePair<int, int>(lastfield, currentfield);
    }

    public void PlayActionSound(ActionSounds sound)
    {
        musicManager?.ActionSound(sound);


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
