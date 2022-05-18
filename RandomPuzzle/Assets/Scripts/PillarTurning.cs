using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarTurning : MonoBehaviour
{
    private bool InRange;
    private GameObject moveablePillar;
    private float pillarRotateSpeed = 5f;
    private Coroutine turningCoroutine;

    private Vector3 targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        moveablePillar = GetComponentInChildren<Moveable>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //If in range
        if(InRange)
        {
            //If Q is pressed
            if (Input.GetKeyDown(KeyCode.Q))
            {
                //If the coroutine is running
                if(turningCoroutine != null)
                {
                    //Stop coroutine
                    StopCoroutine(turningCoroutine);
                    //Set target rotation and apply it to the pillar
                    Quaternion target = Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z);
                    moveablePillar.transform.rotation = target;
                }

                //Start coroutine to rotate pillar
                turningCoroutine = StartCoroutine(RotatePillar(-1));
            }
            //If E is pressed
            if (Input.GetKeyDown(KeyCode.E))
            {
                //If coroutine is running
                if(turningCoroutine != null)
                {
                    //Stop coroutine
                    StopCoroutine(turningCoroutine);
                    //Set target rotation and apply it to the pillar
                    Quaternion target = Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z);
                    moveablePillar.transform.rotation = target;
                }

                //Start coroutine to rotate pillar
                turningCoroutine = StartCoroutine(RotatePillar(1));
                
            }
        }
    }

    /// <summary>
    /// Coroutine to smoothly rotate the pillar to correct rotation
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private IEnumerator RotatePillar(int direction)
    {
        //Set the target rotation
        targetRotation = new Vector3(moveablePillar.transform.eulerAngles.x, moveablePillar.transform.eulerAngles.y + (90 * direction), moveablePillar.transform.eulerAngles.z);

        //While pillar is not at target rotation, rotate it slowly
        while(moveablePillar.transform.eulerAngles != targetRotation)
        {
            moveablePillar.transform.rotation = Quaternion.Slerp(moveablePillar.transform.rotation, Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z), Time.deltaTime * pillarRotateSpeed);
            yield return 0;
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
