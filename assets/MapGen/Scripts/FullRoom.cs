using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class FullRoom : MonoBehaviour
{
	private int row;
	private int col;
	
	private GameObject   room;
	
	public bool rendered;
	
	private List<GameObject> walls;
	private List<Vector3>    wallPos;
	private List<Quaternion> wallRot;
	
	private List<GameObject> enemies;
	private List<Vector3>    enemyPos;
	
	private List<Object> toDestroy;
	private List<Object> enemiesToDestroy;
	
	public FullRoom()
	{
		row = -1;
		col = -1;
		
		room     = new GameObject();
		
		walls    = new List<GameObject>();
		wallPos  = new List<Vector3>();
		wallRot  = new List<Quaternion>();
		
		enemies  = new List<GameObject>();
		enemyPos = new List<Vector3>();
		
		toDestroy = new List<Object>();
		enemiesToDestroy = new List<Object>();
	}
	
	void Update()
	{
		if (room.tag != "Complete")
		{
			int j = 0;
			foreach(GameObject i in enemiesToDestroy)
			{
				if (i.tag == "Dead") 
				{
					++j;
				}
			}
			Debug.Log (j);
			Debug.Log(enemiesToDestroy.Count);
			if (j == enemiesToDestroy.Count) room.tag = "Complete";
		}
		
	}
	
	public List<Object> getEnemies()
	{
		return enemiesToDestroy;
	}
	public void render()
	{
		Instantiate  (room, new Vector3(40 * col, 0, 40 * row), new Quaternion());
		for(int i = 0; i < walls.Count; ++i)
		{
			Instantiate (walls[i], wallPos[i], wallRot[i]);
		}
		for(int i = 0; i < enemies.Count; ++i)
		{
			enemiesToDestroy.Add(Instantiate (enemies[i], enemyPos[i], new Quaternion()));
		}
		rendered = true;
	}
	public void show(bool b)
	{
		foreach(GameObject i in toDestroy)
		{
			if (i.tag == "Dead") i.SetActive (false);
			else                 i.SetActive(b);
		}
		//rendered = b;
	}
	public void showEnemies(bool b)
	{
		foreach(GameObject i in enemiesToDestroy)
		{
			if (i.tag == "Dead") i.SetActive (false);
			else                 i.SetActive(b);
		}
	}
	
	public void setRoom(GameObject room)
	{
		this.room = room;
	}
	public void setWalls(List<GameObject> walls, List<Vector3> pos, List<Quaternion> rot)
	{
		this.walls   = walls;
		this.wallPos = pos;
		this.wallRot = rot;
	}
	public void setEnemies(List<GameObject> enemies, List<Vector3> pos)
	{
		this.enemies = enemies;
		this.enemyPos = pos;
	}
	
	public void setPos(int row, int col)
	{
		this.row = row;
		this.col = col;
	}
	public int getRow()
	{
		return row;
	}
	public int getCol()
	{
		return col;
	}
}