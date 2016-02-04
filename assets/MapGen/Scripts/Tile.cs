using System;

public class Tile
{
	private int row;
	private int col;

	//up    = 0,
	//right = 1,
	//down  = 2,
	//left  = 3
	private bool[] doors;
	private bool[] placedDoors;

	private string tag;

	public Tile()
	{
		row = col = -1;
		tag = " ";

		doors       = new bool[4];
		placedDoors = new bool[4];
	}

	public void setPos(int r, int c)
	{
		row = r;
		col = c;
	}

	public int getRow()
	{
		return row;
	}
	public int getCol()
	{
		return col;
	}

	public string getTag()
	{
		return tag;
	}
	public void setTag(string t)
	{
		tag = t;
	}

	public void setDoor(int i)
	{
		if (i >= 4) return;
		if (i <  0) return;

		doors[i] = true;
	}
	public bool getDoor(int i)
	{
		if (i >= 4) return false;
		if (i <  0 )return false;

		return doors[i];
	}

	public bool[] getDoors()
	{
		return doors;
	}

	public void setPlacedDoor(int i)
	{
		if (i >= 4) return;
		if (i <  0) return;
		
		placedDoors[i] = true;
	}
	public bool getPlacedDoor(int i)
	{
		if (i >= 4) return false;
		if (i <  0 )return false;
		
		return placedDoors[i];
	}

}
