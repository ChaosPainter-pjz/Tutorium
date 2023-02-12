using System.Collections.Generic;
using UnityEngine;

public static class GameMathf
{
    public static List<int> RandomList(int min, int max, int number)
    {
        List<int> list = new List<int>();
        for (int i = 0; i < number; i++)
        {
            list.Add(i);
        }

        for (int index = 0; index < list.Count; index++)
        {
            Random.InitState(list[index]);
            list[index] = Random.Range(min, max);
        }

        return list;
    }
}