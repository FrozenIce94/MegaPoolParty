using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    [Header("Music")]
    public AudioSource SoftMusic;
    public AudioSource HardMusic;

    [Header("ActionSounds")]
    public AudioSource Countdown;
    public AudioSource StudentLoose;
    public AudioSource TeacherLoose;

    public GameManager startGameManager;
    // Start is called before the first frame update
    void Start()
    {
        startGameManager.SetMusicManager(this);
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

    internal void ActionSound(ActionSounds sound)
    {
        switch (sound)
        {
            case ActionSounds.Countdown:
                Countdown.Play();
                break;
            case ActionSounds.StudentLoose:
                StudentLoose.Play();
                break;
            case ActionSounds.TeacherLoose:
                TeacherLoose.Play();
                break;
            default:
                break;
        }
    }

    #region enums    
    public enum ActionSounds
    {
        Countdown,
        StudentLoose,
        TeacherLoose

    }



    #endregion
}
