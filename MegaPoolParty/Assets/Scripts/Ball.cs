using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public float ballInitialVelocity = 600f;
	private Rigidbody rb;
	private bool ballInPlay=false;

	void Awake () {

		rb = GetComponent<Rigidbody> ();

	}


    void LateUpdate()
    {
        //if (transform.position.y>0.21)
       // {
            transform.position = new Vector3(transform.position.x, 0.21f, transform.position.z);
       // }
    }

    void FixedUpdate()
    {
        rb.AddForce(Vector3.forward);
    }
    // Update is called once per frame
    void Update()
    {
        bool StartInput = false;
       // if ((Input.GetButtonDown("Fire1") || Input.GetKey(KeyCode.Space)) && ballInPlay == false)
       // {
        //    StartInput = true;
       // }

        bool positiv = false;
        if (Random.Range(0, 100) > 50)
        {
            positiv = true;
        }
        //#if UNITY_EDITOR
        if (StartInput)
        {
            AddF(positiv);
        }
    }
		
	

        void OnDestroy()
        {
            GM.hasBall = false;
        }

    public void AddF(bool positiv)
    {
        transform.parent = null;
        ballInPlay = true;
        rb.isKinematic = false;
        if (positiv)
        {
            rb.AddForce(new Vector3(ballInitialVelocity, 0, ballInitialVelocity));
        }
        else
        {
            rb.AddForce(new Vector3(-ballInitialVelocity, 0, -ballInitialVelocity));
        }
    }
}