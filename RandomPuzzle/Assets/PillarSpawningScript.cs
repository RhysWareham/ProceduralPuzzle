using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarSpawningScript : MonoBehaviour
{
    private int numOfRows;
    private int numOfCols;
    private int[,] codeGrid;
    private int[,] directionalGrid; // 1 = North, 2 = East, 3 = South, 4 = West

    private void Awake()
    {
        SetNumOfRowCols();
        
        codeGrid = new int[numOfRows, numOfCols];

        directionalGrid = new int[numOfRows, numOfCols];

        for(int i = 0; i < numOfRows; i++)
        {
            for (int j = 0; j < numOfCols; j++)
            {
                //Set codeGrid and directionalGrid to 0's
                codeGrid[i, j] = 0;
                directionalGrid[i, j] = 0;
            }
        }

        FillCodeGrid();
    }

    private void SetNumOfRowCols()
    {
        switch (PuzzleManagement.ChosenDifficulty)
        {
            case PuzzleManagement.Difficulty.EASY:
                numOfRows = Random.Range(2, 4);
                numOfCols = Random.Range(2, 4);
                break;
            case PuzzleManagement.Difficulty.MEDIUM:
                numOfRows = Random.Range(3, 5);
                numOfCols = Random.Range(3, 5);
                break;
            case PuzzleManagement.Difficulty.HARD:
                numOfRows = Random.Range(3, 6);
                numOfCols = Random.Range(4, 6);
                break;
            default:
                numOfRows = 3;
                numOfCols = 3;
                break;
        }
        Debug.Log("num of rows: " + numOfRows);
        Debug.Log("num of cols: " + numOfCols);
    }

    private void FillCodeGrid()
    {
        int codeNumsInGrid = 0;
        int numOfCodeNeeded = PuzzleManagement.RequiredCode.Count;
        int numOfValidSpaces = (numOfCols * 2) + (numOfRows * 2) - 4;
        int spacesLeft = numOfValidSpaces;

        for (int i = 0; i < numOfRows; i++)
        {
            if (codeNumsInGrid == numOfCodeNeeded)
            {
                break;
            }

            for (int x = 0; x < numOfCols; x++)
            {
                if(codeNumsInGrid == numOfCodeNeeded)
                {
                    break;
                }

                if (i != 0 && i != numOfRows - 1
                    && x != 0 && x != numOfCols - 1)
                {
                    //Don't allow number start to be in middle of pillars
                    codeGrid[i, x] = 10; //10 = not a number
                    continue;
                }

                int shouldEnter = Random.Range(0, 2);
                
                //If there are only just enough spaces left to fill grid, Definitely enter this instance
                if(spacesLeft == numOfCodeNeeded - codeNumsInGrid)
                {
                    shouldEnter = 1;
                }
                spacesLeft--;

                //If true
                if (shouldEnter == 1)
                {
                    codeGrid[i, x] = PuzzleManagement.RequiredCode[codeNumsInGrid];
                    codeNumsInGrid++;

                    Debug.Log("Index " + i + "," + x + "= " + codeGrid[i, x]);


                    //If top row
                    if (i == 0)
                    {
                        //If top left
                        if (x == 0)
                        {
                            int randChoice = Random.Range(0, 2);
                            if (randChoice == 1)
                            {
                                //If top row, direction to view number goes down
                                directionalGrid[i, x] = 3;
                            }
                            else
                            {
                                //If Left column, direction goes Right
                                directionalGrid[i, x] = 2;
                            }
                        }
                        //If top right
                        else if (x == numOfCols - 1)
                        {
                            int randChoice = Random.Range(0, 2);
                            if (randChoice == 1)
                            {
                                //If top row, direction to view number goes down
                                directionalGrid[i, x] = 3;
                            }
                            else
                            {
                                //If Right column, direction goes Left
                                directionalGrid[i, x] = 4;
                            }
                        }
                        //If not corner position
                        else
                        {
                            //If top row, direction to view number goes down
                            directionalGrid[i, x] = 3;
                        }
                        continue;
                    }
                    //If bottom row
                    if (i == numOfRows - 1)
                    {
                        //If bottom left
                        if (x == 0)
                        {
                            int randChoice = Random.Range(0, 2);
                            if (randChoice == 1)
                            {
                                //If bottom row, direction to view number goes up
                                directionalGrid[i, x] = 1;
                            }
                            else
                            {
                                //If Left column, direction goes Right
                                directionalGrid[i, x] = 2;
                            }
                        }
                        //If bottom right
                        else if (x == numOfCols - 1)
                        {
                            int randChoice = Random.Range(0, 2);
                            if (randChoice == 1)
                            {
                                //If bottom row, direction to view number goes up
                                directionalGrid[i, x] = 1;
                            }
                            else
                            {
                                //If Right column, direction goes Left
                                directionalGrid[i, x] = 4;
                            }
                        }
                        //If not corner position
                        else
                        {
                            //If bottom row, direction to view number goes up
                            directionalGrid[i, x] = 1;
                        }
                        continue;
                    }
                    //If Left column
                    if (x == 0)
                    {
                        //If Left column, direction goes Right
                        directionalGrid[i, x] = 2;
                        continue;
                    }
                    if (x == numOfCols - 1)
                    {
                        //If Right column, direction goes Left
                        directionalGrid[i, x] = 4;
                        continue;
                    }

                    


                }

            }
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
