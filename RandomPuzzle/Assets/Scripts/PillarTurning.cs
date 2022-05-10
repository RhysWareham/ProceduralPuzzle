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
        if(InRange)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if(turningCoroutine != null)
                {
                    //moveablePillar.transform.Rotate(0f, -90f, 0f);
                    StopCoroutine(turningCoroutine);
                    Quaternion target = Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z);
                    moveablePillar.transform.rotation = target;
                }

                turningCoroutine = StartCoroutine(RotatePillar(-1));
                
                
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(turningCoroutine != null)
                {
                    //moveablePillar.transform.Rotate(0f, 90f, 0f);
                    StopCoroutine(turningCoroutine);
                    Quaternion target = Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z);
                    moveablePillar.transform.rotation = target;

                }
                
                turningCoroutine = StartCoroutine(RotatePillar(1));
                
            }
        }
    }

    private IEnumerator RotatePillar(int direction)
    {
        targetRotation = new Vector3(moveablePillar.transform.eulerAngles.x, moveablePillar.transform.eulerAngles.y + (90 * direction), moveablePillar.transform.eulerAngles.z);

        while(moveablePillar.transform.eulerAngles != targetRotation)
        {
            moveablePillar.transform.rotation = Quaternion.Slerp(moveablePillar.transform.rotation, Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z), Time.deltaTime * pillarRotateSpeed);
            yield return 0;
        }
        //turningCoroutine = null;
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
