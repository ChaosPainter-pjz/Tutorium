using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGame
{
    public class PhotographyGameStart : MonoBehaviour
    {
        private List<string> gradeIds = new List<string>()
        {
            "心情","16"
        };

        private WorldGameManager worldGameManager;

        public void StartGame()
        {
            worldGameManager ??= WorldGameManager.Instance;
            worldGameManager.selectStudentControl.UpdateUI(gradeIds,3,gameObject,worldGameManager.photographyGameControl.Init);
        }
    }
}