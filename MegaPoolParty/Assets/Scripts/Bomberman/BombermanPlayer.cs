using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombermanPlayer : MonoBehaviour
{

    [Header("Common")]
    public bool IsStudent = false;
    public GameManager gameManager;


    [Header("Visuals")]
    public GameObject model;
    public float turnSpeed = 5f;

    [Header("Movement")]
    public float movingVelocity = 5f;
    public float jumpingVelocity = 5f;

    [Header("Equipment")]
    public int health = 5;
    public GameObject bombPrefab;
    public int bombAmount = 2;


    [Header("KeyBinding")]
    public KeyCode keyUp;
    public KeyCode keyDown;
    public KeyCode keyLeft;
    public KeyCode keyRight;
    public KeyCode placeBomb;

    // Private
    private float knockbackTimer;
    private Rigidbody rigidBody;
    private Quaternion targetModelRotation;
    private int maxBombAmount;

    



    // Start is called before the first frame update
    void Start()
    {
       maxBombAmount = bombAmount;
        rigidBody = GetComponent<Rigidbody>();
        targetModelRotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {

        model.transform.rotation = Quaternion.Lerp(model.transform.rotation, targetModelRotation, Time.deltaTime * turnSpeed);
        if(knockbackTimer > 0)
        {
            knockbackTimer -= Time.deltaTime;
        }
        else
        {
            ProcessInput();
        }

    }

    void ProcessInput()
    {
        
        //Stop
        rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, 0);

        //Move in the XZ plane.
        if (Input.GetKey(keyRight))
        {   rigidBody.velocity = new Vector3(movingVelocity, rigidBody.velocity.y, rigidBody.velocity.z);
            targetModelRotation = Quaternion.Euler(0, 90, 0);
        }


        if (Input.GetKey(keyLeft))
        { rigidBody.velocity = new Vector3(-movingVelocity, rigidBody.velocity.y, rigidBody.velocity.z);
            targetModelRotation = Quaternion.Euler(0, 270, 0);
        }

        if (Input.GetKey(keyUp))
        { rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, movingVelocity);
            targetModelRotation = Quaternion.Euler(0, 0, 0);
        }

        if (Input.GetKey(keyDown))
        { rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, -movingVelocity);
            targetModelRotation = Quaternion.Euler(0, 180, 0);
        }

        if (Input.GetKeyDown(placeBomb))
        {
            DoPlaceBomb();
        }
    }

    public void Hit()
    {
        gameManager.EndMinigame(!IsStudent);
    }

    private void DoPlaceBomb()
    {
        if (bombAmount <= 0) return;
        bombAmount -= 1;
        var bombObj = Instantiate(bombPrefab, transform.position, Quaternion.Euler(Vector3.zero));
        var bomb = bombObj.GetComponent<Bombe>();
        bomb.playerInstance = this;

    }
}
