using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPanelScript : MonoBehaviour
{
    [SerializeField] private List<Transform> buttonPositions = new List<Transform>();
    [SerializeField] private Transform selectedButton;
    [SerializeField] private Transform okButton;
    private int currentButtonPos = 0;
    private bool InRange;
    [SerializeField] private CodeBarScript codeBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(InRange)
        {
            if (!codeBar.codeFilled)
            {

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (currentButtonPos > 2)
                    {
                        selectedButton.position = buttonPositions[currentButtonPos - 3].position;
                        currentButtonPos -= 3;
                    }
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (currentButtonPos < 6)
                    {
                        selectedButton.position = buttonPositions[currentButtonPos + 3].position;
                        currentButtonPos += 3;
                    }
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (currentButtonPos != 8)
                    {
                        selectedButton.position = buttonPositions[currentButtonPos + 1].position;
                        currentButtonPos++;
                    }
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (currentButtonPos != 0)
                    {
                        selectedButton.position = buttonPositions[currentButtonPos - 1].position;
                        currentButtonPos--;
                    }
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    codeBar.EnterButtonNumber(buttonPositions[currentButtonPos].GetComponent<SpriteRenderer>().sprite, currentButtonPos+1);
                }

                if(codeBar.codeFilled)
                {
                    selectedButton.position = okButton.position;
                }
            }
            else
            {
                if(Input.GetKeyDown(KeyCode.Return))
                {
                    codeBar.CheckCode();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        InRange = false;
    }
}
