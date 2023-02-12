// This code automatically generated by TableCodeGen
using UnityEngine;
using System.Collections.Generic;
using Basic.CSV2Table;

public class PlotJudgmentList
{
	public class Row :BasicRow
	{
		public string PlotId;
		public string Year;
		public string Semester;
		public string Week;
		public string WhatDay;
		public string CourseID;
		public string NoSaveID;
		public string SaveID;
		public string MusicName;
		public string SceneName;
		public string SoundName;
		public string RoleID;
		public string FriendshipID;
		public string FriendshipValue;

	}
	public PlotJudgmentList(TextAsset csv)
	{
		Load(csv);
	}
	List<Row> rowList = new List<Row>();
	bool isLoaded = false;

	public bool IsLoaded() => isLoaded;

	public List<Row> GetRowList() => rowList;

	public void Load(TextAsset csv)
	{
		rowList.Clear();
		string[][] grid = CsvParser2.Parse(csv.text);
		for(int i = 1 ; i < grid.Length ; i++)
		{
			Row row = new Row();
			row.PlotId = grid[i][0];
			row.Year = grid[i][1];
			row.Semester = grid[i][2];
			row.Week = grid[i][3];
			row.WhatDay = grid[i][4];
			row.CourseID = grid[i][5];
			row.NoSaveID = grid[i][6];
			row.SaveID = grid[i][7];
			row.MusicName = grid[i][8];
			row.SceneName = grid[i][9];
			row.SoundName = grid[i][10];
			row.RoleID = grid[i][11];
			row.FriendshipID = grid[i][12];
			row.FriendshipValue = grid[i][13];
			row.心情 = grid[i][14];
			row.信任 = grid[i][15];
			row.气质 = grid[i][16];
			row.思维 = grid[i][17];
			row.口才 = grid[i][18];
			row.体质 = grid[i][19];
			row.善恶 = grid[i][20];
			row.语文 = grid[i][21];
			row.数学 = grid[i][22];
			row.英语 = grid[i][23];
			row.政治 = grid[i][24];
			row.历史 = grid[i][25];
			row.地理 = grid[i][26];
			row.物理 = grid[i][27];
			row.化学 = grid[i][28];
			row.生物 = grid[i][29];
			row.音乐 = grid[i][30];
			row.表演 = grid[i][31];
			row.舞蹈 = grid[i][32];
			row.手工 = grid[i][33];
			row.棋技 = grid[i][34];
			row.种植 = grid[i][35];
			row.摄影 = grid[i][36];
			row.烹饪 = grid[i][37];
			row.考古 = grid[i][38];
			row.编程 = grid[i][39];
			row.绘画 = grid[i][40];
			row.运动 = grid[i][41];

			rowList.Add(row);
		}
		isLoaded = true;
	}

	public int Count() => rowList.Count;
	public Row this[int i] => rowList[i];

	public Row Find_PlotId(string find)
	{
		return rowList.Find(x => x.PlotId == find);
	}
	public List<Row> FindAll_PlotId(string find)
	{
		return rowList.FindAll(x => x.PlotId == find);
	}
	public Row Find_Year(string find)
	{
		return rowList.Find(x => x.Year == find);
	}
	public List<Row> FindAll_Year(string find)
	{
		return rowList.FindAll(x => x.Year == find);
	}
	public Row Find_Semester(string find)
	{
		return rowList.Find(x => x.Semester == find);
	}
	public List<Row> FindAll_Semester(string find)
	{
		return rowList.FindAll(x => x.Semester == find);
	}
	public Row Find_Week(string find)
	{
		return rowList.Find(x => x.Week == find);
	}
	public List<Row> FindAll_Week(string find)
	{
		return rowList.FindAll(x => x.Week == find);
	}
	public Row Find_WhatDay(string find)
	{
		return rowList.Find(x => x.WhatDay == find);
	}
	public List<Row> FindAll_WhatDay(string find)
	{
		return rowList.FindAll(x => x.WhatDay == find);
	}
	public Row Find_CourseID(string find)
	{
		return rowList.Find(x => x.CourseID == find);
	}
	public List<Row> FindAll_CourseID(string find)
	{
		return rowList.FindAll(x => x.CourseID == find);
	}
	public Row Find_NoSaveID(string find)
	{
		return rowList.Find(x => x.NoSaveID == find);
	}
	public List<Row> FindAll_NoSaveID(string find)
	{
		return rowList.FindAll(x => x.NoSaveID == find);
	}
	public Row Find_SaveID(string find)
	{
		return rowList.Find(x => x.SaveID == find);
	}
	public List<Row> FindAll_SaveID(string find)
	{
		return rowList.FindAll(x => x.SaveID == find);
	}
	public Row Find_MusicName(string find)
	{
		return rowList.Find(x => x.MusicName == find);
	}
	public List<Row> FindAll_MusicName(string find)
	{
		return rowList.FindAll(x => x.MusicName == find);
	}
	public Row Find_SceneName(string find)
	{
		return rowList.Find(x => x.SceneName == find);
	}
	public List<Row> FindAll_SceneName(string find)
	{
		return rowList.FindAll(x => x.SceneName == find);
	}
	public Row Find_SoundName(string find)
	{
		return rowList.Find(x => x.SoundName == find);
	}
	public List<Row> FindAll_SoundName(string find)
	{
		return rowList.FindAll(x => x.SoundName == find);
	}
	public Row Find_RoleID(string find)
	{
		return rowList.Find(x => x.RoleID == find);
	}
	public List<Row> FindAll_RoleID(string find)
	{
		return rowList.FindAll(x => x.RoleID == find);
	}
	public Row Find_FriendshipID(string find)
	{
		return rowList.Find(x => x.FriendshipID == find);
	}
	public List<Row> FindAll_FriendshipID(string find)
	{
		return rowList.FindAll(x => x.FriendshipID == find);
	}
	public Row Find_FriendshipValue(string find)
	{
		return rowList.Find(x => x.FriendshipValue == find);
	}
	public List<Row> FindAll_FriendshipValue(string find)
	{
		return rowList.FindAll(x => x.FriendshipValue == find);
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