using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private GameObject menu;

    [SerializeField] private InputActionReference menuActionReference;
    private InputAction MenuButton => menuActionReference ? menuActionReference.action : null;

    // Start is called before the first frame update
    void Start()
    {
        MenuButton.performed += OnMenuButtonPressed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Function to set the menu's activeness when menu button is pressed
    /// </summary>
    /// <param name="obj"></param>
    void OnMenuButtonPressed(InputAction.CallbackContext obj)
    {
        menu.SetActive(!menu.activeSelf);
    }

    /// <summary>
    /// Function to close menu
    /// </summary>
    public void CloseMenu()
    {
        menu.SetActive(false);
    }

    
    /// <summary>
    /// Function to terminate the application
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
