using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private int lengthOfCode;
    [SerializeField] private GameObject keyCodePanelPrefab;
    [SerializeField] private Transform codePanelPos;
    private CodeBarScript codeBar;
    //[SerializeField] private PuzzleManagement.Difficulty difficulty;
    [SerializeField] private GameObject pillarSpawnerPrefab;
    [SerializeField] private Transform pillarSpawnPos;
    private PillarSpawningScript pillarSpawner;
    [SerializeField] private float pillarRisingSpeed;

    [SerializeField] private DoorControl door;
    [SerializeField] private DoorControl backDoor;
    [SerializeField] private List<DifficultySelector> buttons = new List<DifficultySelector>();
    private bool firstTime = true;

    [SerializeField] private Timer timer;

    private Coroutine pillarMovingCoroutine;
    

    /// <summary>
    /// Function to reset the puzzle room
    /// </summary>
    public void ResetPuzzle()
    {
        //Destroy the current pillar spawner
        Destroy(pillarSpawner);

        //Clear the previous code
        PuzzleManagement.RequiredCode.Clear();
        PuzzleManagement.ShuffledOrder.Clear();

        //Restart the code bar
        codeBar.RestartCodeBar();

        //Stop coroutine if running
        if(pillarMovingCoroutine != null)
        {
            StopCoroutine(pillarMovingCoroutine);
        }

        //Make all difficulty buttons visible
        foreach (DifficultySelector button in buttons)
        {
            button.MoveButton(true);
        }

    }


    /// <summary>
    /// Function to start the creation of the puzzle
    /// </summary>
    /// <param name="inputDifficulty"></param>
    public void CreatePuzzle(int inputDifficulty)
    {
        //Shut the back door behind player
        ShutBackDoor();

        //Hide all difficulty buttons
        foreach(DifficultySelector button in buttons)
        {
            button.MoveButton(false);
        }

        //Set the chosen difficulty
        PuzzleManagement.ChosenDifficulty = (PuzzleManagement.Difficulty)inputDifficulty;

        //Set the length of code sequence from a range based on the difficulty
        switch (PuzzleManagement.ChosenDifficulty)
        {
            case PuzzleManagement.Difficulty.EASY:
                lengthOfCode = Random.Range(2, 5);
                break;
            case PuzzleManagement.Difficulty.MEDIUM:
                lengthOfCode = Random.Range(4, 7);
                break;
            case PuzzleManagement.Difficulty.HARD:
                lengthOfCode = Random.Range(5, 9);
                break;
        }
        
        //Generate the code
        GenerateCode();

        //If this is the first playthrough
        if(firstTime)
        {
            //Spawn the code panel
            SpawnCodeBar();
            firstTime = false;
        }
        else
        {
            //If the code panel already exists, spawn the correct number of slots on the code bar
            codeBar.SpawnCodeBarSlots();
        }

        //Spawn the pillar creator
        SpawnPillarCreator();

        //Set puzzle complete to false
        PuzzleManagement.PuzzleComplete = false;
        
        //Start the timer
        timer.StartTimer();

        //Raise the pillars
        pillarMovingCoroutine = StartCoroutine(MovePillars());
    }


    /// <summary>
    /// Coroutine to move raise the pillars through the ground
    /// </summary>
    /// <returns></returns>
    private IEnumerator MovePillars()
    {
        //Set pillars to be below the ground on start
        pillarSpawner.transform.position = new Vector3(0, -8, 0);

        //While pillar spawner position is has not reached 0, raise the spawner
        while(pillarSpawner.transform.position != Vector3.zero)
        {
            pillarSpawner.transform.position = Vector3.Slerp(pillarSpawner.transform.position, Vector3.zero, Time.deltaTime * pillarRisingSpeed);

            yield return 0;
        }
    }


    /// <summary>
    /// Function to generate a random code of correct length
    /// </summary>
    private void GenerateCode()
    {
        //Choose enough random numbers for the length of code,
        //and store them in a required code list
        for (int i = 0; i < lengthOfCode; i++)
        {
            int randNum = Random.Range(1, 10);
            PuzzleManagement.RequiredCode.Add(randNum);
        }
        
        
        Debug.Log("lengthOfCode: " + lengthOfCode);
    }


    /// <summary>
    /// Instantiates the code bar panel
    /// </summary>
    private void SpawnCodeBar()
    {
        codeBar = Instantiate(keyCodePanelPrefab, codePanelPos).GetComponentInChildren<CodeBarScript>();
    }


    /// <summary>
    /// Instantiates the pillar spawner
    /// </summary>
    private void SpawnPillarCreator()
    {
        pillarSpawner = Instantiate(pillarSpawnerPrefab, pillarSpawnPos).GetComponent<PillarSpawningScript>();
    }
    

    /// <summary>
    /// Call the unlock door function
    /// </summary>
    public void ActivateDoor()
    {
        //Stop the timer as puzzle is complete
        PuzzleManagement.PuzzleComplete = true;
        timer.StopTimer();
        door.Unlock();
    }


    /// <summary>
    /// Calls the lock function for back door
    /// </summary>
    public void ShutBackDoor()
    {
        backDoor.Lock();
    }
}
