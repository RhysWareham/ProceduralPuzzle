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

    [SerializeField] private int maxNumParts = 7;
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
        CheckDirectionForNumber();
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
    /// Function for adding the amount of numbers on a single pillar
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

        //If numOfCols is even
        if(numOfCols % 2 == 0)
        {
            int halfCountCols = numOfCols / 2;

            //If half Cols is not 1
            if(halfCountCols != 1)
            {
                startSpawnX = this.transform.position.x - spawnOffset - (pillarSpacing * (halfCountCols - 1));
            }
            else
            {
                startSpawnX = this.transform.position.x - spawnOffset;
            }
        }
        else
        {
            int halfCountCols = (numOfCols - 1) / 2;
            startSpawnX = this.transform.position.x - (pillarSpacing * (halfCountCols));
        }


        //If numOfRows is even
        if (numOfRows % 2 == 0)
        {
            int halfCountRows = numOfRows / 2;
            //If half Rows is not 1
            if (halfCountRows != 1)
            {
                startSpawnZ = this.transform.position.z + spawnOffset + (pillarSpacing * (halfCountRows - 1));
            }
            else
            {
                startSpawnZ = this.transform.position.z + spawnOffset;
            }
        }
        //If odd
        else
        {
            int halfCountRows = (numOfRows - 1) / 2;
            startSpawnZ = this.transform.position.z + (pillarSpacing * (halfCountRows));
        }

        Vector3 startSpawnOffset = new Vector3(startSpawnX, 0, startSpawnZ);

        for(int i = 0; i < numOfRows; i++)
        {
            for(int j = 0; j < numOfCols; j++)
            {
                //If numbers on pillar is less than or equal to 1
                ///This doesn't take into account if the other direction is at a 90 degree angle, 
                ///so changed it to if atleast 2 directions then it will be four way
                ///Could just check if vertical or horizontal to add?
                if(numOfDirectionGrid[i, j] <= 1)
                {
                    //Spawn a 2 way pillar
                    GameObject pillarPos = Instantiate(twoWayPillarPrefab, this.transform);
                    pillarPos.transform.position = startSpawnOffset + new Vector3(pillarSpacing * j, 0, -pillarSpacing * i);
                    PillarNumScript[i,j] = pillarPos.GetComponent<PillarNumberScript>();

                }
                else
                {
                    //Spawn a 4 way pillar
                    GameObject pillarPos = Instantiate(fourWayPillarPrefab, this.transform);
                    pillarPos.transform.position = startSpawnOffset + new Vector3(pillarSpacing * j, 0, -pillarSpacing * i);
                    PillarNumScript[i, j] = pillarPos.GetComponent<PillarNumberScript>();
                }

                //Randomise rotation of pillar
                RotatePillar(PillarNumScript[i, j].transform);
            }
        }
        //Vector3 startSpawnPoint = this.transform.position + 
        //GameObject pillarPos = Instantiate(twoWayPillarPrefab, this.transform);
        //pillarPos.transform.position = startSpawnOffset;
    }
    
    private void RotatePillar(Transform pillar)
    {
        Quaternion target = Quaternion.Euler(0, Random.Range(0,4) * 90f, 0);
        pillar.transform.rotation = target;
    }

    public void SplitNum(WholeNumberParts numPartsPrefab, int availablePillars)
    {
        numOfPartsInEachPillar.Clear();
        thisNumberParts = numPartsPrefab.numParts;


        //Shuffle the parts up here
        UsefulFunctions.Shuffle(thisNumberParts);
        

        //int availablePillars = numOfRows;
        //List<SpriteRenderer> thisPillarNumParts = new List<SpriteRenderer> ();
        //List<int> numOfPartsInEachPillar = new List<int>();
        int j = 0;
        for(int i = 0; i < thisNumberParts.Count; i++)
        {
            if(numOfPartsInEachPillar.Count != availablePillars)
            {
                numOfPartsInEachPillar.Add(1);
            }
            else
            {
                if(j == availablePillars)
                {
                    j = 0;
                }
                numOfPartsInEachPillar[j] += 1;
                j++;
            }
        }
        UsefulFunctions.Shuffle(numOfPartsInEachPillar);

    }


    public void DetermineHowMuchOfEachNumber(int numOfAvailablePillars )
    {

    }
    private void CheckDirectionForNumber()
    {
        //int idk = 0;
        for( int i = 0; i < numOfRows; i++)
        {
            for (int j = 0; j < numOfCols; j++)
            {
                if (directionalGrid[i, j] != 0)
                {
                    //idk++;
                    //if(idk == PuzzleManagement.RequiredCode.Count)
                    //{
                    //    Debug.Log("Last number");
                    //}

                    //If going down the rows with number facing north
                    //If number must be seen through south direction
                    if (directionalGrid[i, j] == 1 || directionalGrid[i, j] == 3)
                    {
                        //Separates number prefab into separate sprites
                        ///Next will split into even amounts
                        SplitNum(allWholeNumbers[codeGrid[i, j] - 1], numOfRows);
                        int numberPart = 0;
                        for (int k = 0; k < numOfRows; k++)
                        {
                            for (int l = 0; l < numOfPartsInEachPillar[k]; l++)
                            {
                                PillarNumScript[k, j].InsertNumberSprite(directionalGrid[i, j], l, thisNumberParts[numberPart]);
                                numberPart++;

                            }
                            //PillarNumScript[k, j].InsertNumberSprite(directionalGrid[i, j], numOfRows, thisNumberParts[k]);

                        }
                        int placeInOrder = PillarNumScript[i, j].WorkOutNumberInCodeSequence(codeGrid[i, j]);
                        PillarNumScript[i, j].SpawnNumberInSequence(placeInOrder);
                    }
                    else
                    {
                        //Separates number prefab into separate sprites
                        ///Next will split into even amounts
                        SplitNum(allWholeNumbers[codeGrid[i, j] - 1], numOfCols);
                        int numberPart = 0;
                        for (int k = 0; k < numOfCols; k++)
                        {
                            for (int l = 0; l < numOfPartsInEachPillar[k]; l++)
                            {
                                PillarNumScript[i, k].InsertNumberSprite(directionalGrid[i, j], l, thisNumberParts[numberPart]);
                                numberPart++;

                            }
                        }
                        int placeInOrder = PillarNumScript[i, j].WorkOutNumberInCodeSequence(codeGrid[i, j]);
                        PillarNumScript[i, j].SpawnNumberInSequence(placeInOrder);
                    }
                }
            
                
            }
        }
        AttachNumClues();
    }

    void AttachNumClues()
    {
        for(int i = 0; i < numOfRows; i++)
        {
            for(int j = 0; j < numOfCols; j++)
            {

            }
        }
    }

    //private int WorkOutNumberInCodeSequence(int pillarNumber)
    //    {
    //        int thisSequencePlace;
    //    }

    // Start is called before the first frame update

}
