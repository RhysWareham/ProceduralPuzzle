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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        exitDoor.Lock();
        
    }

    private void OnTriggerExit(Collider other)
    {
        CharacterController cc = other.GetComponent<CharacterController>();
        cc.enabled = false;
        other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z - teleportDistance);
        cc.enabled = true;
        //Call to restart the level generation
        puzzleManager.Restart();
        //Unlock the backdoor
        backDoor.Unlock();
    }
}
