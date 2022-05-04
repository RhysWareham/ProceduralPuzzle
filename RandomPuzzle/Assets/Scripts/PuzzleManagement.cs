using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PuzzleManagement
{
    public static List<int> RequiredCode = new List<int>();
    public enum Difficulty
    {
        EASY,
        MEDIUM,
        HARD
    };

    public static Difficulty ChosenDifficulty; 
}
