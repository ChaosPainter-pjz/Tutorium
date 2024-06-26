// This code automatically generated by TableCodeGen

using System.Collections.Generic;
using Basic.CSV2Table.Scripts;
using UnityEngine;

namespace Basic.CSV2Table
{
    public class PlayerCourseLevelList
    {
        public class Row : BasicRow
        {
            public string id;
            public string 名称;
            public string 等级;
            public string 下一级所需经验;
        }

        public PlayerCourseLevelList(TextAsset csv)
        {
            Load(csv);
        }

        private List<Row> rowList = new();
        private bool isLoaded = false;

        public bool IsLoaded()
        {
            return isLoaded;
        }

        public List<Row> GetRowList()
        {
            return rowList;
        }

        public void Load(TextAsset csv)
        {
            rowList.Clear();
            var grid = CsvParser2.Parse(csv.text);
            for (var i = 1; i < grid.Length; i++)
            {
                var row = new Row();
                row.id = grid[i][0];
                row.名称 = grid[i][1];
                row.等级 = grid[i][2];
                row.下一级所需经验 = grid[i][3];
                row.心情 = grid[i][4];
                row.信任 = grid[i][5];
                row.气质 = grid[i][6];
                row.思维 = grid[i][7];
                row.口才 = grid[i][8];
                row.体质 = grid[i][9];
                row.善恶 = grid[i][10];
                row.语文 = grid[i][11];
                row.数学 = grid[i][12];
                row.英语 = grid[i][13];
                row.政治 = grid[i][14];
                row.历史 = grid[i][15];
                row.地理 = grid[i][16];
                row.物理 = grid[i][17];
                row.化学 = grid[i][18];
                row.生物 = grid[i][19];
                row.音乐 = grid[i][20];
                row.表演 = grid[i][21];
                row.舞蹈 = grid[i][22];
                row.手工 = grid[i][23];
                row.棋技 = grid[i][24];
                row.种植 = grid[i][25];
                row.摄影 = grid[i][26];
                row.烹饪 = grid[i][27];
                row.考古 = grid[i][28];
                row.编程 = grid[i][29];
                row.绘画 = grid[i][30];
                row.运动 = grid[i][31];

                rowList.Add(row);
            }

            isLoaded = true;
        }

        public int Count()
        {
            return rowList.Count;
        }

        public Row this[int i] => rowList[i];

        public Row Find_id(string find)
        {
            return rowList.Find(x => x.id == find);
        }

        public List<Row> FindAll_id(string find)
        {
            return rowList.FindAll(x => x.id == find);
        }

        public Row Find_名称(string find)
        {
            return rowList.Find(x => x.名称 == find);
        }

        public List<Row> FindAll_名称(string find)
        {
            return rowList.FindAll(x => x.名称 == find);
        }

        public Row Find_等级(string find)
        {
            return rowList.Find(x => x.等级 == find);
        }

        public List<Row> FindAll_等级(string find)
        {
            return rowList.FindAll(x => x.等级 == find);
        }

        public Row Find_下一级所需经验(string find)
        {
            return rowList.Find(x => x.下一级所需经验 == find);
        }

        public List<Row> FindAll_下一级所需经验(string find)
        {
            return rowList.FindAll(x => x.下一级所需经验 == find);
        }

        public Row Find_心情(string find)
        {
            return rowList.Find(x => x.心情 == find);
        }

        public List<Row> FindAll_心情(string find)
        {
            return rowList.FindAll(x => x.心情 == find);
        }

        public Row Find_信任(string find)
        {
            return rowList.Find(x => x.信任 == find);
        }

        public List<Row> FindAll_信任(string find)
        {
            return rowList.FindAll(x => x.信任 == find);
        }

        public Row Find_气质(string find)
        {
            return rowList.Find(x => x.气质 == find);
        }

        public List<Row> FindAll_气质(string find)
        {
            return rowList.FindAll(x => x.气质 == find);
        }

        public Row Find_思维(string find)
        {
            return rowList.Find(x => x.思维 == find);
        }

        public List<Row> FindAll_思维(string find)
        {
            return rowList.FindAll(x => x.思维 == find);
        }

        public Row Find_口才(string find)
        {
            return rowList.Find(x => x.口才 == find);
        }

        public List<Row> FindAll_口才(string find)
        {
            return rowList.FindAll(x => x.口才 == find);
        }

        public Row Find_体质(string find)
        {
            return rowList.Find(x => x.体质 == find);
        }

        public List<Row> FindAll_体质(string find)
        {
            return rowList.FindAll(x => x.体质 == find);
        }

        public Row Find_善恶(string find)
        {
            return rowList.Find(x => x.善恶 == find);
        }

        public List<Row> FindAll_善恶(string find)
        {
            return rowList.FindAll(x => x.善恶 == find);
        }

        public Row Find_语文(string find)
        {
            return rowList.Find(x => x.语文 == find);
        }

        public List<Row> FindAll_语文(string find)
        {
            return rowList.FindAll(x => x.语文 == find);
        }

        public Row Find_数学(string find)
        {
            return rowList.Find(x => x.数学 == find);
        }

        public List<Row> FindAll_数学(string find)
        {
            return rowList.FindAll(x => x.数学 == find);
        }

        public Row Find_英语(string find)
        {
            return rowList.Find(x => x.英语 == find);
        }

        public List<Row> FindAll_英语(string find)
        {
            return rowList.FindAll(x => x.英语 == find);
        }

        public Row Find_政治(string find)
        {
            return rowList.Find(x => x.政治 == find);
        }

        public List<Row> FindAll_政治(string find)
        {
            return rowList.FindAll(x => x.政治 == find);
        }

        public Row Find_历史(string find)
        {
            return rowList.Find(x => x.历史 == find);
        }

        public List<Row> FindAll_历史(string find)
        {
            return rowList.FindAll(x => x.历史 == find);
        }

        public Row Find_地理(string find)
        {
            return rowList.Find(x => x.地理 == find);
        }

        public List<Row> FindAll_地理(string find)
        {
            return rowList.FindAll(x => x.地理 == find);
        }

        public Row Find_物理(string find)
        {
            return rowList.Find(x => x.物理 == find);
        }

        public List<Row> FindAll_物理(string find)
        {
            return rowList.FindAll(x => x.物理 == find);
        }

        public Row Find_化学(string find)
        {
            return rowList.Find(x => x.化学 == find);
        }

        public List<Row> FindAll_化学(string find)
        {
            return rowList.FindAll(x => x.化学 == find);
        }

        public Row Find_生物(string find)
        {
            return rowList.Find(x => x.生物 == find);
        }

        public List<Row> FindAll_生物(string find)
        {
            return rowList.FindAll(x => x.生物 == find);
        }

        public Row Find_音乐(string find)
        {
            return rowList.Find(x => x.音乐 == find);
        }

        public List<Row> FindAll_音乐(string find)
        {
            return rowList.FindAll(x => x.音乐 == find);
        }

        public Row Find_表演(string find)
        {
            return rowList.Find(x => x.表演 == find);
        }

        public List<Row> FindAll_表演(string find)
        {
            return rowList.FindAll(x => x.表演 == find);
        }

        public Row Find_舞蹈(string find)
        {
            return rowList.Find(x => x.舞蹈 == find);
        }

        public List<Row> FindAll_舞蹈(string find)
        {
            return rowList.FindAll(x => x.舞蹈 == find);
        }

        public Row Find_手工(string find)
        {
            return rowList.Find(x => x.手工 == find);
        }

        public List<Row> FindAll_手工(string find)
        {
            return rowList.FindAll(x => x.手工 == find);
        }

        public Row Find_棋技(string find)
        {
            return rowList.Find(x => x.棋技 == find);
        }

        public List<Row> FindAll_棋技(string find)
        {
            return rowList.FindAll(x => x.棋技 == find);
        }

        public Row Find_种植(string find)
        {
            return rowList.Find(x => x.种植 == find);
        }

        public List<Row> FindAll_种植(string find)
        {
            return rowList.FindAll(x => x.种植 == find);
        }

        public Row Find_摄影(string find)
        {
            return rowList.Find(x => x.摄影 == find);
        }

        public List<Row> FindAll_摄影(string find)
        {
            return rowList.FindAll(x => x.摄影 == find);
        }

        public Row Find_烹饪(string find)
        {
            return rowList.Find(x => x.烹饪 == find);
        }

        public List<Row> FindAll_烹饪(string find)
        {
            return rowList.FindAll(x => x.烹饪 == find);
        }

        public Row Find_考古(string find)
        {
            return rowList.Find(x => x.考古 == find);
        }

        public List<Row> FindAll_考古(string find)
        {
            return rowList.FindAll(x => x.考古 == find);
        }

        public Row Find_编程(string find)
        {
            return rowList.Find(x => x.编程 == find);
        }

        public List<Row> FindAll_编程(string find)
        {
            return rowList.FindAll(x => x.编程 == find);
        }

        public Row Find_绘画(string find)
        {
            return rowList.Find(x => x.绘画 == find);
        }

        public List<Row> FindAll_绘画(string find)
        {
            return rowList.FindAll(x => x.绘画 == find);
        }

        public Row Find_运动(string find)
        {
            return rowList.Find(x => x.运动 == find);
        }

        public List<Row> FindAll_运动(string find)
        {
            return rowList.FindAll(x => x.运动 == find);
        }
    }
}