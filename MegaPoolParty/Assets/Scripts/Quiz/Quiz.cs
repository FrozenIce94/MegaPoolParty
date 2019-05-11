using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

[Serializable]
public class Question
{
    public string questionText;
    public string rightAnswer;
    public string[] wrongAnswers;
}

[Serializable]
public class QuestionDB
{
    public Question[] questions;
}

public class Quiz : MonoBehaviour
{
    public GameObject questionObj;
    public GameObject studentScoreObj;
    public GameObject teacherScoreObj;

    public GameObject answerAObj;
    public GameObject answerBObj;

    public float secondsPerAnswer;
   
    public TextAsset txtDb;

    int studentScore = 0;
    int teacherScore = 0;

    QuestionDB db;
    Question currentQuestion;

    bool newQuestion = true;
    float questionStartTime;
    int questionIndex = 0;

    bool newAnswer = true;
    float answerStartTime;
    int answerIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        db = JsonUtility.FromJson<QuestionDB>(txtDb.text);
    }

    // Update is called once per frame
    void Update()
    {
        float currentTime = Time.time;

        if (newQuestion)
        {
            int newQuestionIndex = (int)Math.Round((db.questions.Length - 1) * UnityEngine.Random.value);
            while(newQuestionIndex == questionIndex)
            {
                newQuestionIndex = (int)Math.Round((db.questions.Length - 1) * UnityEngine.Random.value);
            }
            questionIndex = newQuestionIndex;

            currentQuestion = db.questions[questionIndex];

            TextMeshPro questionTextComp = questionObj.GetComponent<TextMeshPro>();
            questionTextComp.text = currentQuestion.questionText;

            questionStartTime = currentTime;
            newQuestion = false;
        }
        
        if (newAnswer)
        {
            answerStartTime = currentTime;
            int newAnswerIndex = (int)Math.Round(UnityEngine.Random.value * (currentQuestion.wrongAnswers.Length -1));
            while (newAnswerIndex == answerIndex)
            {
                newAnswerIndex = (int)Math.Round(UnityEngine.Random.value * (currentQuestion.wrongAnswers.Length - 1));
            }
            answerIndex = newAnswerIndex;

            newAnswer = false;
        }

        string answerText;
        if(answerIndex == 0)
        {
            answerText = currentQuestion.rightAnswer;
        }
        else
        {
            answerText = currentQuestion.wrongAnswers[answerIndex - 1];
        }

        answerAObj.GetComponent<TextMeshPro>().text = answerText;

        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            newQuestion = true;
            if(0 == answerIndex) //correct answer
            {
                studentScore += 1;
                studentScoreObj.GetComponent<TextMeshPro>().text = studentScore.ToString();
            }
            else
            {
                teacherScore += 1;
                teacherScoreObj.GetComponent<TextMeshPro>().text = teacherScore.ToString();
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            newQuestion = true;
            if (0 == answerIndex) //correct answer
            {
                teacherScore += 1;
                teacherScoreObj.GetComponent<TextMeshPro>().text = teacherScore.ToString();
            }
            else
            {
                studentScore += 1;
                studentScoreObj.GetComponent<TextMeshPro>().text = studentScore.ToString();
            }
        }

        if (currentTime - answerStartTime > secondsPerAnswer)
        {
            newAnswer = true;
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            GetComponent<GameManager>().EndMinigame(studentScore > teacherScore);
        }
    }
}
