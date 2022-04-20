using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public static ScoreController Instance;

    [SerializeField] private TMP_Text _scoreText;

    private int _currentScore = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ResetScore()
    {
        _scoreText.text = "000000000"; // TODO - do it on better way!
        _currentScore = 000000000;

    }

    public void AppendScore( int scoreToAppend)
    {
        _currentScore += scoreToAppend;


        string numberOfDigits = _currentScore.ToString();

        //Debug.Log("Number of digits = " + numberOfDigits.Length);

        int numbersOfZeroesToAdd = 9 - numberOfDigits.Length;

        string finalResult = "";

        for (int i = 0; i <= numbersOfZeroesToAdd-1; i++)
        {
            finalResult += "0";
        }

        finalResult += _currentScore.ToString();
        _scoreText.text = finalResult;

    }

}
