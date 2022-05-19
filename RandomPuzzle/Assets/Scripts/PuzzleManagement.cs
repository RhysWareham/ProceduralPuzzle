using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PuzzleManagement
{
    public static List<int> RequiredCode = new List<int>();
    public static List<int> ShuffledOrder = new List<int>();
    public enum Difficulty
    {
        EASY,
        MEDIUM,
        HARD
    };

    public static Difficulty ChosenDifficulty;

    public static bool PuzzleComplete = true;
    public static float[] FastestTime = new float[3] { 0.0f, 0.0f, 0.0f }; //Easy time, Medium time, Hard time
}
