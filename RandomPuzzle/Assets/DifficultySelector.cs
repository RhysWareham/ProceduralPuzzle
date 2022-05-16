using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySelector : MonoBehaviour
{
    [SerializeField] int difficulty;
    [SerializeField] PuzzleManager manager;
    private bool inRange = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(inRange)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("Pressed");
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
