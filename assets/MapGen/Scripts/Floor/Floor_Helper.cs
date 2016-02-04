using System;
using System.Collections;
using System.Collections.Generic;

public partial class Floor //HELPER METHODS
{
	private Tile findSpecialByTag(string t)
	{
		foreach(Tile tile in specialRooms)
		{
			if (tile.getTag ().Equals(t))
			{
				return tile;
			}
		}
		return new Tile();
	}

	
	// returns the amout of rooms between tile a and tile b
	private int getDistance(Tile a, Tile b) 
	{
		int retVal = 0;
		
		retVal += Math.Abs (a.getCol () - b.getCol ());
		retVal += Math.Abs (a.getRow () - b.getRow ());
		
		return retVal;
	}
	
	private void printToConsole()
	{
		string s = "";
		foreach (var i in floor)
		{
			foreach(var j in i)
			{
				s += "" + j.getTag() + " ";
			}
			s += "\n";
		}
		UnityEngine.Debug.Log(s);
		
	}

	private void Reset()
	{
		floor        = new List<List<Tile>>();
		specialRooms = new List<Tile>();
		
		makeFloor();


	}
}

