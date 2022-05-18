using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPointScript : MonoBehaviour
{
    private bool InRange;
    private Camera holeCamera;
    private Camera playerCamera;
    private bool cameraView = false;
    private Vector3 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        holeCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (InRange)
        {
            //If F is pressed
            if (Input.GetKeyDown(KeyCode.F))
            {
                //If not in hole view
                if(!cameraView)
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
