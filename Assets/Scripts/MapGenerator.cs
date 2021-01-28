using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // constants
    public const int ROOM_SIZE = 24;

    // public variables
    public GameObject[] RoomPrefabs;
    public int NumRooms = 10;

    // private variables
    private System.Random random;

    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();

        GenerateMap();
    }

    public void GenerateMap(int numTerminals)
    {
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
            room.transform.position = new Vector3(realPosition.x, 0, realPosition.y);
            room.transform.Rotate(Vector3.up, random.Next(4) * 90);
        }
    }
 
    public void GenerateMap() => GenerateMap(3);
}
