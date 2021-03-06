using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CallbackEvents;
using UnityEngine.AI;

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
    public GameObject[] WeaponPickupPrefabs;
    public GameObject[] EnemyPrefabs;
    public GameObject[] RoomPrefabs;
    public GameObject StartingRoomPrefab;
    public GameObject WallPrefab;
    public GameObject TerminalPrefab;
    public GameObject DocumentPrefab;
    public int NumRooms = 20;
    public int NumWeaponPickups = 2;
    public int MinDocuments = 3;
    public int MaxDocuments = 7;
    public int MinTerminals = 1;
    public int MaxTerminals = 4;

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
        GameObject parent = new GameObject(levelLocation.ToString());
        parent.AddComponent<NavMeshSurface>();

        Level level = new Level();
        int numTerminals = Levels.Count == 0 ? MaxTerminals : random.Next(MinTerminals, MaxTerminals + 1);
        int numDocuments = random.Next(MinDocuments, MaxDocuments + 1);

        Vector2[] directions = {Vector2.up, Vector2.right, Vector2.down, Vector2.left};
        List<Vector2> roomLocations = new List<Vector2>();
        List<Vector2> terminalRoomLocations = new List<Vector2>();
        List<Vector2> weaponRoomLocations = new List<Vector2>();
        List<Vector2> documentRoomLocations = new List<Vector2>();
        List<GameObject> spawnedEnemies = new List<GameObject>();
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

        while(weaponRoomLocations.Count < NumWeaponPickups)
        {
            Vector2 randRoom = roomLocations[random.Next(roomLocations.Count)];
            if(randRoom != Vector2.zero && !terminalRoomLocations.Contains(randRoom))
            {
                weaponRoomLocations.Add(randRoom);
            }
        }

        while(documentRoomLocations.Count < numDocuments)
        {
            Vector2 randRoom = roomLocations[random.Next(roomLocations.Count)];
            if(randRoom != Vector2.zero && !terminalRoomLocations.Contains(randRoom) && !weaponRoomLocations.Contains(randRoom))
            {
                documentRoomLocations.Add(randRoom);
            }
        }

        foreach(Vector2 roomLocation in roomLocations)
        {
            GameObject roomPrefab;
            if(roomLocation == Vector2.zero)
            {
                roomPrefab = StartingRoomPrefab;
            }
            else
            {
                roomPrefab = RoomPrefabs[random.Next(RoomPrefabs.Length)];
            }
            GameObject roomObject = Instantiate(roomPrefab, parent.transform);
            Vector2 realPosition = roomLocation * ROOM_SIZE;
            roomObject.transform.position = levelLocation + new Vector3(realPosition.x, 0, realPosition.y);
            foreach(Vector2 direction in directions)
            {
                if(!roomLocations.Contains(roomLocation + direction))
                {
                    GameObject wallObject = Instantiate(WallPrefab, parent.transform);
                    wallObject.transform.position = roomObject.transform.position + new Vector3(24, 2.5f, -24) + new Vector3(direction.x, 0, direction.y) * 24;
                    if(direction == Vector2.up || direction == Vector2.down)
                    {
                        wallObject.transform.Rotate(Vector3.up, 90);
                    }
                }
            }

            Room room = roomObject.GetComponent<Room>();
            level.Rooms.Add(room);

            if(terminalRoomLocations.Contains(roomLocation))
            {
                GameObject terminalObject = Instantiate(TerminalPrefab, parent.transform);
                GameObject terminalPlacementObject = room.TerminalLocations[random.Next(room.TerminalLocations.Length)];
                terminalObject.transform.position = terminalPlacementObject.transform.position;
                terminalObject.transform.rotation = terminalPlacementObject.transform.rotation;
                Terminal terminal = terminalObject.GetComponent<Terminal>();
                terminal.LinkedLevelLocation = LEVEL_OFFSET * NextLevelLocation;
                level.Terminals.Add(terminal);
            }

            if(weaponRoomLocations.Contains(roomLocation))
            {
                GameObject weaponPickupObject = Instantiate(WeaponPickupPrefabs[random.Next(WeaponPickupPrefabs.Length)]);
                GameObject weaponPlacementObject = room.TerminalLocations[random.Next(room.TerminalLocations.Length)];
                weaponPickupObject.transform.position = weaponPlacementObject.transform.position - weaponPickupObject.GetComponent<SphereCollider>().center + Vector3.up;
            }

            if(documentRoomLocations.Contains(roomLocation))
            {
                GameObject documentPickupObject = Instantiate(DocumentPrefab);
                GameObject documentPlacementObject = room.TerminalLocations[random.Next(room.TerminalLocations.Length)];
                documentPickupObject.transform.position = documentPlacementObject.transform.position + Vector3.up;
            }

            if(roomLocation != Vector2.zero)
            {
                GameObject EnemyObject = Instantiate(EnemyPrefabs[random.Next(EnemyPrefabs.Length)]);
                GameObject EnemyPlacementObject = room.SpawnLocations[random.Next(room.SpawnLocations.Length)];
                EnemyObject.transform.position = EnemyPlacementObject.transform.position;
                if(room.SpawnLocations.Length > 1)
                {
                    GameObject AnotherEnemyObject = Instantiate(EnemyPrefabs[random.Next(EnemyPrefabs.Length)]);
                    GameObject AnotherEnemyPlacementObject;
                    do
                    {
                        AnotherEnemyPlacementObject = room.SpawnLocations[random.Next(room.SpawnLocations.Length)];
                    } while(AnotherEnemyPlacementObject == EnemyPlacementObject);
                    AnotherEnemyObject.transform.position = AnotherEnemyPlacementObject.transform.position;
                    spawnedEnemies.Add(AnotherEnemyObject);
                }

                spawnedEnemies.Add(EnemyObject);
            }
        }

        parent.GetComponent<NavMeshSurface>().BuildNavMesh();
        foreach(GameObject enemy in spawnedEnemies)
        {
            enemy.GetComponent<NavMeshAgent>().enabled = true;
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

        EventSystem.Current.FireEvent(new GotoNextLevelContext(context.location + new Vector3(ROOM_SIZE / 2, 3, -ROOM_SIZE / 2)));
    }
}
