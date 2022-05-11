using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarNumberScript : MonoBehaviour
{
    [SerializeField] private bool isTwoWay;
    [SerializeField] private SpriteRenderer northViewNum;
    [SerializeField] private SpriteRenderer southViewNum;
    [SerializeField] private SpriteRenderer eastViewNum;
    [SerializeField] private SpriteRenderer westViewNum;
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
    public void InsertNumberSprite(int direction, int numOfAvailablePillars, SpriteRenderer numPart)
    {

        switch(direction)
            {
                case 1:
                    northViewNum.sprite = numPart.sprite;
                    break;                
                case 2:                   
                    if(isTwoWay)
                    {
                        northViewNum.sprite = numPart.sprite;
                    }
                    else
                    {
                        eastViewNum.sprite = numPart.sprite;
                    }
                    break;                
                case 3:                   
                    southViewNum.sprite = numPart.sprite;
                    break;                
                case 4:
                    if (isTwoWay)
                    {
                        southViewNum.sprite = numPart.sprite;
                    }
                    else
                    {
                        westViewNum.sprite = numPart.sprite;
                    }
                        break;
                default:
                    break;
             

            }
        //Texture tex = numPart.texture;
        //quad.GetComponent<MeshRenderer>().material.SetTexture("NumberTexture", numPart.texture);

        
    }
}
