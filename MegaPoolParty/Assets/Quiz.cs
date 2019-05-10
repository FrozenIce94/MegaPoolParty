using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Quiz : MonoBehaviour
{
    public GameObject questionObj;
    public GameObject leftAnswerObj;
    public GameObject rightAnswerObj;
    public GameObject topAnswerObj;
    public GameObject bottomAnswerObj;

    public GameObject answerAObj;
    public GameObject answerBObj;

    public float secondsPerAnswer;
    public float answerRegionLength;

    string question = "";
    string[] answers;

    int pupilScore = 0;
    int teacherScore = 0;

    bool newQuestion = true;
    float questionStartTime;
    // Start is called before the first frame update
    void Start()
    {
        TextMeshPro questionTextComp = questionObj.GetComponent<TextMeshPro>();
        questionTextComp.text = question;

        answers = new string[4];
        answers[0] = "42";
        answers[1] = "lunch";
        answers[2] = "games";
        answers[3] = "3.141";

        //leftAnswerObj.GetComponent<TextMeshPro>().text = answers[0];
        //rightAnswerObj.GetComponent<TextMeshPro>().text = answers[1];
        //topAnswerObj.GetComponent<TextMeshPro>().text = answers[2];
        //bottomAnswerObj.GetComponent<TextMeshPro>().text = answers[3];
    }

    // Update is called once per frame
    void Update()
    {
        float currentTime = Time.time;

        if (newQuestion)
        {
            questionStartTime = currentTime;
            newQuestion = false;
        }

        int answerIndex = (int)((currentTime - questionStartTime) / secondsPerAnswer);

        answerAObj.GetComponent<TextMeshPro>().text = answers[answerIndex % 4];

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            newQuestion = true;
        }
    }
}
