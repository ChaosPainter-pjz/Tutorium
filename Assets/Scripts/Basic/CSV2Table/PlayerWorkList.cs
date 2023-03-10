// This code automatically generated by TableCodeGen
using UnityEngine;
using System.Collections.Generic;

public class PlayerWorkList
{
	public class Row
	{
		public string id;
		public string name;
		public string description;
		public string maxLevel;
		public string InitialYield;
		public string levelYield;
		public string UpExperience;

	}
	public PlayerWorkList(TextAsset csv)
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
			row.id = grid[i][0];
			row.name = grid[i][1];
			row.description = grid[i][2];
			row.maxLevel = grid[i][3];
			row.InitialYield = grid[i][4];
			row.levelYield = grid[i][5];
			row.UpExperience = grid[i][6];

			rowList.Add(row);
		}
		isLoaded = true;
	}

	public int Count() => rowList.Count;
	public Row this[int i] => rowList[i];

	public Row Find_id(string find)
	{
		return rowList.Find(x => x.id == find);
	}
	public List<Row> FindAll_id(string find)
	{
		return rowList.FindAll(x => x.id == find);
	}
	public Row Find_name(string find)
	{
		return rowList.Find(x => x.name == find);
	}
	public List<Row> FindAll_name(string find)
	{
		return rowList.FindAll(x => x.name == find);
	}
	public Row Find_description(string find)
	{
		return rowList.Find(x => x.description == find);
	}
	public List<Row> FindAll_description(string find)
	{
		return rowList.FindAll(x => x.description == find);
	}
	public Row Find_maxLevel(string find)
	{
		return rowList.Find(x => x.maxLevel == find);
	}
	public List<Row> FindAll_maxLevel(string find)
	{
		return rowList.FindAll(x => x.maxLevel == find);
	}
	public Row Find_InitialYield(string find)
	{
		return rowList.Find(x => x.InitialYield == find);
	}
	public List<Row> FindAll_InitialYield(string find)
	{
		return rowList.FindAll(x => x.InitialYield == find);
	}
	public Row Find_levelYield(string find)
	{
		return rowList.Find(x => x.levelYield == find);
	}
	public List<Row> FindAll_levelYield(string find)
	{
		return rowList.FindAll(x => x.levelYield == find);
	}
	public Row Find_UpExperience(string find)
	{
		return rowList.Find(x => x.UpExperience == find);
	}
	public List<Row> FindAll_UpExperience(string find)
	{
		return rowList.FindAll(x => x.UpExperience == find);
	}

}