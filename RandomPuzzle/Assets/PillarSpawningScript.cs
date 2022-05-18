using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarSpawningScript : MonoBehaviour
{
    private int numOfRows;
    private int numOfCols;
    private int[,] codeGrid;
    private int[,] directionalGrid; // 1 = North, 2 = East, 3 = South, 4 = West
    private int[,] numOfDirectionGrid;
    private int numOfCodeNeeded;

    private enum Directions
    {
        NONE,
        NORTH,
        EAST,
        SOUTH,
        WEST
    };

    //Pillar spawning
    private float pillarSpacing = 9f;
    private float spawnOffset = 4.5f;
    [SerializeField] GameObject twoWayPillarPrefab;
    [SerializeField] GameObject fourWayPillarPrefab;

    private PillarNumberScript[,] PillarNumScript;

    [SerializeField] private GameObject num3;
    [SerializeField] private List<SpriteRenderer> thisNumberParts = new List<SpriteRenderer>();
    [SerializeField] private List<WholeNumberParts> allWholeNumbers = new List<WholeNumberParts>();
    private List<int> numOfPartsInEachPillar = new List<int>();
    private List<int> SpreadCodeLayout = new List<int>();


    /// <summary>
    /// On creation of this script
    /// </summary>
    private void Awake()
    {
        //Store the length of code
        numOfCodeNeeded = PuzzleManagement.RequiredCode.Count;

        //Add each number in code to a list for spreading out
        for(int i = 0; i < numOfCodeNeeded; i++)
        {
            SpreadCodeLayout.Add(PuzzleManagement.RequiredCode[i]);
            Debug.Log(PuzzleManagement.RequiredCode[i]);
        }

        //If hard setting, shuffle the layout so the grid doesnt go in index order
        if(PuzzleManagement.ChosenDifficulty == PuzzleManagement.Difficulty.HARD)
        {
            UsefulFunctions.Shuffle(SpreadCodeLayout);
        }

        //Set the num of rows and collumns
        SetNumOfRowCols();
        
        //Set the grid arrays to be of the height and width of rows and columns
        codeGrid = new int[numOfRows, numOfCols];
        directionalGrid = new int[numOfRows, numOfCols];
        numOfDirectionGrid = new int[numOfRows, numOfCols];
        PillarNumScript = new PillarNumberScript[numOfRows, numOfCols];

        //Loop through all instances in 2D arrays
        for(int i = 0; i < numOfRows; i++)
        {
            for (int j = 0; j < numOfCols; j++)
            {
                //Set codeGrid and directionalGrid to 0's
                codeGrid[i, j] = 0;
                directionalGrid[i, j] = 0;
            }
        }

        //Fill the code grid
        FillCodeGrid();

        //Spawn all pillars
        SpawnPillars();

        //Check direction numbers should be spread out across
        CheckDirectionForNumber();
    }


    /// <summary>
    /// Set the number of rows and collumns for this instance of the puzzle
    /// </summary>
    private void SetNumOfRowCols()
    {
        //Set the number of rows and columns to a random range,
        //varying depending on the chosen difficulty
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
                numOfRows = Random.Range(4, 6);
                numOfCols = Random.Range(4, 8);
                break;
            default:
                numOfRows = 3;
                numOfCols = 3;
                break;
        }
        
        Debug.Log("num of rows: " + numOfRows);
        Debug.Log("num of cols: " + numOfCols);
    }


    /// <summary>
    /// Function to fill code grid array with code values
    /// </summary>
    private void FillCodeGrid()
    {
        int codeNumsInGrid = 0;
        
        //Set the number of valid spaces, not allowing two numbers to start on same pillar
        int numOfValidSpaces = (numOfCols * 2) + (numOfRows * 2) - 4;
        int spacesLeft = numOfValidSpaces;

        for (int i = 0; i < numOfRows; i++)
        {
            //If all code numbers have been entered into the grid, break out of loop
            if (codeNumsInGrid == numOfCodeNeeded)
            {
                break;
            }

            for (int j = 0; j < numOfCols; j++)
            {
                //If all code numbers have been entered, break out of loop
                if(codeNumsInGrid == numOfCodeNeeded)
                {
                    break;
                }

                //Don't allow number start to be in middle of pillars
                if (i != 0 && i != numOfRows - 1
                    && j != 0 && j != numOfCols - 1)
                {
                    codeGrid[i, j] = 10; //10 = not a number
                    continue;
                }

                //Number to decide if code number should be entered in this space
                int shouldEnter = Random.Range(0, 2);
                
                //If there are only just enough spaces left to fill grid,
                //Definitely enter the number this instance
                if(spacesLeft == numOfCodeNeeded - codeNumsInGrid)
                {
                    shouldEnter = 1;
                }
                spacesLeft--;

                //If code number should be entered
                if (shouldEnter == 1)
                {
                    //Set grid instance to the code number and increase number of numbers in grid
                    codeGrid[i, j] = SpreadCodeLayout[codeNumsInGrid];
                    codeNumsInGrid++;

                    //If grid instance is on top row
                    if (i == 0)
                    {
                        //If top left
                        if (j == 0)
                        {
                            //Number to decide if number to be spread out in which direction
                            int randChoice = Random.Range(0, 2);
                            if (randChoice == 1)
                            {
                                //If top row, direction to view number goes down
                                directionalGrid[i, j] = 3;
                                //Increase the amount of numbers on this pillar
                                AddNumberofNumsOnPillar(true, j);
                            }
                            else
                            {
                                //If Left column, direction goes Right
                                directionalGrid[i, j] = 2;
                                AddNumberofNumsOnPillar(false, i);
                            }
                        }
                        //If top right
                        else if (j == numOfCols - 1)
                        {
                            int randChoice = Random.Range(0, 2);
                            if (randChoice == 1)
                            {
                                //If top row, direction to view number goes down
                                directionalGrid[i, j] = 3;
                                AddNumberofNumsOnPillar(true, j);
                            }
                            else
                            {
                                //If Right column, direction goes Left
                                directionalGrid[i, j] = 4;
                                AddNumberofNumsOnPillar(false, i);
                            }
                        }
                        //If not corner position
                        else
                        {
                            //If top row, direction to view number goes down
                            directionalGrid[i, j] = 3;
                            AddNumberofNumsOnPillar(true, j);
                        }
                        Debug.Log("Index " + i + "," + j + "= " + codeGrid[i, j] + " going " + (Directions)directionalGrid[i, j]);

                        //Continue to next array index
                        continue;
                    }

                    //If on bottom row
                    if (i == numOfRows - 1)
                    {
                        //If bottom left
                        if (j == 0)
                        {
                            int randChoice = Random.Range(0, 2);
                            if (randChoice == 1)
                            {
                                //If bottom row, direction to view number goes up
                                directionalGrid[i, j] = 1;
                                AddNumberofNumsOnPillar(true, j);
                            }
                            else
                            {
                                //If Left column, direction goes Right
                                directionalGrid[i, j] = 2;
                                AddNumberofNumsOnPillar(false, i);
                            }
                        }
                        //If bottom right
                        else if (j == numOfCols - 1)
                        {
                            int randChoice = Random.Range(0, 2);
                            if (randChoice == 1)
                            {
                                //If bottom row, direction to view number goes up
                                directionalGrid[i, j] = 1;
                                AddNumberofNumsOnPillar(true, j);
                            }
                            else
                            {
                                //If Right column, direction goes Left
                                directionalGrid[i, j] = 4;
                                AddNumberofNumsOnPillar(false, i);
                            }
                        }
                        //If not corner position
                        else
                        {
                            //If bottom row, direction to view number goes up
                            directionalGrid[i, j] = 1;
                            AddNumberofNumsOnPillar(true, j);
                        }
                        Debug.Log("Index " + i + "," + j + "= " + codeGrid[i, j] + " going " + (Directions)directionalGrid[i, j]);

                        //Continue to next array index
                        continue;
                    }

                    //If just Left column
                    if (j == 0)
                    {
                        //If Left column, direction goes Right
                        directionalGrid[i, j] = 2;
                        AddNumberofNumsOnPillar(false, i);
                        Debug.Log("Index " + i + "," + j + "= " + codeGrid[i, j] + " going " + (Directions)directionalGrid[i, j]);

                        continue;
                    }

                    //If just Right column
                    if (j == numOfCols - 1)
                    {
                        //If Right column, direction goes Left
                        directionalGrid[i, j] = 4;
                        AddNumberofNumsOnPillar(false, i);
                        Debug.Log("Index " + i + "," + j + "= " + codeGrid[i, j] + " going " + (Directions)directionalGrid[i, j]);

                        continue;
                    }

                    
                    
                }
            }
        }
        
    }


    /// <summary>
    /// Function for adding up the amount of numbers on a single pillar
    /// </summary>
    /// <param name="vertical"></param>
    /// <param name="currentIndex"></param>
    private void AddNumberofNumsOnPillar(bool vertical, int currentIndex)
    {
        //If adding, spread out in a vertical line
        if(vertical)
        {
            //Go through each row
            for (int k = 0; k < numOfRows; k++)
            {
                //Increase the number in this array index
                numOfDirectionGrid[k, currentIndex]++;
            }
        }
        //If adding in a horizontal line
        else
        {
            //Go through each column
            for (int k = 0; k < numOfCols; k++)
            {
                //Increase the number in this array index
                numOfDirectionGrid[currentIndex, k]++;
            }
        }
    }


    /// <summary>
    /// Function to spawn all pillars in correct positions
    /// </summary>
    private void SpawnPillars()
    {
        float startSpawnX = 0;
        float startSpawnZ = 0;

        //Work out the X axis spawn value
        startSpawnX = UsefulFunctions.WorkOutStartSpawnValue(spawnOffset, pillarSpacing, numOfCols, this.transform.position.x);
        startSpawnZ = UsefulFunctions.WorkOutStartSpawnValue(-spawnOffset, -pillarSpacing, numOfRows, this.transform.position.z);

        //Create the startSpawnPoint's offset using the x and z values
        Vector3 startSpawnOffset = new Vector3(startSpawnX, 0, startSpawnZ);

        //Loop through all positions in grid
        for(int i = 0; i < numOfRows; i++)
        {
            for(int j = 0; j < numOfCols; j++)
            {
                //If numbers on pillar is less than or equal to 1
                //Meaning either 0 or 1 number pass through it
                if(numOfDirectionGrid[i, j] <= 1)
                {
                    //Spawn a 2 way pillar
                    GameObject pillarPos = Instantiate(twoWayPillarPrefab, this.transform);
                    //Set the position of the pillar correctly
                    pillarPos.transform.position = startSpawnOffset + new Vector3(pillarSpacing * j, 0, -pillarSpacing * i);
                    //Keep track of the pillar by storing in an array
                    PillarNumScript[i,j] = pillarPos.GetComponent<PillarNumberScript>();

                }
                //If more than 1 number passes through the pillar
                else
                {
                    //Spawn a 4 way pillar
                    GameObject pillarPos = Instantiate(fourWayPillarPrefab, this.transform);
                    pillarPos.transform.position = startSpawnOffset + new Vector3(pillarSpacing * j, 0, -pillarSpacing * i);
                    PillarNumScript[i, j] = pillarPos.GetComponent<PillarNumberScript>();
                }

                //Randomise rotation of pillar so the numbers are not already aligned
                RotatePillar(PillarNumScript[i, j].transform);
            }
        }
    }
    

    /// <summary>
    /// Function rotating the pillar a random amount
    /// </summary>
    /// <param name="pillar"></param>
    private void RotatePillar(Transform pillar)
    {
        //Create a target rotation with random multiple of 90 in Y value
        Quaternion target = Quaternion.Euler(0, Random.Range(0,4) * 90f, 0);
        //Set the pillar's rotation
        pillar.transform.rotation = target;
    }


    /// <summary>
    /// Function to check which directions the numbers must be spread across pillars
    /// </summary>
    private void CheckDirectionForNumber()
    {
        //Loop through the directionalGrid
        for( int i = 0; i < numOfRows; i++)
        {
            for (int j = 0; j < numOfCols; j++)
            {
                //If this index in directionalGrid is not 0
                //There is a number in this pillar
                if (directionalGrid[i, j] != 0)
                {
                    //If number must be seen through north direction or
                    //if number must be seen through south direction
                    if (directionalGrid[i, j] == 1 || directionalGrid[i, j] == 3)
                    {
                        //Spread the number parts across the rows
                        SpreadNumberParts(numOfRows, i, j, true);
                    }
                    //If seen through east or west direction
                    else
                    {
                        //Spread the number parts across the columns
                        SpreadNumberParts(numOfCols, i, j, false);
                    }
                }                
            }
        }
    }


    /// <summary>
    /// Splits a number prefab into multiple parts to be distributed
    /// </summary>
    /// <param name="numPartsPrefab"></param>
    /// <param name="availablePillars"></param>
    public void SplitNum(WholeNumberParts numPartsPrefab, int availablePillars)
    {
        //Clear the previous number of parts in each pillar
        numOfPartsInEachPillar.Clear();
        //Set this number parts list to the chosen number prefab parts
        thisNumberParts = numPartsPrefab.numParts;

        //Shuffle the order of number parts up
        UsefulFunctions.Shuffle(thisNumberParts);
        
        int currentPillar = 0;
        //For i is less than the count of thisNumberParts list
        for(int i = 0; i < thisNumberParts.Count; i++)
        {
            //If the count of pillars is not equal to the number of available pillars in row or column
            if(numOfPartsInEachPillar.Count != availablePillars)
            {
                //Add an instance of 1 to the list
                numOfPartsInEachPillar.Add(1);
            }
            //If the list is as long as the number of available pillars
            else
            {
                //If current pillar value is equal to the amount of available pillars
                if(currentPillar == availablePillars)
                {
                    //Set current pillar back to 0
                    currentPillar = 0;
                }
                //Increase the number of parts on current pillar by 1
                numOfPartsInEachPillar[currentPillar] += 1;
                currentPillar++;
            }
        }

        //Shuffle the order of parts in pillars, so that the first pillar doesn't always
        //contain the most parts
        UsefulFunctions.Shuffle(numOfPartsInEachPillar);

    }


    /// <summary>
    /// Function to loop through and spawn the specified amount of parts of the current number in 
    /// each pillar in the row/column
    /// </summary>
    /// <param name="numOfRowsOrCols"></param>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <param name="vertical"></param>
    private void SpreadNumberParts(int numOfRowsOrCols, int i, int j, bool vertical)
    {
        //Separates number prefab into separate sprites
        //And spread the parts out across pillars in the line
        SplitNum(allWholeNumbers[codeGrid[i, j] - 1], numOfRowsOrCols);

        int numberPart = 0;
        //Loop through the rows
        for (int k = 0; k < numOfRowsOrCols; k++)
        {
            //Loop through number of parts needed in current pillar
            for (int l = 0; l < numOfPartsInEachPillar[k]; l++)
            {
                //If going down the rows
                if(vertical)
                {
                    //Insert the number part sprite into correct view direction on pillar
                    PillarNumScript[k, j].InsertNumberPartSprite(directionalGrid[i, j], l, thisNumberParts[numberPart]);

                }
                //If going across the columns
                else
                {
                    //Insert the number part sprite into correct view direction on pillar
                    PillarNumScript[i, k].InsertNumberPartSprite(directionalGrid[i, j], l, thisNumberParts[numberPart]);

                }
                numberPart++;

            }
        }

        //If on hard difficulty, add order numbers to pillars to remove the chance of the player needing
        //to enter thousands of sequence possibilities from the numbers found in pillars
        if(PuzzleManagement.ChosenDifficulty == PuzzleManagement.Difficulty.HARD)
        {
            //Get the number's position within the code sequence
            int placeInOrder = PillarNumScript[i, j].WorkOutNumberInCodeSequence(codeGrid[i, j]);
            if(placeInOrder == 100)
            {
                Debug.LogError("Number not found in code");
                return;
            }

            //Spawn a sprite on the starting pillar for the number, to state it's order in the code
            PillarNumScript[i, j].SpawnNumberInSequence(placeInOrder);

        }
    }


    /// <summary>
    /// Destroy all pillars at end of level
    /// </summary>
    private void OnDestroy()
    {
        //Loop through all pillars in grid, and destroy them
        for (int i = 0; i < numOfRows; i++)
        {
            for (int j = 0; j < numOfCols; j++)
            {
                Destroy(PillarNumScript[i, j].gameObject);
            }
        }
    }

}
