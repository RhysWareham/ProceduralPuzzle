using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;
    private PlayerInput inputHandler;
    private 

    // Start is called before the first frame update
    void Start()
    {
        inputHandler = GetComponent<PlayerInput>();
        playerCamera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
