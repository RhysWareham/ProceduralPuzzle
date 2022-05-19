using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportControl : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] float teleportDistance = 84.69f;
    [SerializeField] DoorControl exitDoor;
    [SerializeField] DoorControl backDoor;
    [SerializeField] PuzzleManager puzzleManager;


    /// <summary>
    /// Lock the exit door on entering the trigger
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //Lock the exit door
        exitDoor.Lock();
        
    }

    /// <summary>
    /// On exit, teleport player to starting corridor and reset the puzzle room
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        //Disable the character controller in order to teleport player, and then reenable
        CharacterController cc = other.GetComponent<CharacterController>();
        cc.enabled = false;
        other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z - teleportDistance);
        cc.enabled = true;

        //Call to restart the level generation
        puzzleManager.ResetPuzzle();
        //Unlock the backdoor
        backDoor.Unlock();
    }
}
