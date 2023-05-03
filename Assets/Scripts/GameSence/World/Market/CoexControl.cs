using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace World
{
    /// <summary>
    /// 朋克城控制器
    /// </summary>
    public class CoexControl : MonoBehaviour
    {
        [SerializeField] private SaveObject saveObject;
        public Text taskContentText;
        public Text rewardText;

        private void OnEnable()
        {
            AudioControl.Instance.PlayBackgroundMusic(AudioControl.BackgroundMusicType.World);
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

        //增加任务面板功能，可以显示、接取任务。任务内容随机生成，内容主要是指定天数内某个属性达到某个数值，任务奖励是奖励属性数值
        private void OnTaskPanel()
        {
            RefreshTaskList();
            //显示任务列表
            //生成随机任务
            int days = Random.Range(1, 7);
            int attribute = Random.Range(1, 4);
            int value = Random.Range(10, 21);
            string taskContent = $"在{days}天内将{attribute}属性提升至{value}";

            //显示任务内容
            taskContentText.text = $"任务内容：{taskContent}";


            //奖励属性数值
            rewardText.text = $"奖励{attribute}属性{value}点";
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
            CoexData coexData = saveObject.SaveData.coexData;

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
                    int number = Random.Range(2, 4);
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

        private CoexData.Task GetATask()
        {
            CoexData.Task task = new();
            int studentIndex = Random.Range(0, saveObject.SaveData.studentUnits.Count);
            StudentUnit studentUnit = saveObject.SaveData.studentUnits[studentIndex];
            task.UnitId = studentUnit.id;

            if (Random.Range(0, 5f) < 1)
            {
                //基本属性任务 20%
                task.Grade = studentUnit.properties[Random.Range(0, studentUnit.properties.Count)].Copy();
                task.Grade.score += Random.Range(5, 10);
            }
            else if (Random.Range(0, 1f) < 0.5f || studentUnit.interestGrade.Count == 0)
            {
                //如果不存在兴趣，就转为学科studentUnit.interestGrade.Count==0
                //学科任务 40%
                task.Grade = studentUnit.mainGrade[Random.Range(0, studentUnit.mainGrade.Count)].Copy();
                task.Grade.score += Random.Range(25, 40);
            }
            else
            {
                //兴趣任务 40%
                task.Grade = studentUnit.interestGrade[Random.Range(0, studentUnit.interestGrade.Count)].Copy();
                task.Grade.score += Random.Range(25, 40);
            }

            return task;
        }


        //接取任务
        private void OnFetchTask()
        {
        }
    }
}