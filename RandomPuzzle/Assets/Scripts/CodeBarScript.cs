using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeBarScript : MonoBehaviour
{
    [SerializeField] private List<Transform> enterableSlots = new List<Transform>();
    private Sprite emptyField;
    private List<int> EnteredCode = new List<int>();
    private int numOfNumbersEntered = 0;
    public bool codeFilled = false;

    [SerializeField] private GameObject emptyFieldPrefab;
    [SerializeField] private Transform slotSpawnPoint;
    private float slotOffset = 0.065f;
    private Vector3 slotSpacing = new Vector3(0.13f,0,0);

    private PuzzleManager puzzleManager;

    // Start is called before the first frame update
    void Start()
    {
        puzzleManager = FindObjectOfType<PuzzleManager>();
        emptyField = enterableSlots[0].GetComponent<SpriteRenderer>().sprite;
    }

    private void Awake()
    {
        SpawnCodeBarSlots();
    }

    public void RestartCodeBar()
    {
        foreach(Transform t in enterableSlots)
        {
            Destroy(t.gameObject);
        }
        EnteredCode.Clear();
        codeFilled = false;
        numOfNumbersEntered = 0;
    }

    public void SpawnCodeBarSlots()
    {
        enterableSlots.Clear();
        float startSpawnPointX = 0;
        //If even number of numbers needed
        if (PuzzleManagement.RequiredCode.Count % 2 == 0)
        {
            int halfCount = PuzzleManagement.RequiredCode.Count / 2;
            if (halfCount != 1)
            {
                startSpawnPointX = slotSpawnPoint.localPosition.x - slotOffset - (slotSpacing.x * (halfCount - 1));
            }
            else
            {
                startSpawnPointX = slotSpawnPoint.localPosition.x - slotOffset;
            }

        }
        else if (PuzzleManagement.RequiredCode.Count != 1)
        {

            int halfCount = (PuzzleManagement.RequiredCode.Count - 1) / 2;
            //float startSpawnPointX;
            startSpawnPointX = slotSpawnPoint.localPosition.x - (slotSpacing.x * halfCount);
        }

        Vector3 spawnPointOffset = Vector3.zero;
        spawnPointOffset.x = startSpawnPointX;
        Vector3 spawnPoint = slotSpawnPoint.localPosition + spawnPointOffset;
        spawnPoint = new Vector3(spawnPoint.x, 0, -0.6f);

        for (int i = 0; i < PuzzleManagement.RequiredCode.Count; i++)
        {

            GameObject slotPos = Instantiate(emptyFieldPrefab, slotSpawnPoint.position, Quaternion.identity);
            slotPos.transform.SetParent(this.transform);
            slotPos.transform.localPosition = spawnPoint + (slotSpacing * i);

            enterableSlots.Add(slotPos.transform);
        }
    }

    public void EnterButtonNumber(Sprite enterredNumberSprite, int enterredNumber)
    {
        EnteredCode.Add(enterredNumber);
        enterableSlots[numOfNumbersEntered].GetComponent<SpriteRenderer>().sprite = enterredNumberSprite;
        numOfNumbersEntered++;
        if (numOfNumbersEntered == enterableSlots.Count)
        {
            codeFilled = true;
        }
    }

    public void CheckCode()
    {
        //puzzleManager.ActivateDoor();
        ////If code is correct, unlock the door
        //if(EnteredCode == PuzzleManagement.RequiredCode)
        //{
        //}

        //Could use this to show players which numbers they have got right in order so far
        for(int i = 0; i < PuzzleManagement.RequiredCode.Count; i++)
        {
            if (EnteredCode[i] == PuzzleManagement.RequiredCode[i])
            {
                enterableSlots[i].GetComponent<SpriteRenderer>().color = Color.green;
                if(i == PuzzleManagement.RequiredCode.Count-1)
                {
                    puzzleManager.ActivateDoor();
                }
            }
            else
            {
                ResetCodeBarOnFailure();
                return;
            }
        }
        
    }

    public void ResetCodeBarOnFailure()
    {
        StartCoroutine(FlashColour(Color.red));

        numOfNumbersEntered = 0;
        EnteredCode.Clear();
        codeFilled = false;
    }

    public IEnumerator FlashColour(Color failColor)
    {
        for (int i = 0; i < PuzzleManagement.RequiredCode.Count; i++)
        {
            if(PuzzleManagement.ChosenDifficulty == PuzzleManagement.Difficulty.EASY)
            {
                //If the number is not correct in the order, set to red
                if(enterableSlots[i].GetComponent<SpriteRenderer>().color != Color.green)
                {
                    enterableSlots[i].GetComponent<SpriteRenderer>().color = failColor;

                }
            }
            else
            {
                enterableSlots[i].GetComponent<SpriteRenderer>().color = failColor;

            }
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < PuzzleManagement.RequiredCode.Count; i++)
        {
            enterableSlots[i].GetComponent<SpriteRenderer>().sprite = emptyField;
            enterableSlots[i].GetComponent<SpriteRenderer>().color = Color.white;
        }
        yield return 0;
    }
}
