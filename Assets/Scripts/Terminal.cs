using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CallbackEvents;

public class Terminal : MonoBehaviour
{
    // Automatic Properties
    public Vector3 LinkedLevelLocation { get; set; }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleInteract()
    {
        //TODO: cool terminal camera zoom text interface puzzle hacking minigame desu

        EventSystem.Current.FireEvent(new GenerateNextLevelContext(LinkedLevelLocation));
    }
}
