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

    //public void SplitNum(GameObject numPartsPrefab)
    //{
    //    SpriteRenderer[] numPartsArray = numPartsPrefab.GetComponentsInChildren<SpriteRenderer>();
    //    for (int i = 0; i < numPartsArray.Length; i++)
    //    {
    //        numParts.Add(numPartsArray[i].sprite);
    //    }
    //}

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
