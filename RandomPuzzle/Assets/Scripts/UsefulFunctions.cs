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
}
