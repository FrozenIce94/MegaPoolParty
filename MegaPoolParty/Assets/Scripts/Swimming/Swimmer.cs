using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swimmer : MonoBehaviour
{

    private int finish = 85;

    [Header("CheatModus")]
    public int Step = 5;

    [Header("Common")]
    public bool IsStudent;
    public GameManager gameManager;

    [Header("KeyBinding")]
    public KeyCode keyPressing;

    private Rigidbody rigidBody;
    private bool left = false;


    bool endRequested = false;
    bool timerRegistered = false;
    public void requestEnd()
    {
        endRequested = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!timerRegistered)
        {
            timerRegistered = GetComponent<GameManager>().StartTimer(requestEnd, GameManager.Games.Swimming);
        }
        if (!timerRegistered)
            return;
        rigidBody.velocity = new Vector3(0, 0, 0);

        if (!GameManager.CanCaptureInput && !endRequested) return;
        var prefix = (IsStudent ? "Student" : "Lehrer");
        var horizontalVelocity = Input.GetAxisRaw($"{prefix}H");

        //Move in the XZ plane.
        if ((horizontalVelocity > 0 && left)
            || (horizontalVelocity < 0 && !left))
        {
            rigidBody.velocity = new Vector3(Step, 0, 0);
            left = !left;
        }


        CheckGameEnd();
    }

    bool hasEnded = false;
    public void CheckGameEnd()
    {
        if(rigidBody.position.x >= finish && !hasEnded)
        {
            hasEnded = true;
            gameManager.EndMinigame(IsStudent);
            GetComponent<GameManager>().StopTimer();
        }else if (endRequested && !hasEnded)
        {
            hasEnded = true;
            GetComponent<GameManager>().StopTimer();
            gameManager.EndMinigame(null);
        }
    }
}
