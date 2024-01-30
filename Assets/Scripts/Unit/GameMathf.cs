using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    public static class GameMathf
    {
        public static List<int> RandomList(int min, int max, int number)
        {
            var list = new List<int>();
            for (var i = 0; i < number; i++) list.Add(i);

            for (var index = 0; index < list.Count; index++)
            {
                Random.InitState(list[index]);
                list[index] = Random.Range(min, max);
            }

            return list;
        }
    }
}