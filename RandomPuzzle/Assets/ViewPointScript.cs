using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPointScript : MonoBehaviour
{
    private bool InRange;
    private Camera holeCamera;
    private Camera playerCamera;
    private bool cameraView = false;

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
            if (Input.GetKeyDown(KeyCode.F))
            {
                if(!cameraView)
                {
                    holeCamera.enabled = true;
                    playerCamera.enabled = false;
                    cameraView = true;
                }
                else
                {
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
