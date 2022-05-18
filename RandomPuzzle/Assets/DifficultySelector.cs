using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySelector : MonoBehaviour
{
    [SerializeField] private int difficulty;
    [SerializeField] private PuzzleManager manager;
    private bool inRange = false;



    // Update is called once per frame
    void Update()
    {
        //If in range
        if(inRange)
        {
            //If enter has been pressed
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("Pressed");
                //Call the create puzzle function using specified difficulty
                manager.CreatePuzzle(difficulty);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        inRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        inRange = false;
    }
}
