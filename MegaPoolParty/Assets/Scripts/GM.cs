using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GM : MonoBehaviour
{


    public static GM instance = null;
    public GameObject Ball;
    private GameObject cloneBall;
    private int L_Score = 0;
    private int R_Score = 0;
    public int SiegScore = 10;
    public Text Text_Score;
    public Text Text_Sieg;
    public Text Text_Start;
    public Text Text_Count;

    private GameManager gameM;
    private System.Random rnd = new System.Random();
    public static bool hasBall;

    public bool firstBall;

    public float Countdowntime;


    public bool SchuelerWinner;

    public Animation LeherAnim;
    public Animation SchuelerAnim;

    bool countDownTimerFinished = false;
    bool firstBallSpawn = true;
    bool timerRegistered = false;
    public void timerEndGameStart()
    {
        if (L_Score > R_Score)
        {
            gameM.EndMinigame(true);
        }
        else if (L_Score < R_Score)
        {
            gameM.EndMinigame(false);
        }
        else
        {
            gameM.EndMinigame(null);
        }
    }

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            hasBall = false;
            firstBall = true;
        }
        else if (instance != this)
            Destroy(gameObject);

        //Setup();
        gameM = GetComponent<GameManager>();

    }

    void Update()
    {
        if (!timerRegistered)
        {
            timerRegistered = GetComponent<GameManager>().StartTimer(timerEndGameStart, GameManager.Games.Pong);
        }
        //if (!timerRegistered)
        //    return;


        if (Text_Sieg.enabled == true)
        {
            return;
        }
        if (!GameManager.CanCaptureInput) return;
        if ((Input.GetButtonDown("Fire1_S") 
            || Input.GetButtonDown("Fire1_L") 
            || Input.GetKey(KeyCode.Space) 
            || firstBallSpawn)
            && hasBall == false)
        {
            firstBallSpawn = false;
            if (!firstBall) { return; }
            firstBall = false;
            Debug.Log("new Ball");
            hasBall = true;
            Setup();
        }
    }

    public void Setup()
    {
        Text_Count.text = "3";
        Text_Count.enabled = false;
        cloneBall = Instantiate(Ball, transform.position, Quaternion.identity) as GameObject;
        Invoke("TextStartaus", 0.3f);

    }

    public void startSetup()
    {
        Debug.Log(Countdowntime + ":" + Time.timeSinceLevelLoad);

        Text_Count.text = "1";

        Invoke("Setup", 0.8f);
    }
    private void TextStartaus()
    {
        Text_Start.enabled = false;
        bool pos = false;
        if (rnd.Next(0, 100) > 50)
        {
            pos = true;
        }
        if (cloneBall != null)
        {
            cloneBall.GetComponent<Ball>().AddF(pos);
        }
    }

    public void Score(bool isLinks)
    {
        GameManager.musicManager.ActionSound(MusicManager.ActionSounds.RightAnswer);

        Destroy(cloneBall);

        if (isLinks == true)
        {
            R_Score += 1;
        }
        else
        {
            L_Score = L_Score + 1;
        }

        Text_Score.text = L_Score + "  :  " + R_Score;


        int SpielZustand = CheckSieg();

        switch (SpielZustand)
        {
            case 0:
                break;
            case 1:
                Sieg(1);
                LeherAnim.Play();                
                return;
            case 2:
                SchuelerAnim.Play();
                Sieg(2);
                return;
        }

        Countdowntime = Time.timeSinceLevelLoad;
        Text_Count.enabled = true;
        Text_Count.text = "2";
        Invoke("startSetupZwei", 0.8f);
    }

    public void startSetupZwei()
    {
      //  Debug.Log(Countdowntime + ":" + Time.timeSinceLevelLoad);

        Text_Count.text = "1";

        Invoke("startSetup", 0.8f);
    }

    enum Spieler
    {
        keiner,
        links,
        rechts
    }

    private void Sieg(int wer)
    {
        if ((Spieler)wer == Spieler.links)
        {
            Text_Sieg.text = "Sieger ist der Schüler!";
            SchuelerWinner = true;
        }
        else if ((Spieler)wer == Spieler.rechts)
        {
            Text_Sieg.text = "Sieger ist der Lehrer!";
            SchuelerWinner = false;
        }
        Text_Sieg.enabled = true;
        Invoke("Neustart", 4);
    }

    private void Neustart()
    {
        // Application.LoadLevel (0);
        gameM.StopTimer();
        gameM.EndMinigame(SchuelerWinner);

    }

    private int CheckSieg()
    {

        // 0= kein Sieger, 1= Sieger Links; 2= Sieger Rechts


        if (L_Score >= SiegScore)

        {
            return 1;

        }


        if (R_Score >= SiegScore)

        {
            return 2;

        }

        if (countDownTimerFinished)
        {
            if(L_Score > R_Score)
            {
                gameM.EndMinigame(true);
            } else if(L_Score < R_Score)
            {
                gameM.EndMinigame(false);
            } else
            {
                gameM.EndMinigame(null);
            }
        }


        return 0;

    }


    /*

	void CheckGameOver()
	{
		if (bricks < 1)
		{
			youWon.SetActive(true);
			Time.timeScale = .25f;
			Invoke ("Reset_Win", resetDelay);
		}
		
		if (lives < 1)
		{
			gameOver.SetActive(true);
			Time.timeScale = .25f;
			Invoke ("Reset_Loose", resetDelay);
		}
		
	}
	
	void Reset_Loose()
	{
		Time.timeScale = 1f;
		Application.LoadLevel(0);
	}

	void Reset_Win()
	{
		Time.timeScale = 1f;
		if (Application.loadedLevel < Application.levelCount){
			Application.LoadLevel(Application.loadedLevel+1);
		} else {
			Application.LoadLevel (0);

		}
	}

	public void LoseLife()
	{
		lives--;
		livesText.text = "Leben: " + lives;
		Instantiate(deathParticles, clonePaddle.transform.position, Quaternion.identity);
		Destroy(clonePaddle);
		Invoke ("SetupPaddle", resetDelay);
		CheckGameOver();
	}
	
	void SetupPaddle()
	{
		clonePaddle = Instantiate(paddle, transform.position, Quaternion.identity) as GameObject;
	}
	
	public void DestroyBrick()
	{
		bricks--;
		CheckGameOver();
	}
*/
}

