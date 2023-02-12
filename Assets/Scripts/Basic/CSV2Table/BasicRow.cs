using System;

namespace Basic.CSV2Table
{
    public abstract class BasicRow
    {
        public string 心情;
        public string 信任;
        public string 气质;
        public string 思维;
        public string 口才;
        public string 体质;
        public string 善恶;
        public string 语文;
        public string 数学;
        public string 英语;
        public string 政治;
        public string 历史;
        public string 地理;
        public string 物理;
        public string 化学;
        public string 生物;
        public string 音乐;
        public string 表演;
        public string 舞蹈;
        public string 手工;
        public string 棋技;
        public string 种植;
        public string 摄影;
        public string 烹饪;
        public string 考古;
        public string 编程;
        public string 绘画;
        public string 运动;

        private string[] StringProperties => new[] {气质, 思维, 口才, 体质, 善恶};
        public int[] Properties => Array.ConvertAll(StringProperties, s => int.TryParse(s, out int value)?value:0);
        private string[] StringAllProperties => new[] {心情, 信任, 气质, 思维, 口才, 体质, 善恶};
        public int[] AllProperties => Array.ConvertAll(StringAllProperties, s => int.TryParse(s, out int value)?value:0);

        private string[] StringMainGrade => new[] {语文, 数学, 英语, 政治, 历史, 地理, 物理, 化学, 生物};
        public int[] MainGrade => Array.ConvertAll(StringMainGrade, s => int.TryParse(s, out int value)?value:0);

        private string[] StringInterestGrade => new[] {音乐, 表演, 舞蹈, 手工, 棋技, 种植, 摄影, 烹饪, 考古, 编程, 绘画, 运动};
        public int[] InterestGrade => Array.ConvertAll(StringInterestGrade, s => int.TryParse(s, out int value)?value:0);




    }
}