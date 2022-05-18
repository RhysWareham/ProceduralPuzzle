using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private int lengthOfCode;
    [SerializeField] private GameObject keyCodePanelPrefab;
    [SerializeField] private Transform codePanelPos;
    private CodeBarScript codeBar;
    [SerializeField] private PuzzleManagement.Difficulty difficulty;
    [SerializeField] private GameObject pillarSpawnerPrefab;
    [SerializeField] private Transform pillarSpawnPos;
    private PillarSpawningScript pillarSpawner;

    [SerializeField] private DoorControl door;
    [SerializeField] private DoorControl backDoor;
    [SerializeField] private List<DifficultySelector> buttons = new List<DifficultySelector>();
    private bool firstTime = true;

    // Start is called before the first frame update
    void Start()
    {
        //CreatePuzzle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
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

        //Make difficulty selection visible
        foreach (DifficultySelector button in buttons)
        {
            button.gameObject.SetActive(true);
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

        //Deactivate all difficulty buttons
        foreach(DifficultySelector button in buttons)
        {
            button.gameObject.SetActive(false);
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
