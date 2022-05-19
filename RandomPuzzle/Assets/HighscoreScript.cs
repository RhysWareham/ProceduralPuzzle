using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighscoreScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI easyScore;
    [SerializeField] private TextMeshProUGUI mediumScore;
    [SerializeField] private TextMeshProUGUI hardScore;
    private TextMeshProUGUI thisText;

    // Start is called before the first frame update
    void Start()
    {
        thisText = GetComponent<TextMeshProUGUI>();
    }


    /// <summary>
    /// Function for saving highscore
    /// </summary>
    public void CheckToSaveHighScore(float newTime)
    {
        //If the new time was faster than previous
        if (newTime < PuzzleManagement.FastestTime[(int)PuzzleManagement.ChosenDifficulty] ||
            PuzzleManagement.FastestTime[(int)PuzzleManagement.ChosenDifficulty] == 0)
        {
            //Update new fastest time
            PuzzleManagement.FastestTime[(int)PuzzleManagement.ChosenDifficulty] = ((Mathf.Round(newTime * 100f)) / 100f);
        }

        MakeHighScoresVisible();
    }


    /// <summary>
    /// Show high scores
    /// </summary>
    private void MakeHighScoresVisible()
    {
        thisText.text = "High Scores:";
        easyScore.text = "Easy: " + PuzzleManagement.FastestTime[0];
        mediumScore.text = "Medium: " + PuzzleManagement.FastestTime[1];
        hardScore.text = "Hard: " + PuzzleManagement.FastestTime[2];
    }


    /// <summary>
    /// Hide the highscores
    /// </summary>
    public void HideHighScores()
    {
        thisText.text = "";
        easyScore.text = "";
        mediumScore.text = "";
        hardScore.text = "";
    }
}
