using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public List<QuestionAndAnswer> QnA;
    public GameObject[] options;
    public int currentQuestion;

    public UnityEngine.UI.Text QuestionTxt;

    public GameObject Quizpanel;
    public GameObject GoPanel;

    public Text ScoreTxt;

    int totalQuestion = 0;
    public int score;

    private void Start()
    {
        totalQuestion = QnA.Count;
        GoPanel.SetActive(false);
        generateQuestion();
    }

    public void retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    void GameOver()
    {
        Quizpanel.SetActive(false);
        GoPanel.SetActive(true);
        ScoreTxt.text = score + "/" + totalQuestion;
    }

    public void correct()
    {
        score += 1;
        QnA.RemoveAt(currentQuestion);
        generateQuestion();
    }

    public void wrong()
    {
        //when answer wrong
        QnA.RemoveAt(currentQuestion);
        generateQuestion();
    }

    void SetAnswers()
{
    // Get the number of options excluding the correct answer
    int numIncorrect = options.Length - 1;

    // Generate random incorrect answers
    List<string> incorrectAnswers = GenerateRandomIncorrectAnswers(numIncorrect);

    // Shuffle the options array to ensure the correct answer is at a random position
    ShuffleArray(options);

    // Set the correct answer
    int correctAnswerIndex = Random.Range(0, options.Length);
    options[correctAnswerIndex].GetComponent<AnswerScript>().isCorrect = true;
    options[correctAnswerIndex].transform.GetChild(0).GetComponent<Text>().text = QnA[currentQuestion].CorrectAnswer;

    // Set incorrect answers
    int incorrectIndex = 0;
    for (int i = 0; i < options.Length; i++)
    {
        if (i != correctAnswerIndex)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<Text>().text = incorrectAnswers[incorrectIndex];
            incorrectIndex++;
        }
    }
}


    void generateQuestion()
    {
        if (QnA.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA.Count);

            QuestionTxt.text = QnA[currentQuestion].Question;

            // Set the answers (both correct and incorrect)
            SetAnswers();
        }
        else
        {
            Debug.Log("Out of question");
            GameOver();
        }
    }

    List<string> GenerateRandomIncorrectAnswers(int numIncorrect)
    {
        // Here you can implement your logic to generate random incorrect answers.
        // For simplicity, let's say you have a list of predefined incorrect answers.
        List<string> incorrectAnswers = new List<string> { "あ", "い", "う", "え", "お", "か", "き", "く", "け", "こ" };

        // Remove the correct answer from the list of incorrect answers
        string correctAnswer = QnA[currentQuestion].CorrectAnswer;
        incorrectAnswers.Remove(correctAnswer);

        // Shuffle the remaining incorrect answers
        ShuffleList(incorrectAnswers);

        return incorrectAnswers.GetRange(0, numIncorrect);
    }

    void ShuffleArray(GameObject[] array)
    {
        // Fisher-Yates shuffle algorithm
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            GameObject temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    void ShuffleList<T>(List<T> list)
    {
        // Fisher-Yates shuffle algorithm
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
