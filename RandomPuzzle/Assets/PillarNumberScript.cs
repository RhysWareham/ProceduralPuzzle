using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarNumberScript : MonoBehaviour
{
    [SerializeField] private bool isTwoWay;
    [SerializeField] private List<SpriteRenderer> northViewNumList;
    [SerializeField] private List<SpriteRenderer> southViewNumList;
    [SerializeField] private List<SpriteRenderer> eastViewNumList;
    [SerializeField] private List<SpriteRenderer> westViewNumList;
    [SerializeField] private SpriteRenderer numInCode;
    [SerializeField] private List<Sprite> numbers;


    /// <summary>
    /// Spawns the number order sprite on the pillar
    /// </summary>
    /// <param name="numInSequence"></param>
    public void SpawnNumberInSequence(int numInSequence)
    {
        //Spawn a sprite of this number's position in code sequence, on this starting pillar
        numInCode.sprite = numbers[numInSequence];
    }


    /// <summary>
    /// Work out the number's position within the code sequence
    /// </summary>
    /// <param name="thisNumber"></param>
    /// <returns></returns>
    public int WorkOutNumberInCodeSequence(int thisNumber)
    {
        //Loop through all numbers in the code
        for (int i = 0; i < PuzzleManagement.RequiredCode.Count; i++)
        {
            //If this number is equal to the value in current index of the code
            if(thisNumber == PuzzleManagement.RequiredCode[i])
            {
                //If this number is not already in the order list at this index
                if(PuzzleManagement.ShuffledOrder.Count == 0 || 
                    PuzzleManagement.ShuffledOrder[PuzzleManagement.ShuffledOrder.Count-1] != i)
                {
                    //Add the index to the order list
                    //because i is the position that thisNumber is found within the initial code order
                    PuzzleManagement.ShuffledOrder.Add(i);
                    
                    //Return the index
                    return i;
                }
                
            }
        }

        //If number not found in code, return 100
        return 100;
    }


    //public void DetermineHowMuchOfEachNumber(int numOfAvailablePillars, )
    /// <summary>
    /// Function inserts a given number part sprite into the correct sprite renderer based on direction for view
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="pillarNumPartSlot"></param>
    /// <param name="numPart"></param>
    public void InsertNumberPartSprite(int direction, int pillarNumPartSlot, SpriteRenderer numPart)
    {
        //Switch based on direction
        switch(direction)
            {
                //If north view
                case 1:
                    //Set one northview number part sprite renderer to be the numPart sprite 
                    northViewNumList[pillarNumPartSlot].sprite = numPart.sprite;
                    break;    
                
                //If east view
                case 2:       
                    //But if pillar is only twoWay, set the northView sprite, because it
                    //does not contain an east view
                    if(isTwoWay)
                    {
                        northViewNumList[pillarNumPartSlot].sprite = numPart.sprite;
                    }
                    else
                    {
                        eastViewNumList[pillarNumPartSlot].sprite = numPart.sprite;
                    }
                    break;  

                //If south view
                case 3:                   
                    southViewNumList[pillarNumPartSlot].sprite = numPart.sprite;
                    break;   
                
                //If west view
                case 4:
                    //But if pillar is only twoWay, set the south view sprite, because it
                    //does not contain an west view    
                    if (isTwoWay)
                    {
                        southViewNumList[pillarNumPartSlot].sprite = numPart.sprite;
                    }
                    else
                    {
                        westViewNumList[pillarNumPartSlot].sprite = numPart.sprite;
                    }
                        break;
                default:
                    break;
            }
        
        
    }
}
