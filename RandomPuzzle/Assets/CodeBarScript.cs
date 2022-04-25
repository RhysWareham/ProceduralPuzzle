using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeBarScript : MonoBehaviour
{
    [SerializeField] private List<Transform> EnteredSlots = new List<Transform>();
    private Sprite emptyField;
    private List<int> EnteredCode = new List<int>();
    private int numOfNumbersEntered = 0;
    public bool codeFilled = false;

    

    // Start is called before the first frame update
    void Start()
    {
        emptyField = EnteredSlots[0].GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterButtonNumber(Sprite enterredNumberSprite, int enterredNumber)
    {
        EnteredCode.Add(enterredNumber);
        EnteredSlots[numOfNumbersEntered].GetComponent<SpriteRenderer>().sprite = enterredNumberSprite;
        numOfNumbersEntered++;
        if (numOfNumbersEntered == EnteredSlots.Count)
        {
            codeFilled = true;
        }
    }

    public void CheckCode()
    {
        for(int i = 0; i < numOfNumbersEntered; i++)
        {
            if (EnteredCode[i] == PuzzleManagement.RequiredCode[i])
            {
                EnteredSlots[i].GetComponent<SpriteRenderer>().color = Color.green;
            }
            else
            {
                ResetCodeBar();
                return;
            }
        }
        
    }

    public void ResetCodeBar()
    {
        for (int i = 0; i < numOfNumbersEntered; i++)
        {
            EnteredSlots[i].GetComponent<SpriteRenderer>().sprite = emptyField;
        }
        numOfNumbersEntered = 0;
        EnteredCode.Clear();
        codeFilled = false;
    }
}
