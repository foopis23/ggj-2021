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
            GameObject Room = RoomPrefabs[(int) (Random.value * RoomPrefabs.Length)];
            Room.transform.position = roomLocation * ROOM_SIZE;
        }
    }
 
    public void GenerateMap() => GenerateMap(3);
}
