using System;
using Basic;
using SaveManager.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameSence.World.Market
{
    /// <summary>
    /// 朋克城控制器
    /// </summary>
    public class CoexControl : MonoBehaviour
    {
        [SerializeField] private SaveObject saveObject;
        [SerializeField] private Transform m_taskContents;

        private CoexData coexData;
        [SerializeField] private CoexCard[] m_tasks;

        private void OnEnable()
        {
            AudioControl.Instance?.PlayBackgroundMusic(AudioControl.BackgroundMusicType.World);
            coexData ??= saveObject.SaveData.coexData;
            RefreshTaskList();
            OnTaskPanel();
        }

        /// <summary>
        /// 点击了返回按钮
        /// </summary>
        public void OnReturn()
        {
            AudioControl.Instance.PlayBackgroundMusic(AudioControl.BackgroundMusicType.World);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 打开面板，读取存档，显示内容
        /// </summary>
        private void OnTaskPanel()
        {
            //显示未接任务
            for (var i = 0; i < m_tasks.Length; i++)
                if (i < coexData.LockTasks.Count)
                    m_tasks[i].Init(saveObject.SaveData, coexData.LockTasks[i], OnFetchTask);
                else if (i < coexData.LockTasks.Count + coexData.Tasks.Count)
                    m_tasks[i].Init(saveObject.SaveData, coexData.Tasks[i - coexData.LockTasks.Count], OnFetchTask);
                else
                    m_tasks[i].gameObject.SetActive(false);
        }

        /// <summary>
        /// 刷新任务列表
        /// 刷新规则：
        /// 1.相差一周，则随机刷新1条任务
        /// 2.相差二周及以上，随机刷新2~3条任务
        /// 3.任务不满5条的，优先补满5条，之后再替换
        /// </summary>
        private void RefreshTaskList()
        {
            if (Math.Abs(coexData.date.Week - saveObject.SaveData.gameDate.Week) == 1 &&
                coexData.date.Semester == saveObject.SaveData.gameDate.Semester &&
                coexData.date.year == saveObject.SaveData.gameDate.year)
            {
                // 只相差一周，刷新1条
                if (coexData.Tasks.Count < 5)
                    //这里只补充
                    while (coexData.Tasks.Count < 5)
                        coexData.Tasks.Add(GetATask());
                else
                    //刷新1条
                    coexData.Tasks[Random.Range(0, coexData.Tasks.Count)] = GetATask();
            }
            else if (coexData.date != saveObject.SaveData.gameDate)
            {
                //刷新2-3条
                if (coexData.Tasks.Count < 5)
                {
                    //这里只补充
                    while (coexData.Tasks.Count < 5) coexData.Tasks.Add(GetATask());
                }
                else
                {
                    // 刷新2-3条
                    var number = Random.Range(2, 4);
                    int index1, index2, index3;
                    index1 = Random.Range(0, 5);

                    index2 = index1;
                    while (index2 == index1) index2 = Random.Range(0, 5);

                    index3 = index1;
                    while (index3 == index2 || index3 == index1) index3 = Random.Range(0, 5);
                    coexData.Tasks[index1] = GetATask();
                    coexData.Tasks[index2] = GetATask();
                    if (number == 3) coexData.Tasks[index3] = GetATask();

                    coexData.Tasks[Random.Range(0, coexData.Tasks.Count)] = GetATask();
                }
            }

            coexData.date = saveObject.SaveData.gameDate.Copy();
        }

        /// <summary>
        /// 生成一个新的任务
        /// </summary>
        /// <returns></returns>
        private CoexData.Task GetATask()
        {
            CoexData.Task task = new();
            var studentIndex = Random.Range(0, saveObject.SaveData.studentUnits.Count);
            var studentUnit = saveObject.SaveData.studentUnits[studentIndex];
            task.UnitId = studentUnit.id;

            if (Random.Range(0, 5f) < 1)
            {
                //基本属性任务 20%
                task.Grade = studentUnit.properties[Random.Range(0, studentUnit.properties.Count)].Copy();
                task.Grade.score += Random.Range(5, 10);
                var range = Random.Range(0.08f, 0.12f);
                task.RewardValue = Mathf.RoundToInt(task.Grade.score * range);
            }
            else if (Random.Range(0, 1f) < 0.5f || studentUnit.interestGrade.Count == 0)
            {
                //如果不存在兴趣，就转为学科studentUnit.interestGrade.Count==0
                //学科任务 40%
                task.Grade = studentUnit.mainGrade[Random.Range(0, studentUnit.mainGrade.Count)].Copy();
                task.Grade.score += Random.Range(25, 60);
                var range = Random.Range(0.08f, 0.16f);
                task.RewardValue = Mathf.RoundToInt(task.Grade.score * range);
            }
            else
            {
                //兴趣任务 40%
                task.Grade = studentUnit.interestGrade[Random.Range(0, studentUnit.interestGrade.Count)].Copy();
                task.Grade.score += Random.Range(25, 60);
                var range = Random.Range(0.08f, 0.16f);
                task.RewardValue = Mathf.RoundToInt(task.Grade.score * range);
            }

            return task;
        }


        /// <summary>
        /// 点击了卡片的按钮，需要刷新面板
        /// </summary>
        private void OnFetchTask()
        {
            CardUpdate();
        }

        /// <summary>
        /// 刷新卡片的显示
        /// </summary>
        private void CardUpdate()
        {
            OnTaskPanel();
        }
    }
}
