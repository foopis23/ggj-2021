using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CallbackEvents;

public class GenerateNextLevelContext : EventContext
{
    public Vector3 location;

    public GenerateNextLevelContext(Vector3 location)
    {
        this.location = location;
    }
}

public class MapGenerator : MonoBehaviour
{
    // constants
    public const int ROOM_SIZE = 48;
    public const int LEVEL_OFFSET = 1000;

    // public variables
    public GameObject[] RoomPrefabs;
    public GameObject TerminalPrefab;
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
    public Dictionary<Vector3, Level> Levels { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();
        Levels = new Dictionary<Vector3, Level>();
        nextLevelLocation = Vector3.zero;

        EventSystem.Current.RegisterEventListener<GenerateNextLevelContext>(OnEnterNextLevelEvent);

        GenerateLevel(Vector3.zero);
    }

    public void GenerateLevel(Vector3 levelLocation)
    {
        Level level = new Level();
        int numTerminals = Levels.Count == 0 ? 3 : random.Next(1, 4);

        Vector2[] directions = {Vector2.up, Vector2.right, Vector2.down, Vector2.left};
        List<Vector2> roomLocations = new List<Vector2>();
        List<Vector2> terminalRoomLocations = new List<Vector2>();
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

        while(terminalRoomLocations.Count < numTerminals)
        {
            Vector2 randRoom = roomLocations[random.Next(roomLocations.Count)];
            if(randRoom != Vector2.zero)
            {
                terminalRoomLocations.Add(randRoom);
            }
        }

        foreach(Vector2 roomLocation in roomLocations)
        {
            GameObject roomPrefab = RoomPrefabs[random.Next(RoomPrefabs.Length)];
            GameObject roomObject = Instantiate(roomPrefab);
            Vector2 realPosition = roomLocation * ROOM_SIZE;
            roomObject.transform.position = levelLocation + new Vector3(realPosition.x, 0, realPosition.y);
            Room room = roomObject.GetComponent<Room>();
            level.Rooms.Add(room);

            if(terminalRoomLocations.Contains(roomLocation))
            {
                GameObject terminalObject = Instantiate(TerminalPrefab);
                GameObject terminalPlacementObject = room.TerminalLocations[random.Next(room.TerminalLocations.Length)];
                terminalObject.transform.position = terminalPlacementObject.transform.position;
                terminalObject.transform.rotation = terminalPlacementObject.transform.rotation;
                Terminal terminal = terminalObject.GetComponent<Terminal>();
                terminal.LinkedLevelLocation = LEVEL_OFFSET * NextLevelLocation;
                level.Terminals.Add(terminal);
            }
        }

        Levels.Add(levelLocation, level);
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

    public void OnEnterNextLevelEvent(GenerateNextLevelContext context)
    {
        if(!Levels.ContainsKey(context.location))
        {
            GenerateLevel(context.location);
        }

        EventSystem.Current.FireEvent(new GotoNextLevelContext(context.location));
    }
}
