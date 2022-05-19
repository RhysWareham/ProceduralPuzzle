using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ViewPointScript : MonoBehaviour
{
    private bool InRange;
    private Camera holeCamera;
    private Camera playerCamera;
    private bool cameraView = false;
    private Vector3 playerPos;

    [SerializeField] private InputActionReference pillarViewActionReference;
    private InputAction PillarViewActionButton => pillarViewActionReference ? pillarViewActionReference.action : null;
        

    // Start is called before the first frame update
    void Start()
    {
        holeCamera = GetComponent<Camera>();

        PillarViewActionButton.performed += PillarViewActionButton_performed;

    }


    /// <summary>
    /// Function to look through or exit pillar view port 
    /// </summary>
    /// <param name="obj"></param>
    private void PillarViewActionButton_performed(InputAction.CallbackContext obj)
    {
        //If in range
        if(InRange)
        {
            //If not in hole view
            if (!cameraView)
            {
                //Enable hole camera and disable player camera
                holeCamera.enabled = true;
                playerCamera.enabled = false;
                cameraView = true;
                playerPos = playerCamera.transform.parent.position;
            }
            else
            {
                //Enable player camera and disable hole camera
                playerCamera.enabled = true;
                holeCamera.enabled = false;
                cameraView = false;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (InRange)
        {
            //If in hole view
            if(cameraView)
            {
                //If the player moves
                if(playerPos != playerCamera.transform.parent.position)
                {
                    //Change back to player camera
                    playerCamera.enabled = true;
                    holeCamera.enabled = false;
                    cameraView = false;
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        InRange = true;
        playerCamera = other.GetComponent<PlayerController>().playerCamera;
    }

    private void OnTriggerExit(Collider other)
    {
        InRange = false;
    }
}
