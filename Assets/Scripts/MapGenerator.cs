﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // constants
    public const int ROOM_SIZE = 24;
    public const int LEVEL_OFFSET = 1000;

    // public variables
    public GameObject[] RoomPrefabs;
    public int NumRooms = 10;

    // private variables
    private System.Random random;
    private Vector3 nextLevelLocation;
    private Vector3 NextLevelLocation
    {
        get { UpdateNextLevelLocation(); return nextLevelLocation; }
        set { nextLevelLocation = value; }
    }
    
    // Automatic Properties
    public List<Level> Levels { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();
        Levels = new List<Level>();
        nextLevelLocation = Vector3.left;

        GenerateLevel();
    }

    public void GenerateLevel()
    {
        Level level = new Level();
        Vector3 levelLocation = LEVEL_OFFSET * NextLevelLocation;
        int numTerminals = Levels.Count == 0 ? 3 : random.Next(1, 4);

        Vector2[] directions = {Vector2.up, Vector2.right, Vector2.down, Vector2.left};
        List<Vector2> roomLocations = new List<Vector2>();
        List<Vector2> terminalLocations = new List<Vector2>();
        roomLocations.Add(Vector2.zero);

        while(roomLocations.Count < NumRooms)
        {
            Vector2 randRoom = roomLocations[random.Next(roomLocations.Count)];
            Vector2 nextRoom = randRoom + directions[random.Next(4)];
            if(!roomLocations.Contains(nextRoom))
            {
                roomLocations.Add(nextRoom);
            }
        }

        while(terminalLocations.Count < numTerminals)
        {
            Vector2 randRoom = roomLocations[random.Next(roomLocations.Count)];
            if(randRoom != Vector2.zero)
            {
                terminalLocations.Add(randRoom);
            }
        }

        foreach(Vector2 roomLocation in roomLocations)
        {
            GameObject roomPrefab = RoomPrefabs[random.Next(RoomPrefabs.Length)];
            GameObject room = Instantiate(roomPrefab);
            Vector2 realPosition = roomLocation * ROOM_SIZE;
            room.transform.position = levelLocation + new Vector3(realPosition.x, 0, realPosition.y);
            level.Rooms.Add(room.GetComponent<Room>());
        }

        Levels.Add(level);
    }
 
    public void UpdateNextLevelLocation()
    {
        nextLevelLocation.x++;
        if(nextLevelLocation.x >= 10)
        {
            nextLevelLocation.x = 0;
            nextLevelLocation.z++;
            if(nextLevelLocation.z >= 10)
            {
                nextLevelLocation.y++;
            }
        }
    }
}
