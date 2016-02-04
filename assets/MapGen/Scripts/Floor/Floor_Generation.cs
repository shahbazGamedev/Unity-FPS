using System;
using System.Collections;
using System.Collections.Generic;


public partial class Floor // MAP GEN
{
	private void makeFloor()
	{
		for (int i = 0; i < floorSize; ++i)
		{
			List<Tile> row = new List<Tile>();
			for (int j = 0; j < floorSize; ++j)
			{
				Tile t = new Tile();
				
				t.setPos (i, j);
				row.Add  (t);
			}
			floor.Add (row);
		}
	}
	
	private bool makeSpecialRooms()
	{
		Tile bossRoom     = new Tile();
		Tile startRoom    = new Tile();
		Tile treasureRoom = new Tile();
		
		bossRoom.setTag     ("B");
		startRoom.setTag    ("S");
		treasureRoom.setTag ("T");
		
		if (!placeSpecialRoom(bossRoom)    ||
		    !placeSpecialRoom(startRoom)   ||
		    !placeSpecialRoom(treasureRoom))
		    {
				return false;
			}
		return true;
	}


	private bool placeSpecialRoom(Tile special)
	{
		specialRooms.Add (special);
		bool ok;

		if (special.getTag ().Equals ("S"))
		{
			special.setPos(floorSize / 2, floorSize / 2);

			floor[special.getRow ()][special.getCol ()] = special;
			return true;
		}
		//checks to see if the newly placed tile
		//is the minimum distance away from every
		//other specially placed tile

		int tries = 0;
		do
		{
			if (tries > 50)
			{
				return false;
			}
			ok = true;
			special.setPos (gen.Next(0, floorSize), gen.Next(0, floorSize));
			foreach (Tile i in specialRooms)
			{
				if (i.Equals(special)) continue;
				//UnityEngine.Debug.Log (i.getTag () + special.getTag () + " DISTANCE: " + getDistance (i, special) + " MIN DISTANCE: " + minDistance );
				if (getDistance (i, special) < minDistance)	
				{
					ok = false;
					break;
				}
				if (getDistance (i, special) > maxDistance)	
				{
					ok = false;
					break;
				}

				++tries;
				
			}
		}while (!ok);
		
		floor[special.getRow ()][special.getCol ()] = special;
		return true;
	}

	private bool placeDeadEnds()
	{
		for (int i = 0; i < Math.Sqrt (floorNumber); ++i)
		{
			Tile endRoom = new Tile();
			endRoom.setTag("E");
			
			if (!placeSpecialRoom (endRoom))
			{
				return false;
			}
		}
		return true;
	}
	
	//Bresenhams line drawing algorithms
	private void connectRooms() // connects all the rooms to start
	{
		Tile start = findSpecialByTag("S");
		
		int row;
		int col;
		
		foreach(Tile tile in specialRooms)
		{
			if (tile.getTag ().Equals ("S")) continue;
			
			row = start.getRow ();
			col = start.getCol ();
			
			int w = tile.getCol() - col;
			int h = tile.getRow() - row;
			
			int dx1 = 0;
			int dx2 = 0;
			int dy1 = 0;
			int dy2 = 0;
			
			if (w < 0 ) 
			{
				dx1 = -1;
				dx2 = -1;
			}
			else if (w > 0)
			{
				dx1 = 1;
				dx2 = 1;
			}
			if (h < 0)      dy1 = -1;
			else if (h > 0) dy1 =  1;
			
			int longest  = Math.Abs (w);
			int shortest = Math.Abs (h);
			
			if (!(longest > shortest))
			{
				int temp = longest;
				longest  = shortest;
				shortest = temp;
				if (h < 0)      dy2 = -1;
				else if (h > 0) dy2 =  1;
				
				dx2 = 0;
			}
			
			int numerator = longest >> 1;
			for (int i = 0; i < longest; ++i)
			{
				numerator += shortest;
				if (!(numerator < longest))
				{
					numerator -= longest;
					
					row += dy1;
					
					if (floor[row][col].getTag ().Equals (" "))
					{
						floor[row][col].setPos (row, col);
						floor[row][col].setTag ("+");
					}
					
					col += dx1;
					if (floor[row][col].getTag ().Equals (" "))
					{
						floor[row][col].setPos (row, col);
						floor[row][col].setTag ("+");
					}
				}
				else
				{
					col += dx2;
					
					if (floor[row][col].getTag ().Equals (" "))
					{
						floor[row][col].setPos (row, col);
						floor[row][col].setTag ("+");
					}
					row += dy2;
					if (floor[row][col].getTag ().Equals (" "))
					{
						floor[row][col].setPos (row, col);
						floor[row][col].setTag ("+");
					}
				}
			}	
		}
	}

	private void setDoors()
	{
		foreach(var i in floor)
		{
			foreach(Tile j in i)
			{
				if (j.getTag() == " ") continue;
				int row = j.getRow ();
				int col = j.getCol ();
				try
				{
					if (floor[row-1][col].getTag () != " ") j.setDoor (0);
				}
				catch(Exception e) {}

				try
				{
					if (floor[row][col + 1].getTag () != " ") j.setDoor (1);
				}
				catch(Exception e) {}

				try
				{
					if (floor[row + 1][col].getTag () != " ") j.setDoor (2);
				}
				catch(Exception e) {}

				try
				{
					if (floor[row][col - 1].getTag () != " ") j.setDoor (3);
				}
				catch(Exception e) {}

			}
		}
	}
	
}
