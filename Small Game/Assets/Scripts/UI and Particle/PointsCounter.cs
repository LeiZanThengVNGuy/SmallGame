using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PointsCounter : MonoBehaviour
{
    public TextMeshProUGUI CurPoints;
    public TextMeshProUGUI[] HighScoresText;
    int CurrentScore = 0;
    int HighScore = 0;

    [HideInInspector] public bool AllowAddScore = true;
    [HideInInspector]public int MultipledPoints = 0;
    // Start is called before the first frame update
    void Start()
    {
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
        CurPoints.SetText(CurrentScore.ToString());
        for(int i = 0; i < HighScoresText.Length; i++)
        {
            HighScoresText[i].SetText("Highscore:" + HighScore.ToString());
        }
    }
    public void AddScore(int ScoreAmount)
    {
        if(AllowAddScore)
        {
            CurrentScore += ScoreAmount + 1 * MultipledPoints;
        }
        CurPoints.SetText(CurrentScore.ToString());
        for(int i = 0; i < HighScoresText.Length; i++)
        {
            HighScoresText[i].SetText("Highscore:" + HighScore.ToString());
        }
        if(HighScore < CurrentScore)
        {
            PlayerPrefs.SetInt("HighScore", CurrentScore);
        }
    }
}
