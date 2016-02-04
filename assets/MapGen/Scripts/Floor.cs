using System;
using System.Collections;
using System.Collections.Generic;

public partial class Floor 
{
	public Floor(int floorNum)
	{

		floorNumber = floorNum;
		floorSize   = 3 + (int)(floorNumber * 1.5);

		minDistance = 2 + (floorNumber / 2);
		maxDistance = 2 + (floorNumber * 2); 

		if (minDistance > 13) minDistance = 13;

		gen = new Random();

		bool a = false, b = false;

		//Sometimes there can be a placement where
		//there is no possible solution for the map
		//so keep going until a correct one is found
		do
		{
			floor        = new List<List<Tile>>();
			specialRooms = new List<Tile>();

			makeFloor();
			a = makeSpecialRooms();
			b = placeDeadEnds();
		}while (!a || !b);

		connectRooms ();

		setDoors();

		//printToConsole ();
	}

	public List<List<Tile>> getFloor()
	{
		return floor;
	}
	public int getFloorSize()
	{
		return floorSize;
	}
}



