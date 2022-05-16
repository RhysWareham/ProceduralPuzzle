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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Unlock()
    {
        if (CheckCoroutineRunning())
        {
            StopCoroutine(currentCoroutine);
            this.transform.position = lockedPos.position;
        }
        doorMoveSpeed = doorUnlockSpeed;
        currentCoroutine = StartCoroutine(MoveDoor(unlockedPos));
    }

    public void Lock()
    {
        if(CheckCoroutineRunning())
        {
            StopCoroutine(currentCoroutine);
            this.transform.position = unlockedPos.position;
        }
        doorMoveSpeed = doorLockSpeed;
        currentCoroutine = StartCoroutine(MoveDoor(lockedPos));
    }

    private bool CheckCoroutineRunning()
    {
        if(currentCoroutine != null)
        {
            return true;
        }
        return false;
    }

    private IEnumerator MoveDoor(Transform targetTransform)
    {

        Vector3 targetPos = targetTransform.position;

        while (this.transform.position != targetPos)
        {
            this.transform.position = Vector3.Slerp(this.transform.position, targetPos, Time.deltaTime * doorMoveSpeed);
            yield return 0;
        }
    }
}
