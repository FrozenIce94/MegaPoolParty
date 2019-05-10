﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region "Variables"

    //Static Vars
    public static bool gamerunning;
    public static int currentfield;

    private static int playfield;
    private static Games lastGame;

    #endregion
    #region "Public Methods"

    /// <summary>
    /// Initialisiert das Basisspiel
    /// </summary>
    public void InitializeGame()
    {
        playfield = 7;
        currentfield = 3;
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

        int gameCounts = 3;
        System.Random random = new System.Random();

        int nextGame = 0;
        while (nextGame == 0 || nextGame == (int)lastGame)
        {
            int randomNumber = random.Next(1, gameCounts);
        }

        lastGame = (Games)nextGame;
        StartGame(lastGame);
    }

    /// <summary>
    /// Startet ein neues Spiel sofern keins aktiv ist
    /// </summary>
    /// <param name="winner">Schüler = true, Lehrer = false</param>
    public void MinigameEnd(bool winner)
    {
        if (winner)
        {
            currentfield += 1;
        } else
        {
            currentfield -= 1;
        }

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
            case Games.Pong:
                //Hier Szene Starten
                break;
            case Games.Hockey:
                //Hier Szene Starten
                break;
            case Games.Quiz:
                //Hier Szene Starten
                break;
        }

        Debug.Log("GameManager: Spiel" + game.ToString() + "gestartet");
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
    }

    /// <summary>
    /// Prüft ob das gesamte Spiel vorbei ist
    /// </summary>
    /// <param name="winner">Schüler = true, Lehrer = false</param>
    private void ShowEndScreen(bool winner)
    {
        //Hier End Screen aufrufen
        //Parameter der Gewinner
    }

    /// <summary>
    /// zeigt den Hub an
    /// </summary>
    private void ShowHub()
    {
        //Hier Hub aufrufen mit Parameter für Änderung? oder ruft der Hub das auf
    }

    #endregion
    #region "Enum"

    private enum Games
    {
        None = 0,
        Pong = 1,
        Hockey = 2,
        Quiz = 3
    }

    #endregion

}
