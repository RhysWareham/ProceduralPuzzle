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
        //If even number of slots are needed
        if (PuzzleManagement.RequiredCode.Count % 2 == 0)
        {
            //Get the half count
            int halfCount = PuzzleManagement.RequiredCode.Count / 2;
            //If half count is not 1
            if (halfCount != 1)
            {
                //The start spawn point must subtract the slotOffset and
                //the slotSpacing multiplied by half of the number of slots needed
                startSpawnPointX = slotSpawnPoint.localPosition.x - slotOffset - (slotSpacing.x * (halfCount - 1));
            }
            //If half count is 1
            else
            {
                //The start spawn point only needs to be offset by the slot offset
                startSpawnPointX = slotSpawnPoint.localPosition.x - slotOffset;
            }

        }
        //If the required count is odd but not equal to 1
        else if (PuzzleManagement.RequiredCode.Count != 1)
        {
            //Set the half count
            int halfCount = (PuzzleManagement.RequiredCode.Count - 1) / 2;
            //Start spawn point only needs to be offset by the slot spacing multiplied by the half count,
            //as there will be no slot in the centre of the code bar
            startSpawnPointX = slotSpawnPoint.localPosition.x - (slotSpacing.x * halfCount);
        }

        //Create a vector3 of value zero and then set the x value to startSpawnPointX
        Vector3 spawnPointOffset = Vector3.zero;
        spawnPointOffset.x = startSpawnPointX;
        //Set the spawnpoint to be the slotSpawnPoint plus the spawnPointOffset
        Vector3 spawnPoint = slotSpawnPoint.localPosition + spawnPointOffset;
        spawnPoint = new Vector3(spawnPoint.x, 0, -0.6f);

        //Instantiate an emptyfield slot in each position for the correct number of code
        for (int i = 0; i < PuzzleManagement.RequiredCode.Count; i++)
        {
            GameObject slotPos = Instantiate(emptyFieldPrefab, slotSpawnPoint.position, Quaternion.identity);
            //Set the parent of the slots to be this transform
            slotPos.transform.SetParent(this.transform);
            slotPos.transform.localPosition = spawnPoint + (slotSpacing * i);

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
            //If on easy difficulty
            if(PuzzleManagement.ChosenDifficulty == PuzzleManagement.Difficulty.EASY)
            {
                //If the number is not in the correct position in the sequence, set sprite to the failed colour
                if(enterableSlots[i].GetComponent<SpriteRenderer>().color != Color.green)
                {
                    enterableSlots[i].GetComponent<SpriteRenderer>().color = failColor;

                }
            }
            //If not on easy, set all slot sprites to the failed colour
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
        yield return 0;
    }
}
