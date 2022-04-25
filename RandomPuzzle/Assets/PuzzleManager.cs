using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private int lengthOfCode;


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < lengthOfCode; i++)
        {
            int randNum = Random.Range(1, 9);
            PuzzleManagement.RequiredCode.Add(randNum);
            Debug.Log(randNum);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
