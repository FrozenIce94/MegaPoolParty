using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static MusicManager;

public class StartCounter : MonoBehaviour
{
    public delegate void OnTimerFinished();
    private OnTimerFinished timerFinishedCallback;
    private bool timerActive = false;
    private float timeLeft;
    public readonly float initialTime = 30.0f;

    private string[] descriptionList = new string[] {"None, you should not be here!", "", "", "", "" };

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
            if ((Input.GetAxisRaw("Jump") == 1 || Input.GetAxisRaw("Submit") == 1) && m_isAxisInUse == false)
            {
                HideInstructions();
                StartCountdown();

                m_isAxisInUse = true;
            }
        }

        if (Input.GetAxisRaw("Jump") == 0 && Input.GetAxisRaw("Submit") == 0)
        {
            m_isAxisInUse = false;
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

        countdownActive = false;
        timerActive = false;
        showInstructions = false;

        Time.timeScale = 0.0f;

        //show game instructions
        ShowInstructions(game);
        
        //
    }

    public void StopGame()
    {
        timerFinishedCallback = null;
        timerActive = false;
        timer.SetActive(false);
    }

    void StartCountdown()
    {
        gameManager.PlayActionSound(ActionSounds.Countdown);
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
        GameManager.pauseMenu.pausable = true;
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
        showInstructions = false;
        instructions.SetActive(false);
    }

    void ShowInstructions(GameManager.Games game)
    {
        instructions.SetActive(true);
        TextMeshProUGUI descText = instDescription.GetComponent<TextMeshProUGUI>();
        descText.text = "test Beschreibung vom code aus fuer game: " + game.ToString();
        showInstructions = true;
        GameManager.pauseMenu.pausable = false;
    }

    void TimerDone()
    {
        timerActive = false;
        timer.SetActive(false);
        timerFinishedCallback();
    }
}
