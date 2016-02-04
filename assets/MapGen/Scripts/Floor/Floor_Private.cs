using System;
using System.Collections;
using System.Collections.Generic;

public partial class Floor // PRIVATE MEMBERS
{
	private int floorNumber;
	private int floorSize;
	
	private int minDistance;
	private int maxDistance;
	
	
	private Random gen;
	
	private List<List<Tile>> floor;
	private List<Tile>       specialRooms;
	
}