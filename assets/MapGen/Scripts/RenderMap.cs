using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class RenderMap : MonoBehaviour 
{
	
	public        int floorNum  = 1;
	private const int floorSize = 40;
	
	public	Transform player;
	private int       playerRow;
	private int       playerCol;
	
	private Floor     floor;
	private Vector3   roomPos;
	
	private Object[]  rooms;
	private Object[]  walls;
	private Object[]  enemies;
	
	private List<FullRoom> fullRooms;
	
	public static bool roomComplete = false;
	
	// Use this for initialization
	void Start () 
	{
		floor   = new Floor         (floorNum);
		roomPos = new Vector3       (0, 0, 0);
		
		fullRooms = new List<FullRoom>();
		
		loadRoomResources  ();
		
		loadMap          ();
		foreach(var i in fullRooms)
		{
			i.render();
		}
		
	}
	void Update()
	{
		playerRow = (int) (player.position.z + 20) / floorSize;
		playerCol = (int) (player.position.x + 20) / floorSize;
		
		foreach(FullRoom f in fullRooms)
		{
			
			int distance = 0;
			distance += Mathf.Abs(playerRow - f.getRow ());
			distance += Mathf.Abs(playerCol - f.getCol ());
			
			if (distance == 0)
			{
				f.showEnemies(true);
				int count = 0;
				foreach(GameObject i in f.getEnemies())
				{
					if (i.tag == "Dead")
						++count;
				}
				if (count == f.getEnemies().Count)
				{
					roomComplete = true;
				}
				else
				{
					roomComplete = false;
				}
			}
			else
			{
				f.showEnemies(false);
			}
			if (distance >= 2)
			{
				f.show(false);
			}
			else
			{
				f.show (true);
			}
			
			
		}
	}
	
	
	private void loadRoomResources()
	{
		rooms   = Resources.LoadAll ("Rooms");
		walls   = Resources.LoadAll ("Walls");
		enemies = Resources.LoadAll ("Enemies");
	}
	private void loadMap()
	{
		int row = 0, col = 0;
		foreach (var i in floor.getFloor ())
		{
			col = 0;
			foreach(Tile tile in i)
			{
				if (!(tile.getTag() == " "))
				{
					FullRoom toAdd = new FullRoom();
					toAdd.setPos (row, col);
					
					
					GameObject room = pickRoom (tile) as GameObject;
					toAdd.setRoom(room);
					
					//Instantiate  (room, roomPos, new Quaternion());
					placeWalls   (tile, toAdd);
					
					if (tile.getTag() != "S" && tile.getTag () != "T" && tile.getTag() != "B")
					{
						placeEnemies (room.GetComponent<Transform>(), toAdd);
					}
					fullRooms.Add (toAdd);	
				}
				
				roomPos.x += floorSize;
				++col;
			}
			roomPos.x  = 0;
			roomPos.z += floorSize;
			++row;
		}
	}
	
	private Object pickRoom(Tile tile)
	{
		if (tile.getTag() == "S") // startRoom
		{
			player.transform.position = new Vector3(roomPos.x, 2, roomPos.z);
			return rooms[0];
		}

		if (tile.getTag () == "B")
		{
			return rooms[0];
		}

		if (tile.getTag () == "T") // startRoom
		{	
			return rooms[6];
		}

		if (tile.getDoor(0) && tile.getDoor (2) && !tile.getDoor(1) && /*!tile.getDoor (3) && */ (Random.Range (0, 100) <= 75))
		{
			return rooms[5];
		}
		
		return rooms[Random.Range(0, 5)];
	}
	
	
	private void placeWalls(Tile tile, FullRoom f)
	{
		List<GameObject> wallsToAdd = new List<GameObject>();
		List<Vector3> posToAdd = new List<Vector3>();
		List<Quaternion> rotToAdd = new List<Quaternion>();
		for (int i = 0; i < 4; ++i)
		{
			if (tile.getDoor (i))
			{
				Object temp = new Object();
				temp = walls[0];
				switch(i)
				{
				case 1:
				{
					if (!doorPlaced (tile, i))
					{
						GameObject wall = new GameObject();
						wall = temp as GameObject;
						posToAdd.Add (new Vector3(roomPos.x + 20.005f, 5, roomPos.z + 12.5f));
						wallsToAdd.Add (wall);
						rotToAdd.Add (new Quaternion());
						//Instantiate(temp, new Vector3(roomPos.x + 20.005f, 5, roomPos.z + 12.5f), new Quaternion());
						tile.setPlacedDoor(i);
					}
					break;
				}
				case 3:
				{
					if (!doorPlaced (tile, i))
					{
						GameObject wall = new GameObject();
						wall = temp as GameObject;
						posToAdd.Add(new Vector3(roomPos.x - 20.005f, 5, roomPos.z - 12.5f));
						wallsToAdd.Add (wall);
						rotToAdd.Add(new Quaternion());
						//Instantiate(temp, new Vector3(roomPos.x - 20.005f, 5, roomPos.z - 12.5f), new Quaternion());
						tile.setPlacedDoor(i);
					}
					break;
				}
				case 0:
				{
					if (!doorPlaced (tile, i))
					{
						GameObject g = new GameObject();
						g = temp as GameObject;
						g.transform.Rotate(90, 0, 0);
						posToAdd.Add(new Vector3(roomPos.x - 12.5f, 5, roomPos.z - 20.005f));
						wallsToAdd.Add (g);
						rotToAdd.Add(g.transform.rotation);
						//Instantiate(g, new Vector3(roomPos.x - 12.5f, 5, roomPos.z - 20.005f), g.transform.rotation);
						g.transform.Rotate(-90, 0, 0);
						tile.setPlacedDoor(i);
					}
					break;
				}
				case 2:
				{
					if (!doorPlaced (tile, i))
					{
						GameObject g = new GameObject();
						g = temp as GameObject;
						g.transform.Rotate(90, 0, 0);
						posToAdd.Add( new Vector3(roomPos.x + 12.5f, 5, roomPos.z + 20.005f));
						
						wallsToAdd.Add (g);
						rotToAdd.Add(g.transform.rotation);
						//Instantiate(g, new Vector3(roomPos.x + 12.5f, 5, roomPos.z + 20.005f), g.transform.rotation);
						g.transform.Rotate(-90, 0, 0);
						tile.setPlacedDoor(i);
					}
					break;
				}
				}
			}
			else
			{
				Object temp = new Object();
				temp = walls[1];
				switch(i)
				{
				case 1:
				{
					GameObject wall = temp as GameObject;
					posToAdd.Add(new Vector3(roomPos.x + 20.005f, 5, roomPos.z));
					wallsToAdd.Add (wall);
					rotToAdd.Add(new Quaternion());
					//Instantiate(temp, new Vector3(roomPos.x + 20.005f, 5, roomPos.z), new Quaternion());
					break;
				}
				case 3:
				{
					GameObject wall = temp as GameObject;
					posToAdd.Add(new Vector3(roomPos.x - 20.005f, 5, roomPos.z));
					wallsToAdd.Add (wall);
					rotToAdd.Add(new Quaternion());
					//Instantiate(temp, new Vector3(roomPos.x - 20.005f, 5, roomPos.z), new Quaternion());
					break;
				}
				case 0:
				{
					GameObject g = temp as GameObject;
					g.transform.Rotate(90, 0, 0);
					posToAdd.Add(new Vector3(roomPos.x, 5, roomPos.z - 20.005f));
					wallsToAdd.Add (g);
					rotToAdd.Add(g.transform.rotation);
					//Instantiate(g, new Vector3(roomPos.x, 5, roomPos.z - 20.005f), g.transform.rotation);
					g.transform.Rotate(-90, 0, 0);
					break;
				}
				case 2:
				{
					GameObject g = temp as GameObject;
					g.transform.Rotate(90, 0, 0);
					posToAdd.Add(new Vector3(roomPos.x, 5, roomPos.z + 20.005f));
					wallsToAdd.Add (g);
					rotToAdd.Add(g.transform.rotation);
					//Instantiate(g, new Vector3(roomPos.x, 5, roomPos.z + 20.005f), g.transform.rotation);
					g.transform.Rotate(-90, 0, 0);
					break;
				}
				}
			}
		}
		f.setWalls (wallsToAdd, posToAdd, rotToAdd);
	}
	
	private void placeEnemies(Transform room, FullRoom f)
	{
		List<GameObject> enemiesToAdd = new List<GameObject>();
		List<Vector3>    posToAdd = new List<Vector3>();
		foreach(Transform child in room.transform)
		{
			if (child.tag.Equals("Spawns"))
			{
				if (Random.Range (0, 100) < 50) continue;
				
				int enemyNumber = Random.Range(0, 5);
				GameObject enemy = enemies[enemyNumber] as GameObject;
				posToAdd.Add(child.position + enemy.GetComponent<Transform>().transform.position + roomPos);
				enemiesToAdd.Add (enemy);
				
				//Instantiate(enemies[enemyNumber], child.position + enemy.GetComponent<Transform>().transform.position + roomPos, enemy.GetComponent<Transform>().transform.rotation);
				
			}
		}
		f.setEnemies(enemiesToAdd, posToAdd);
	}
	
	private bool doorPlaced(Tile tile, int i)
	{
		int check = -1;
		int posRow = tile.getRow ();
		int posCol = tile.getCol ();
		
		if (i == 0)
		{
			check = 2;
			--posRow;
		}
		if (i == 1) 
		{
			check = 3;
			++posCol;
		}
		if (i == 2) 
		{
			check = 0;
			++posRow;
		}
		if (i == 3) 
		{
			check = 1;
			--posCol;
		}
		return floor.getFloor()[posRow][posCol].getPlacedDoor(check);
	}
}
