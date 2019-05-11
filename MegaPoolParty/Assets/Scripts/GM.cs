using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GM : MonoBehaviour {
	

	public static GM instance = null;
	public GameObject Ball;
	private GameObject cloneBall;
	private int L_Score=0;
	private int R_Score=0;
	public int SiegScore = 10;
	public Text Text_Score;
	public Text Text_Sieg;
	public Text Text_Start;

    public static bool hasBall;
	
		// Use this for initialization
	void Awake () 
	{
        if (instance == null)
        {
            instance = this;
            hasBall = false;
        }
        else if (instance != this)
            Destroy(gameObject);
		
		//Setup();
		
	}

   void Update()
    {
        bool StartInput = false;

        if (Text_Sieg.enabled==true)
        {
            return;
        }
   
        if ((Input.GetButtonDown("Fire1") || Input.GetKey(KeyCode.Space)) && hasBall == false)
        {
            Debug.Log("new Ball");
            hasBall = true;
             Setup();
        }
    }

    public void Setup()
	{
		cloneBall = Instantiate(Ball, transform.position, Quaternion.identity) as GameObject;
		Invoke ("TextStartaus", 0.3f);

	}

	private void TextStartaus()
	{
		Text_Start.enabled = false;
        bool pos = false;
        if (Random.Range(0, 100) > 50)
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
	
		Destroy (cloneBall);
	
		if (isLinks == true) {
			R_Score += 1;
		}
		else 
		{
			L_Score = L_Score+1;
		}

		Text_Score.text = L_Score + "  :  " + R_Score;


		int SpielZustand=CheckSieg ();

		switch (SpielZustand) {
		case 0:
			break;
		case 1:
			Sieg(1);
			return;
			break;
		case 2:
			Sieg(2);
			return;
			break;
		}
		//Invoke ("Setup", 3);

	}

	enum Spieler
	{
		keiner,
		links,
		rechts
	}

	private void Sieg(int wer)
	{
		if ((Spieler) wer == Spieler.links) {
			Text_Sieg.text = "Sieger ist der linke Spieler";
		}
		else if ((Spieler) wer == Spieler.rechts) {
			Text_Sieg.text = "Sieger ist der rechter Spieler";
		}
		Text_Sieg.enabled = true;
		Invoke ("Neustart", 2);
	}

	private void Neustart()
	{
		Application.LoadLevel (0);
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

