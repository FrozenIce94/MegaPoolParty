using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public AudioSource SoftMusic;
    public AudioSource HardMusic;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Spielt ein Lied ab
    /// </summary>
    /// <param name="hard">Ist es hart, dann hart</param>
    public void PlayMusic(bool hard)
    {
        if (hard)
        {
            if (HardMusic.isPlaying) return;
            if (SoftMusic.isPlaying)
                SoftMusic.Stop();
            HardMusic.Play();
        }
        else
        {
            if (SoftMusic.isPlaying) return;
            if (HardMusic.isPlaying)
                HardMusic.Stop();
            SoftMusic.Play();
        }
    }
}
