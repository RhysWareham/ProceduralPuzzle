using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public static class UsefulFunctions
{
    //https://stackoverflow.com/questions/69115335/cannot-create-an-instance-of-the-static-class-random
    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rand = new System.Random();

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }


    /// <summary>
    /// Function to return the starting spawn value
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="spacing"></param>
    /// <param name="numOfValues"></param>
    /// <param name="thisPosValue"></param>
    /// <returns></returns>
    public static float WorkOutStartSpawnValue(float offset, float spacing, int numOfValues, float thisPosValue)
    {
        //If number of values is even
        if (numOfValues % 2 == 0)
        {
            //Get the half count
            int halfCount = numOfValues / 2;

            //If half count is not 1
            if (halfCount != 1)
            {
                //The start spawn point must subtract the offset and
                //the spacing between objects multiplied by half of the number of values needed
                return (thisPosValue - offset - (spacing * (halfCount - 1)));
            }
            //If half count is 1
            else
            {
                //The start spawn point only needs to be offset by the offset, as only 2
                //values are needed to be spawned
                return (thisPosValue - offset);
            }
        }
        //If the required count is odd
        else
        {
            //Set the half count
            int halfCount = (numOfValues - 1) / 2;
            //Start spawn point only needs to be offset by the spacing value multiplied by the half count,
            //as there will be nothing spawning in the centre of the grid
            return (thisPosValue - (spacing * (halfCount)));
        }
    }
}
