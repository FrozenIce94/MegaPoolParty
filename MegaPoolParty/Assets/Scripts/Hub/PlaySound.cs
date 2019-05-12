using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{

    public AudioSource teleport, wooooosh, fall;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayTeleportSound()
    {
        teleport.Play();
    }

    public void PlayFallSound()
    {
        fall.Play();
    }

    public void PlayWooooshSound()
    {
        wooooosh.Play();
    }
}
