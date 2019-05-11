using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BombermanPlayer : MonoBehaviour
{

    [Header("Common")]
    public bool IsStudent = false;
    public GameManager gameManager;
    public BombermanController controllerInstance;
    public BombermanPlayer otherPlayer;
    public List<Collider> currentBombColliders = new List<Collider>();

    [Header("Testing")]
    public bool stuckInCollisionMode = true;


    [Header("Visuals")]
    public GameObject model;
    public float turnSpeed = 5f;
    public TextMeshProUGUI bombLabel;

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
    private Collider playerCollider;



    private void RefreshBombText()
    {
        bombLabel.text  = $"Bomben: {bombAmount} / {maxBombAmount}";
    }
    // Start is called before the first frame update
    void Start()
    {
        maxBombAmount = bombAmount;
        RefreshBombText();
        playerCollider = GetComponent<Collider>();
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

    internal void IncreaseBombs()
    {
        bombAmount += 1;
        RefreshBombText();
    }

    void ProcessInput()
    {
        
        //Stop
        rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, 0);

        //Check if Bomb area is left
        foreach (var collider in currentBombColliders.ToList())
        {
            //if so => enable collision
            if(!collider.bounds.Intersects(playerCollider.bounds))
            {
                Physics.IgnoreCollision(collider, playerCollider, false);
                currentBombColliders.Remove(collider);
            }
        }



        //Move in the XZ plane.
        if (Input.GetKey(keyRight))
        {   rigidBody.velocity = new Vector3(movingVelocity, rigidBody.velocity.y, rigidBody.velocity.z);
            targetModelRotation = Quaternion.Euler(0, 180, 0);
        }


        if (Input.GetKey(keyLeft))
        { rigidBody.velocity = new Vector3(-movingVelocity, rigidBody.velocity.y, rigidBody.velocity.z);
            targetModelRotation = Quaternion.Euler(0, 0, 0);
        }

        if (Input.GetKey(keyUp))
        { rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, movingVelocity);
            targetModelRotation = Quaternion.Euler(0, 90, 0);
            
        }

        if (Input.GetKey(keyDown))
        { rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, -movingVelocity);
            targetModelRotation = Quaternion.Euler(0, 270, 0);
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
        //No bombs, no placing
        if (bombAmount <= 0) return;
        bombAmount -= 1;
        RefreshBombText();

        //Create Bomb and disable collider until collision is left
        var placeVector =  controllerInstance.GetNearestPositionInGrid(transform.position);
        var bombObj = Instantiate(bombPrefab, 
                                  new Vector3(placeVector.x, 
                                              bombPrefab.transform.localScale.y * 10,
                                              placeVector.z),
                                  Quaternion.Euler(Vector3.zero), transform);
        var bombCollider = bombObj.GetComponent<Collider>();
        currentBombColliders.Add(bombCollider);
        Physics.IgnoreCollision(bombCollider, playerCollider);

        if(!stuckInCollisionMode && otherPlayer != null)
        { 
            //if other player also is in the bounds, ignore him too and add to List
            var otherPlayerCollider = otherPlayer.GetComponent<Collider>();
            if(bombCollider.bounds.Intersects(otherPlayerCollider.bounds))
            {
                Physics.IgnoreCollision(bombCollider, otherPlayerCollider);
                otherPlayer.currentBombColliders.Add(bombCollider);
            }
        }

        var bomb = bombObj.GetComponent<Bombe>();
        bomb.playerInstance = this;
    }
}
