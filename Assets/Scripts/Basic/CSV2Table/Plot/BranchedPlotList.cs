// This code automatically generated by TableCodeGen
using UnityEngine;
using System.Collections.Generic;

public class BranchedPlotList
{
	public class Row
	{
		public string PlotId;
		public string Text;
		public string RoleID;
		public string RolePortrait;
		public string Branches_1;
		public string NextPlotId_1;
		public string Branches_2;
		public string NextPlotId_2;
		public string Branches_3;
		public string NextPlotId_3;
		public string Branches_4;
		public string NextPlotId_4;

	}
	public BranchedPlotList(TextAsset csv)
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
			row.Text = grid[i][1];
			row.RoleID = grid[i][2];
			row.RolePortrait = grid[i][3];
			row.Branches_1 = grid[i][4];
			row.NextPlotId_1 = grid[i][5];
			row.Branches_2 = grid[i][6];
			row.NextPlotId_2 = grid[i][7];
			row.Branches_3 = grid[i][8];
			row.NextPlotId_3 = grid[i][9];
			row.Branches_4 = grid[i][10];
			row.NextPlotId_4 = grid[i][11];

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
	public Row Find_Text(string find)
	{
		return rowList.Find(x => x.Text == find);
	}
	public List<Row> FindAll_Text(string find)
	{
		return rowList.FindAll(x => x.Text == find);
	}
	public Row Find_RoleID(string find)
	{
		return rowList.Find(x => x.RoleID == find);
	}
	public List<Row> FindAll_RoleID(string find)
	{
		return rowList.FindAll(x => x.RoleID == find);
	}
	public Row Find_RolePortrait(string find)
	{
		return rowList.Find(x => x.RolePortrait == find);
	}
	public List<Row> FindAll_RolePortrait(string find)
	{
		return rowList.FindAll(x => x.RolePortrait == find);
	}
	public Row Find_Branches_1(string find)
	{
		return rowList.Find(x => x.Branches_1 == find);
	}
	public List<Row> FindAll_Branches_1(string find)
	{
		return rowList.FindAll(x => x.Branches_1 == find);
	}
	public Row Find_NextPlotId_1(string find)
	{
		return rowList.Find(x => x.NextPlotId_1 == find);
	}
	public List<Row> FindAll_NextPlotId_1(string find)
	{
		return rowList.FindAll(x => x.NextPlotId_1 == find);
	}
	public Row Find_Branches_2(string find)
	{
		return rowList.Find(x => x.Branches_2 == find);
	}
	public List<Row> FindAll_Branches_2(string find)
	{
		return rowList.FindAll(x => x.Branches_2 == find);
	}
	public Row Find_NextPlotId_2(string find)
	{
		return rowList.Find(x => x.NextPlotId_2 == find);
	}
	public List<Row> FindAll_NextPlotId_2(string find)
	{
		return rowList.FindAll(x => x.NextPlotId_2 == find);
	}
	public Row Find_Branches_3(string find)
	{
		return rowList.Find(x => x.Branches_3 == find);
	}
	public List<Row> FindAll_Branches_3(string find)
	{
		return rowList.FindAll(x => x.Branches_3 == find);
	}
	public Row Find_NextPlotId_3(string find)
	{
		return rowList.Find(x => x.NextPlotId_3 == find);
	}
	public List<Row> FindAll_NextPlotId_3(string find)
	{
		return rowList.FindAll(x => x.NextPlotId_3 == find);
	}
	public Row Find_Branches_4(string find)
	{
		return rowList.Find(x => x.Branches_4 == find);
	}
	public List<Row> FindAll_Branches_4(string find)
	{
		return rowList.FindAll(x => x.Branches_4 == find);
	}
	public Row Find_NextPlotId_4(string find)
	{
		return rowList.Find(x => x.NextPlotId_4 == find);
	}
	public List<Row> FindAll_NextPlotId_4(string find)
	{
		return rowList.FindAll(x => x.NextPlotId_4 == find);
	}

}