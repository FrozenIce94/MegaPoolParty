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

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigidBody.velocity = new Vector3(0, 0, 0);
        //Move in the XZ plane.
        if (Input.GetKeyDown(keyPressing))
        {
            rigidBody.velocity = new Vector3(Step, 0, 0);          
        }

        CheckGameEnd();
    }

    public void CheckGameEnd()
    {
        if(rigidBody.position.x >= finish)
        {
            gameManager.EndMinigame(IsStudent);
        }
    }
}
