﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartCounter : MonoBehaviour
{
    public delegate void OnTimerFinished();
    private OnTimerFinished timerFinishedCallback;
    private bool timerActive = false;
    private float timeLeft;
    private readonly float initialTime = 60.0f;

    private float countdownTime;
    private bool countdownActive = false;

    private bool showInstructions = false;

    private bool m_isAxisInUse = false;
    [Header("Timer Object")]
    public GameObject timer;
    public GameObject hand;
    [Header("Instructions Object")]
    public GameObject instructions;
    public GameObject instDescription;
    public GameObject instPicture;
    [Header("Countdown Object")]
    public TextMeshProUGUI countdown;
    public GameObject countdownContainer;
    [Header("GameManager")]
    public GameManager gameManager;

    private void Start()
    {
        //StartGame(() => { Debug.Log("game finished"); }, game: GameManager.Games.Pong);
        gameManager.SetTimerObject(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (showInstructions)
        {
            if (Input.GetAxisRaw("Jump") == 1 && m_isAxisInUse == false)
            {
                showInstructions = false;
                HideInstructions();
                StartCountdown();

                m_isAxisInUse = true;
            }
        }

        if (timerActive)
        {
            timeLeft = timeLeft - Time.deltaTime;
            hand.transform.SetPositionAndRotation(hand.transform.position, Quaternion.Euler(new Vector3(0,0, (timeLeft / initialTime) * 360)));
            if (timeLeft < 0.0f)
            {
                TimerDone();
            }
        }

        if (countdownActive)
        {
            if (countdownTime + 2.8 < Time.realtimeSinceStartup)
            {
                StopCountdown();
                StartTimer();
            }
            else if (countdownTime + 2.1 < Time.realtimeSinceStartup)
            {
                countdown.text = "los!";
            } else if (countdownTime + 1.4 < Time.realtimeSinceStartup)
            {
                countdown.text = "1";
            } else if(countdownTime + 0.7 < Time.realtimeSinceStartup)
            {
                countdown.text = "2";
            }
        }
    }

    public void StartGame(OnTimerFinished callback, GameManager.Games game)
    {
        timerFinishedCallback = callback;

        timeLeft = initialTime;

        Time.timeScale = 0.0f;

        //show game instructions
        ShowInstructions(game);
        
        //
    }

    public void StopGame()
    {
        timerFinishedCallback = null;
        timerActive = false;
    }

    void StartCountdown()
    {
        countdown.text = "3";
        countdownContainer.SetActive(true);
        countdownActive = true;
        countdownTime = Time.realtimeSinceStartup;
    }

    void StopCountdown()
    {
        countdownContainer.SetActive(false);
        countdownActive = false;
        Time.timeScale = 1.0f;
        StartTimer();
    }

    public void PauseTimer() => timerActive = false;

    public void ResumeTimer() => timerActive = true;

    void StartTimer()
    {
        timerActive = true;
        hand.transform.SetPositionAndRotation(hand.transform.position, Quaternion.identity);
        timer.SetActive(true);
    }

    void HideInstructions()
    {
        instructions.SetActive(false);
    }

    void ShowInstructions(GameManager.Games game)
    {
        instructions.SetActive(true);
        TextMeshProUGUI descText = instDescription.GetComponent<TextMeshProUGUI>();
        descText.text = "test Beschreibung vom code aus fuer game: " + game.ToString();
        showInstructions = true;

    }

    void TimerDone()
    {
        timerActive = false;
        timer.SetActive(false);
        timerFinishedCallback();
    }
}
