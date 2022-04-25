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

    [SerializeField] private GameObject emptyFieldPrefab;
    [SerializeField] private Transform slotSpawnPoint;
    private float slotOffset = 0.065f;
    private Vector3 slotSpacing = new Vector3(0.13f,0,0);

    // Start is called before the first frame update
    void Start()
    {
        emptyField = EnteredSlots[0].GetComponent<SpriteRenderer>().sprite;
    }

    private void Awake()
    {
        EnteredSlots.Clear();
        float startSpawnPointX = 0;
        //If even number of numbers needed
        if (PuzzleManagement.RequiredCode.Count % 2 == 0)
        {
            int halfCount = PuzzleManagement.RequiredCode.Count / 2;
            if (halfCount != 1)
            {
                startSpawnPointX = slotSpawnPoint.position.x - slotOffset - (slotSpacing.x * halfCount);
            }
            else
            {
                startSpawnPointX = slotSpawnPoint.position.x - slotOffset;
            }

        }
        else if (PuzzleManagement.RequiredCode.Count != 1)
        {
            
            int halfCount = (PuzzleManagement.RequiredCode.Count - 1) / 2;
            //float startSpawnPointX;
            startSpawnPointX = slotSpawnPoint.position.x - (slotSpacing.x * halfCount);
        }

        Vector3 spawnPointOffset = Vector3.zero;
        spawnPointOffset.x = startSpawnPointX;
        Vector3 spawnPoint = slotSpawnPoint.position + spawnPointOffset;
        spawnPoint = new Vector3(spawnPoint.x, 0, -0.6f);

        for (int i = 0; i < PuzzleManagement.RequiredCode.Count; i++)
        {
            
            GameObject slotPos = Instantiate(emptyFieldPrefab, slotSpawnPoint.position, Quaternion.identity);
            slotPos.transform.SetParent(this.transform);
            slotPos.transform.localPosition = spawnPoint + (slotSpacing * i);
            
            EnteredSlots.Add(slotPos.transform);
        }
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
