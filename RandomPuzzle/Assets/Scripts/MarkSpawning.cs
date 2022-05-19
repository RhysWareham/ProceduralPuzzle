using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkSpawning : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> markSpawnPoints = new List<SpriteRenderer>();
    [SerializeField] private List<Sprite> marks;


    /// <summary>
    /// Function spawning number of marks to show what position in code sequence it is
    /// </summary>
    /// <param name="numOfMarks"></param>
    public void SpawnMarks(int numOfMarks)
    {
        //Shuffle the list of spawn points
        UsefulFunctions.Shuffle(markSpawnPoints);

        //Loop through amount of marks needed
        for(int i = 0; i < numOfMarks + 1; i++)
        {
            //Randomly select the mark sprite
            markSpawnPoints[i].sprite = marks[Random.Range(0, marks.Count)];
        }
    }


}
