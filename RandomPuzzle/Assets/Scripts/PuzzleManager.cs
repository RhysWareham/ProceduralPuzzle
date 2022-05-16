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

    public void Restart()
    {
        pillarSpawner.DestroyAllPillars();
        Destroy(pillarSpawner);

        //Clear the previous code
        PuzzleManagement.RequiredCode.Clear();
        PuzzleManagement.ShuffledOrder.Clear();

        codeBar.RestartCodeBar();

        foreach (DifficultySelector button in buttons)
        {
            button.gameObject.SetActive(true);
        }
    }

    public void CreatePuzzle(int inputDifficulty)
    {
        ShutBackDoor();

        foreach(DifficultySelector button in buttons)
        {
            button.gameObject.SetActive(false);
        }

        PuzzleManagement.ChosenDifficulty = (PuzzleManagement.Difficulty)inputDifficulty;

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
        //lengthOfCode = 3;
        //Generate code and code panel
        GenerateCode();
        if(firstTime)
        {
            SpawnCodeBar();
            firstTime = false;
        }
        else
        {
            codeBar.SpawnCodeBarSlots();
        }

        SpawnPillarCreator();
    }

    private void GenerateCode()
    {
        for (int i = 0; i < lengthOfCode; i++)
        {
            int randNum = Random.Range(1, 10);
            PuzzleManagement.RequiredCode.Add(randNum);
            //PuzzleManagement.RequiredCode.Add(3);
            //Debug.Log(randNum);
        }
        //PuzzleManagement.RequiredCode.Add(1);
        //PuzzleManagement.RequiredCode.Add(2);
        //PuzzleManagement.RequiredCode.Add(1);
        
        Debug.Log("lengthOfCode: " + lengthOfCode);
    }

    private void SpawnCodeBar()
    {
        codeBar = Instantiate(keyCodePanelPrefab, codePanelPos).GetComponentInChildren<CodeBarScript>();
    }

    private void SpawnPillarCreator()
    {
        pillarSpawner = Instantiate(pillarSpawnerPrefab, pillarSpawnPos).GetComponent<PillarSpawningScript>();
    }
    
    public void ActivateDoor()
    {
        door.Unlock();
    }

    public void ShutBackDoor()
    {
        backDoor.Lock();
    }
}
