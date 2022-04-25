using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private int lengthOfCode;
    [SerializeField] private GameObject codeBarPrefab;
    [SerializeField] private Transform codePanelPos;

    // Start is called before the first frame update
    void Start()
    {
        lengthOfCode = Random.Range(2, 5);
        for(int i = 0; i < lengthOfCode; i++)
        {
            int randNum = Random.Range(1, 9);
            PuzzleManagement.RequiredCode.Add(randNum);
            Debug.Log(randNum);
        }

        SpawnCodeBar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnCodeBar()
    {
        Instantiate(codeBarPrefab, codePanelPos);
    }
}
