using UnityEngine;

namespace Basic
{
    [CreateAssetMenu]
    public class DataObject : ScriptableObject
    {
        public TextAsset studentsCourseList;
        public TextAsset studentList;
        public TextAsset studentMarkList;
        public TextAsset studentWeeklyPropertyList;
        public TextAsset playerWorkList;
        public TextAsset playerCourseList;
        public TextAsset playerCourseLevelList;
        public TextAsset storeGoodsList;
        public TextAsset supermarketGoodsList;
        public TextAsset articleList; //道具表
        public TextAsset npcList;
        [Header("剧情所用表格")] public TextAsset plotJudgmentList;
        public TextAsset branchedPlotList;
        public TextAsset linearPlotList;
        public TextAsset studentsAttributePlotList;
        public TextAsset universalPlotList;
        public TextAsset timerAndHintList;
        [Header("结局的表格")] public TextAsset enterHigherSchool;
        public TextAsset gameOverList;
        public TextAsset schoolWorkList;
        [Header("其他剧情表格")] public TextAsset otherPlotJudgmentList;

        /// <summary>
        /// 名字集
        /// </summary>
        public string[] randomName =
        {
            "松思远", "蒙奇胜", "桂弘文", "龚雨信", "任天材", "浦华荣", "白力行", "邰志尚", "怀飞捷", "欧英哲",
            "涂钦飞翰", "周阳嘉祯", "附庸飞星", "第三翰飞", "公绪涵意", "重丘俊雅", "董阏元恺", "耆门高格", "刘王嘉荣", "鲜阳英喆",
            "隆尔安", "邱龙梅", "范欣合", "菱雅柔", "田元绿", "姚紫丝", "范慕青", "萧雅凡", "焦凝洁", "万舒岚",
            "毛真", "杜擎", "秦叔", "萧晸", "梁操", "白端", "卢山", "胡伟", "潘欧", "武船",
            "钱柔", "余评", "易姲", "邓岚", "常洛", "蔡思", "马洁", "康艳", "康烁", "陆绣"
        };

        [Header("每日新闻")] public TextAsset dailyNews;
    }
}