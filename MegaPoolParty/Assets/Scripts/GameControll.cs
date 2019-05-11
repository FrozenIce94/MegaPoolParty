using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControll : MonoBehaviour
{

    //StudentV -Input.GetAxis("StudentV")
    //StudentH - Input.GetAxis("StudentH")
    //Fire1_S - Input.GetButtonDown("Fire1_S") 
    //Fire2_S - Input.GetButtonDown("Fire2_S")

    //LehrerV - Input.GetAxis("LehrerV")
    //LehrerH - Input.GetAxis("LehrerH")
    //Fire1_L - Input.GetButtonDown("Fire1_L")
    //Fire2_L  - Input.GetButtonDown("Fire2_L")

    public float paddleSpeed = 1f;

    public GameObject leftPanel;
    private Vector3 playerPosLeft = new Vector3(-7f, 0f, 0);

    public GameObject rightPanel;
    private Vector3 playerPosRight = new Vector3(7f, 0f, 0);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        Controll1();
    }

    private void Controll1()
    {

        float zPos = leftPanel.transform.position.z + (Input.GetAxis("StudentV") * -paddleSpeed);
        playerPosLeft = new Vector3(-6.5f, 0, Mathf.Clamp(zPos, -4f, 4f));
        leftPanel.transform.position = playerPosLeft;

       
        float zPosL = rightPanel.transform.position.z + (Input.GetAxis("LehrerV") * -paddleSpeed);
        playerPosRight = new Vector3(6.5f, 0, Mathf.Clamp(zPosL, -4f, 4f));
        rightPanel.transform.position = playerPosRight;

        //LehrerV
        
        /*
        float ArrowInput = 0;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            ArrowInput = 0.1f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            ArrowInput = -0.1f;
        }
            zPos = rightPanel.transform.position.z + (ArrowInput * paddleSpeed);
        playerPosRight = new Vector3(7.5f, 0, Mathf.Clamp(zPos, -4f, 4f));
        rightPanel.transform.position = playerPosRight;
        */
    }


    // Update is called once per frame
    void Controll2()
    {
        float speed = 0.03f;
        float limittop = 4;
        float limitbottom = -4;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Debug.Log("oben");
            if (leftPanel.transform.position.z < limittop)
            {
                leftPanel.transform.position = new Vector3(leftPanel.transform.position.x, leftPanel.transform.position.y, leftPanel.transform.position.z + speed);
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Debug.Log("oben");
            if (leftPanel.transform.position.z > limitbottom)
            {
                leftPanel.transform.position = new Vector3(leftPanel.transform.position.x, leftPanel.transform.position.y, leftPanel.transform.position.z - speed);
            }
          }


        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log("oben");
            if (rightPanel.transform.position.z < limittop)
            {
                rightPanel.transform.position = new Vector3(rightPanel.transform.position.x, rightPanel.transform.position.y, rightPanel.transform.position.z + speed);
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            Debug.Log("oben");
            if (rightPanel.transform.position.z > limitbottom)
            {
                rightPanel.transform.position = new Vector3(rightPanel.transform.position.x, rightPanel.transform.position.y, rightPanel.transform.position.z - speed);
            }
        }

    }
}
