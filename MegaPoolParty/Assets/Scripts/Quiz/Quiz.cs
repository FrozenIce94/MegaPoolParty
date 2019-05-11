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
public class QuestionCategory
{
    public string categoryName;
    public Question[] questions;
}


[Serializable]
public class QuestionDB
{
    public QuestionCategory[] categories;
}



public class Quiz : MonoBehaviour
{
    public GameObject questionObj;
    public GameObject categoryObj;
    public GameObject studentScoreObj;
    public GameObject teacherScoreObj;
    public GameObject answerAObj;
    public float secondsPerAnswer;
    public TextAsset txtDb;

    [Header("WinConditions")]
    [SerializeField] int scoreToWin = 10;

    int studentScore = 0;
    int teacherScore = 0;

    QuestionDB db;
    Question currentQuestion;

    int categoryIndex = 0;

    bool newQuestion = true;
    float questionStartTime;
    int questionIndex = 0;

    bool newAnswer = true;
    float answerStartTime;
    int answerIndex = 0;

    bool endRequested = false;

    ArrayList answerPermutation;
    ArrayList answeredQuestions;

    public void requestEnd()
    {
        endRequested = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        db = JsonUtility.FromJson<QuestionDB>(txtDb.text);
        categoryIndex = (int)Math.Round((db.categories.Length - 1) * UnityEngine.Random.value);
        categoryObj.GetComponent<TextMeshPro>().text = db.categories[categoryIndex].categoryName;
        answeredQuestions = new ArrayList();
        answerPermutation = new ArrayList();

    }

    bool timerRegistered = false;

    // Update is called once per frame
    void Update()
    {
        if(!timerRegistered)
            timerRegistered = GetComponent<GameManager>().StartTimer(requestEnd, GameManager.Games.Quiz);
        if(!timerRegistered)
        {
            return;
        }

        float currentTime = Time.time;

        if (newQuestion)
        {
            newAnswer = true;
            int newQuestionIndex = (int)Math.Round((db.categories[categoryIndex].questions.Length - 1) * UnityEngine.Random.value);
            while(answeredQuestions.Contains(newQuestionIndex))
            {
                newQuestionIndex = (int)Math.Round((db.categories[categoryIndex].questions.Length - 1) * UnityEngine.Random.value);
            }
            questionIndex = newQuestionIndex;

            currentQuestion = db.categories[categoryIndex].questions[questionIndex];

            TextMeshPro questionTextComp = questionObj.GetComponent<TextMeshPro>();
            questionTextComp.text = currentQuestion.questionText;

            answerPermutation.Clear();
            answerPermutation.Add(0);
            for(int i = 0; i < currentQuestion.wrongAnswers.Length; ++i)
            {
                answerPermutation.Add(i + 1);
            }
            System.Random r = new System.Random();
            answerPermutation = ShuffleList<int>(answerPermutation);

            questionStartTime = currentTime;
            newQuestion = false;
        }
        
        if (newAnswer)
        {
            answerStartTime = currentTime;
            answerIndex += 1;
            answerIndex = answerIndex % (currentQuestion.wrongAnswers.Length + 1);

            newAnswer = false;
        }

        string answerText;
        if((int)answerPermutation[answerIndex] == 0)
        {
            answerText = currentQuestion.rightAnswer;
        }
        else
        {
            answerText = currentQuestion.wrongAnswers[(int)answerPermutation[answerIndex] - 1];
        }

        answerAObj.GetComponent<TextMeshPro>().text = answerText;

        if(Input.GetKeyDown(KeyCode.A)  || 
            Input.GetKeyDown(KeyCode.W) || 
            Input.GetKeyDown(KeyCode.S) || 
            Input.GetKeyDown(KeyCode.D) ||
            Input.GetButtonDown("Fire1_S"))
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

            answeredQuestions.Add(questionIndex);
            if(answeredQuestions.Count >= db.categories[categoryIndex].questions.Length)
            {
                answeredQuestions.Clear();
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)  || 
            Input.GetKeyDown(KeyCode.DownArrow)     || 
            Input.GetKeyDown(KeyCode.LeftArrow)     || 
            Input.GetKeyDown(KeyCode.RightArrow)    ||
            Input.GetButtonDown("Fire1_L"))
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
            answeredQuestions.Add(questionIndex);
            if (answeredQuestions.Count >= db.categories[categoryIndex].questions.Length)
            {
                answeredQuestions.Clear();
            }

        }

        if (currentTime - answerStartTime > secondsPerAnswer)
        {
            newAnswer = true;
        }
        if (studentScore >= scoreToWin || teacherScore >= scoreToWin)
            endRequested = true;
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GetComponent<GameManager>().EndMinigame(null);
        }

        if (endRequested)
        {
            if (teacherScore == studentScore)
            {
                GetComponent<GameManager>().EndMinigame(null);
            }
            else if (teacherScore < studentScore)
            {
                GetComponent<GameManager>().EndMinigame(true);
            }
            else
            {
                GetComponent<GameManager>().EndMinigame(false);
            }
        }
    }

    private ArrayList ShuffleList<E>(ArrayList inputList)
    {
        ArrayList randomList = new ArrayList();

        System.Random r = new System.Random();
        int randomIndex = 0;
        while (inputList.Count > 0)
        {
            randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
            randomList.Add(inputList[randomIndex]); //add it to the new, random list
            inputList.RemoveAt(randomIndex); //remove to avoid duplicates
        }

        return randomList; //return the new random list
    }

}
