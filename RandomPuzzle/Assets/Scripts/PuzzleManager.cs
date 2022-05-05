using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private int lengthOfCode;
    [SerializeField] private GameObject codeBarPrefab;
    [SerializeField] private Transform codePanelPos;
    [SerializeField] private PuzzleManagement.Difficulty difficulty;
    [SerializeField] private GameObject pillarSpawnerPrefab;
    [SerializeField] private Transform pillarSpawnPos;

    // Start is called before the first frame update
    void Start()
    {
        CreatePuzzle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreatePuzzle()
    {
        PuzzleManagement.ChosenDifficulty = difficulty;

        switch (PuzzleManagement.ChosenDifficulty)
        {
            case PuzzleManagement.Difficulty.EASY:
                lengthOfCode = Random.Range(2, 5);
                break;
            case PuzzleManagement.Difficulty.MEDIUM:
                lengthOfCode = Random.Range(4, 8);
                break;
            case PuzzleManagement.Difficulty.HARD:
                lengthOfCode = Random.Range(5, 9);
                break;

        }
        
        //Generate code and code panel
        GenerateCode();
        SpawnCodeBar();

        SpawnPillarCreator();
    }

    private void GenerateCode()
    {
        for (int i = 0; i < lengthOfCode; i++)
        {
            int randNum = Random.Range(1, 9);
            PuzzleManagement.RequiredCode.Add(randNum);
            //Debug.Log(randNum);
        }
        Debug.Log("lengthOfCode: " + lengthOfCode);
    }

    private void SpawnCodeBar()
    {
        Instantiate(codeBarPrefab, codePanelPos);
    }

    private void SpawnPillarCreator()
    {
        Instantiate(pillarSpawnerPrefab, pillarSpawnPos);
    }
}
