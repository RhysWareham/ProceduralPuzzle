using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    [SerializeField] private Transform unlockedPos;
    [SerializeField] private Transform lockedPos;
    [SerializeField] private float doorMoveSpeed;
    [SerializeField] private float doorUnlockSpeed = 1f;
    [SerializeField] private float doorLockSpeed = 10f;

    private Coroutine currentCoroutine;


    /// <summary>
    /// Function to unlock the door
    /// </summary>
    public void Unlock()
    {
        //If the coroutine is running, stop it
        if (CheckCoroutineRunning())
        {
            StopCoroutine(currentCoroutine);
            //Set position of door to locked position
            this.transform.position = lockedPos.position;
        }

        //Start coroutine to move the door to unlocked position
        doorMoveSpeed = doorUnlockSpeed;
        currentCoroutine = StartCoroutine(MoveDoor(unlockedPos));
    }

    /// <summary>
    /// Function to lock the door
    /// </summary>
    public void Lock()
    {
        //If the coroutine is running, stop it
        if (CheckCoroutineRunning())
        {
            StopCoroutine(currentCoroutine);
            //Set position of door to unlocked position
            this.transform.position = unlockedPos.position;
        }

        //Start coroutine to move the door to locked position
        doorMoveSpeed = doorLockSpeed;
        currentCoroutine = StartCoroutine(MoveDoor(lockedPos));
    }

    /// <summary>
    /// Function checking if coroutine is running
    /// </summary>
    /// <returns></returns>
    private bool CheckCoroutineRunning()
    {
        if(currentCoroutine != null)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Coroutine to move door to given position
    /// </summary>
    /// <param name="targetTransform"></param>
    /// <returns></returns>
    private IEnumerator MoveDoor(Transform targetTransform)
    {
        //Set the target position
        Vector3 targetPos = targetTransform.position;

        //While the door is not at the target position, move the door
        while (this.transform.position != targetPos)
        {
            this.transform.position = Vector3.Slerp(this.transform.position, targetPos, Time.deltaTime * doorMoveSpeed);
            yield return 0;
        }
    }
}
