using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wippen : MonoBehaviour
{
    public bool links = false;
    public float time =2;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DirChange", 2);
    }

    // Update is called once per frame
    void Update()
    {
 
        if (links)
        {
            transform.Rotate(Vector3.right * Time.deltaTime * 2);
         }
        else
        {
            transform.Rotate(Vector3.left * Time.deltaTime * 2);
        }
    }

    void DirChange()
    {
        links = !links;
        Invoke("DirChange", 2);
    }
}
