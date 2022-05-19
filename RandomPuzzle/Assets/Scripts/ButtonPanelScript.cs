using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonPanelScript : MonoBehaviour
{
    [SerializeField] private List<Transform> buttonPositions = new List<Transform>();
    [SerializeField] private Transform selectedButton;
    [SerializeField] private Transform okButton;
    private int currentButtonPos = 0;
    private bool InRange;
    [SerializeField] private CodeBarScript codeBar;

    [SerializeField] private InputActionReference selectActionReference;
    private InputAction SelectActionButton => selectActionReference ? selectActionReference.action : null;

    [SerializeField] private InputActionReference arrowKeyActionReference;
    private InputAction ArrowKeyActionButton => arrowKeyActionReference ? arrowKeyActionReference.action : null;


    private void Start()
    {
        SelectActionButton.performed += SelectActionButton_performed;
        ArrowKeyActionButton.performed += ArrowKeyActionButton_performed; 
    }


    /// <summary>
    /// Function for when arrow keys are entered when using the code panel
    /// </summary>
    /// <param name="obj"></param>
    private void ArrowKeyActionButton_performed(InputAction.CallbackContext obj)
    {
        //If in range
        if (InRange)
        {
            //If the codebar is not filled
            if (!codeBar.codeFilled)
            {
                //Get the inputted value
                Vector2 direction = obj.ReadValue<Vector2>();

                //If up arrow is pressed
                if (direction.y == 1)
                {
                    //If current pos is more than 2
                    if (currentButtonPos > 2)
                    {
                        //Move to number on row above
                        selectedButton.position = buttonPositions[currentButtonPos - 3].position;
                        currentButtonPos -= 3;
                    }
                }
                //If down arrow is pressed
                if (direction.y == -1)
                {
                    //If not on bottom row
                    if (currentButtonPos < 6)
                    {
                        //Move directly to row below
                        selectedButton.position = buttonPositions[currentButtonPos + 3].position;
                        currentButtonPos += 3;
                    }
                }
                //If right arrow is pressed
                if (direction.x == 1)
                {
                    //If not at end of panel
                    if (currentButtonPos != 8)
                    {
                        //Move to next button along
                        selectedButton.position = buttonPositions[currentButtonPos + 1].position;
                        currentButtonPos++;
                    }
                }
                //If left arrow is pressed
                if (direction.x == -1)
                {
                    //If not at the first button
                    if (currentButtonPos != 0)
                    {
                        //Move to button to the left
                        selectedButton.position = buttonPositions[currentButtonPos - 1].position;
                        currentButtonPos--;
                    }
                }

                //If codebar is filled
                if (codeBar.codeFilled)
                {
                    //Move selected position to the OK button
                    selectedButton.position = okButton.position;
                }
            }
        }
    }

    /// <summary>
    /// Function for when select button has been pressed
    /// </summary>
    /// <param name="obj"></param>
    private void SelectActionButton_performed(InputAction.CallbackContext obj)
    {
        //If in range
        if(InRange)
        {
            //If codebar is not filled
            if(!codeBar.codeFilled)
            {
                //Enter the chosen number
                codeBar.EnterButtonNumber(buttonPositions[currentButtonPos].GetComponent<SpriteRenderer>().sprite, currentButtonPos + 1);
            }
            //If code bar is filled
            else
            {
                //Call the check code function
                codeBar.CheckCode();
                //Move selectedButton position back to 1
                selectedButton.position = buttonPositions[0].position;
                currentButtonPos = 0;
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
