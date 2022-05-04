using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarTurning : MonoBehaviour
{
    private bool InRange;
    private GameObject moveablePillar;

    // Start is called before the first frame update
    void Start()
    {
        moveablePillar = GetComponentInChildren<Moveable>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(InRange)
        {
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
    }

    private void OnTriggerExit(Collider other)
    {
        InRange = false;
    }


}
