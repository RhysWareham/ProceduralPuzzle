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
    //[SerializeField] private Transform quad;

    //private List<Sprite> numParts = new List<Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnNumberInSequence(int numInSequence)
    {
        numInCode.sprite = numbers[numInSequence];
    }

    public int WorkOutNumberInCodeSequence(int thisNumber)
    {
        for (int i = 0; i < PuzzleManagement.RequiredCode.Count; i++)
        {
            if(thisNumber == PuzzleManagement.RequiredCode[i])
            {
                
                //If the number is already in the order, continue for loop to increase index
                if(PuzzleManagement.ShuffledOrder.Count == 0 || PuzzleManagement.ShuffledOrder[PuzzleManagement.ShuffledOrder.Count-1] != i)
                {
                    PuzzleManagement.ShuffledOrder.Add(i); //number 5 is the first in Order
                    return i;
                }
                
            }
        }
        return 100;
    }

    //public void DetermineHowMuchOfEachNumber(int numOfAvailablePillars, )
    public void InsertNumberSprite(int direction, int pillarNumSlot, SpriteRenderer numPart)
    {

        switch(direction)
            {
                case 1:
                    northViewNumList[pillarNumSlot].sprite = numPart.sprite;
                    break;                
                case 2:                   
                    if(isTwoWay)
                    {
                        northViewNumList[pillarNumSlot].sprite = numPart.sprite;
                    }
                    else
                    {
                        eastViewNumList[pillarNumSlot].sprite = numPart.sprite;
                    }
                    break;                
                case 3:                   
                    southViewNumList[pillarNumSlot].sprite = numPart.sprite;
                    break;                
                case 4:
                    if (isTwoWay)
                    {
                        southViewNumList[pillarNumSlot].sprite = numPart.sprite;
                    }
                    else
                    {
                        westViewNumList[pillarNumSlot].sprite = numPart.sprite;
                    }
                        break;
                default:
                    break;
             

            }
        //Texture tex = numPart.texture;
        //quad.GetComponent<MeshRenderer>().material.SetTexture("NumberTexture", numPart.texture);

        
    }
}
