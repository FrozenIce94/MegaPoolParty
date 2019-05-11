using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swimmer : MonoBehaviour
{

    private bool hold = false;
    private int finish = 100;

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
        if (Input.GetKey(keyPressing) && !hold)
        {
            hold = true;
            rigidBody.velocity = new Vector3(5, 0, 0);          
            Debug.Log("Swimmer (IsStudent = " + IsStudent + "): Move X");
        } else {
            hold = false;
            Debug.Log("Swimmer (IsStudent = " + IsStudent + "): Hold = False");
        }

        CheckGameEnd();
    }

    public void CheckGameEnd()
    {
        if(rigidBody.velocity.x >= finish)
        {
            Time.timeScale = 0;
            gameManager.EndMinigame(IsStudent);
        }
    }
}
