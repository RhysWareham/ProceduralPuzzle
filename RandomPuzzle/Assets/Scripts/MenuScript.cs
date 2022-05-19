using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private Timer timer;

    [SerializeField] private InputActionReference menuActionReference;
    private InputAction MenuButton => menuActionReference ? menuActionReference.action : null;

    // Start is called before the first frame update
    void Start()
    {
        MenuButton.performed += OnMenuButtonPressed;
        CloseMenu();
    }


    /// <summary>
    /// Function to set the menu's activeness when menu button is pressed
    /// </summary>
    /// <param name="obj"></param>
    void OnMenuButtonPressed(InputAction.CallbackContext obj)
    {
        menu.SetActive(!menu.activeSelf);
        //If menu is open, stop timer
        if(menu.activeSelf)
        {
            timer.StopTimer();
        }
        else
        {
            //If puzzle is not complete, continue timer
            if (!PuzzleManagement.PuzzleComplete)
            {
                //Continue timer
                timer.StartTimer();

            }
        }
    }

    /// <summary>
    /// Function to close menu
    /// </summary>
    public void CloseMenu()
    {
        menu.SetActive(false);
        //If puzzle is not complete
        if(!PuzzleManagement.PuzzleComplete)
        {
            //Continue timer
            timer.StartTimer();

        }
    }

    
    /// <summary>
    /// Function to terminate the application
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
