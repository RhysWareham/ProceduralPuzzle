using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySelector : MonoBehaviour
{
    [SerializeField] public int difficulty;
    [SerializeField] private PuzzleManager manager;
    private bool inRange = false;
    [SerializeField] float hiddenYValue;
    [SerializeField] float visibleYValue;
    private Coroutine currentCoroutine;
    private float moveSpeed = 5f;


    // Update is called once per frame
    void Update()
    {
        //If in range
        if(inRange)
        {
            //If enter has been pressed
            if (Input.GetKeyDown(KeyCode.Return))
            {
                //Don't allow for spamming of pillar creation
                GetComponent<Collider>().enabled = false;
                inRange = false;

                Debug.Log("Pressed");
                //Call the create puzzle function using specified difficulty
                manager.CreatePuzzle(difficulty);
            }

        }
    }


    /// <summary>
    /// Function to start a coroutine moving the button up or down
    /// </summary>
    /// <param name="moveUp"></param>
    public void MoveButton(bool moveUp)
    {
        //If coroutine is already running, stop it
        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        //Start coroutine feeding in the hidden or visible Y value
        if(moveUp)
        {
            GetComponent<Collider>().enabled = true;
            currentCoroutine = StartCoroutine(HideOrUnhideButton(visibleYValue));

        }
        else
        {
            currentCoroutine = StartCoroutine(HideOrUnhideButton(hiddenYValue));
        }
    }


    /// <summary>
    /// Coroutine to move button to position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private IEnumerator HideOrUnhideButton(float position)
    {
        //Set the target position using the Y value
        Vector3 targetPos = new Vector3(this.transform.position.x, position, this.transform.position.z);
        
        //While the button is not in position, move it towards the target positon
        while(this.transform.position.y != position)
        {
            this.transform.position = Vector3.Slerp(this.transform.position, targetPos, Time.deltaTime * moveSpeed);
            yield return 0;
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
