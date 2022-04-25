using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarTurning : MonoBehaviour
{
    private bool InRange;
    private Camera holeCamera;
    private Camera playerCamera;
    private GameObject moveablePillar;

    // Start is called before the first frame update
    void Start()
    {
        holeCamera = GetComponentInChildren<Camera>();
        moveablePillar = GetComponentInChildren<Moveable>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(InRange)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                holeCamera.enabled = true;
                playerCamera.enabled = false;
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                moveablePillar.transform.Rotate(0f, -90f, 0f);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                moveablePillar.transform.Rotate(0f, 90f, 0f);
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
        other.GetComponent<PlayerController>().playerCamera.enabled = true;
        holeCamera.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {

    }
}
