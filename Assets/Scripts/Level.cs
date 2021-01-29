using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    // Automatic Properties
    public List<Room> Rooms { get; private set; }
    public List<Terminal> Terminals { get; private set; }

    public Level()
    {
        Rooms = new List<Room>();
        Terminals = new List<Terminal>();
    }
}
