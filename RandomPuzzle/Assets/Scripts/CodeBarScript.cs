using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeBarScript : MonoBehaviour
{
    [SerializeField] private List<Transform> enterableSlots = new List<Transform>();
    private Sprite emptyField;
    private List<int> enteredCode = new List<int>();
    private int numOfNumbersEntered = 0;
    public bool codeFilled = false;
    public bool codebarReady = true;

    [SerializeField] private GameObject emptyFieldPrefab;
    [SerializeField] private Transform slotSpawnPoint;
    private float slotOffset = 0.065f;
    private Vector3 slotSpacing = new Vector3(0.13f,0,0);

    private PuzzleManager puzzleManager;

    private void Start()
    {
        //Get a reference to the puzzle manager
        puzzleManager = FindObjectOfType<PuzzleManager>();
        //Set empty field sprite to the blank slot sprite
        emptyField = enterableSlots[0].GetComponent<SpriteRenderer>().sprite;
    }


    private void Awake()
    {
        //Spawn the code bar slots
        SpawnCodeBarSlots();
    }


    /// <summary>
    /// Resets the codebar
    /// </summary>
    public void RestartCodeBar()
    {
        //Destroy all slots in bar
        foreach(Transform t in enterableSlots)
        {
            Destroy(t.gameObject);
        }
        //Clear the previous slot list
        enterableSlots.Clear();
        //Clear the entered code
        enteredCode.Clear();
        codeFilled = false;
        numOfNumbersEntered = 0;
    }


    /// <summary>
    /// Function to spawn the slot sprites on the code bar, and spread evenly
    /// </summary>
    public void SpawnCodeBarSlots()
    {
        float startSpawnPointX = 0;
        //Work out the X axis spawn value
        startSpawnPointX = UsefulFunctions.WorkOutStartSpawnValue(slotOffset, slotSpacing.x, PuzzleManagement.RequiredCode.Count, slotSpawnPoint.localPosition.x);

        //Create a vector3 using the x value of startSpawnPointX
        Vector3 spawnPointOffset = new Vector3(startSpawnPointX, 0, 0);
        //Set the startSpawnPoint to be the slotSpawnPoint plus the spawnPointOffset
        Vector3 startSpawnPoint = slotSpawnPoint.localPosition + spawnPointOffset;
        startSpawnPoint = new Vector3(startSpawnPoint.x, 0, -0.6f);

        //Instantiate an emptyfield slot in each position for the correct number of code
        for (int i = 0; i < PuzzleManagement.RequiredCode.Count; i++)
        {
            GameObject slotPos = Instantiate(emptyFieldPrefab, slotSpawnPoint.position, Quaternion.identity);
            //Set the parent of the slots to be this transform
            slotPos.transform.SetParent(this.transform);
            slotPos.transform.localPosition = startSpawnPoint + (slotSpacing * i);

            //Add each slot to a list
            enterableSlots.Add(slotPos.transform);
        }
    }


    /// <summary>
    /// Function for when a player has entered a number into the code panel
    /// </summary>
    /// <param name="enterredNumberSprite"></param>
    /// <param name="enterredNumber"></param>
    public void EnterButtonNumber(Sprite enterredNumberSprite, int enterredNumber)
    {
        //Add the entered number to a list
        enteredCode.Add(enterredNumber);
        //Set the codebar slot sprite to the number enterred and increase the number of numbers enterred
        enterableSlots[numOfNumbersEntered].GetComponent<SpriteRenderer>().sprite = enterredNumberSprite;
        numOfNumbersEntered++;
        
        //If the player has entered a number into all possible slots
        if (numOfNumbersEntered == enterableSlots.Count)
        {
            //Set codeFilled to true
            codeFilled = true;
        }
    }


    /// <summary>
    /// Function to check if the enterred code is correct
    /// </summary>
    public void CheckCode()
    {
        //Loop through all numbers in code sequence
        for(int i = 0; i < PuzzleManagement.RequiredCode.Count; i++)
        {
            //If the entered number is the same as the required number in this position of the sequence
            if (enteredCode[i] == PuzzleManagement.RequiredCode[i])
            {
                //Then set the slot sprite to green
                enterableSlots[i].GetComponent<SpriteRenderer>().color = Color.green;
                
                //If all numbers entered have been correct
                if(i == PuzzleManagement.RequiredCode.Count-1)
                {
                    //Call the activate door function
                    puzzleManager.ActivateDoor();
                }
            }
            //If any number has been incorrect
            else
            {
                //Call the reset codeBar function and return out of the CheckCode function
                ResetCodeBarOnFailure();
                return;
            }
        }
        
    }


    /// <summary>
    /// Reset the code bar by clearing the entered values
    /// </summary>
    public void ResetCodeBarOnFailure()
    {
        codebarReady = false;
        //Start a coroutine to flash the slots red
        StartCoroutine(FlashColour(Color.red));

        //Clear the entered values
        numOfNumbersEntered = 0;
        enteredCode.Clear();
        codeFilled = false;
        
    }


    /// <summary>
    /// Flashes the slot sprites a specified colour
    /// </summary>
    /// <param name="failColor"></param>
    /// <returns></returns>
    public IEnumerator FlashColour(Color failColor)
    {
        for (int i = 0; i < PuzzleManagement.RequiredCode.Count; i++)
        {
            //If not on hard mode
            if(PuzzleManagement.ChosenDifficulty != PuzzleManagement.Difficulty.HARD)
            {
                //If the number is not in the correct position in the sequence, set sprite to the failed colour
                if(enterableSlots[i].GetComponent<SpriteRenderer>().color != Color.green)
                {
                    enterableSlots[i].GetComponent<SpriteRenderer>().color = failColor;

                }
            }
            //If on hard, set all slot sprites to the failed colour after an attempt
            else
            {
                enterableSlots[i].GetComponent<SpriteRenderer>().color = failColor;

            }
        }

        //Wait for 1 second
        yield return new WaitForSeconds(1f);

        //Set all slot sprites back to white and an empty field
        for (int i = 0; i < PuzzleManagement.RequiredCode.Count; i++)
        {
            enterableSlots[i].GetComponent<SpriteRenderer>().sprite = emptyField;
            enterableSlots[i].GetComponent<SpriteRenderer>().color = Color.white;
        }

        codebarReady = true;
        yield return 0;
    }
}
