using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI timeText2;
    private float currentTime = 0;
    private bool timing = false;
    [SerializeField] private HighscoreScript highScores;
    
    // Start is called before the first frame update
    void Start()
    {
        timeText2 = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //If timing
        if(timing)
        {
            //Increment the timer
            currentTime += Time.deltaTime;
            //Update the timer text
            timeText2.text = ((Mathf.Round(currentTime*100f))/100f).ToString();

        }
    }

    /// <summary>
    /// Function to stop timer
    /// </summary>
    public void StopTimer()
    {
        timing = false;
        //If puzzle is complete and timer is more than 0
        if (PuzzleManagement.PuzzleComplete == true && currentTime > 0)
        {
            //Check to save the highscore and then reset timer
            timing = false;
            highScores.CheckToSaveHighScore(currentTime);
            ResetTimer();
        }
    }


    /// <summary>
    /// Function to start timer
    /// </summary>
    public void StartTimer()
    {
        if(currentTime == 0)
        {
            //Hide high scores
            highScores.HideHighScores();
        }
        timing = true;
    }


    /// <summary>
    /// Function to reset the timer
    /// </summary>
    private void ResetTimer()
    {
        currentTime = 0;
    }


}
