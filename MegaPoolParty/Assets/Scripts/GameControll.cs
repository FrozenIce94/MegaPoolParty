using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControll : MonoBehaviour
{
   public GameObject leftPanel;
    public GameObject rightPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
