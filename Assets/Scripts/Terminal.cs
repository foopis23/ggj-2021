using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CallbackEvents;

public class Terminal : MonoBehaviour, IInteractable
{
    // public variables
    public bool Return = false;

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

    public void OnInteractable()
    {
        //TODO: show UI prompt
        EventSystem.Current.FireEvent(new ShowInteractionDialogueContext("Use (E)"));
    }

    public void Interact()
    {
        //TODO: cool terminal camera zoom text interface puzzle hacking minigame piss baby desu

        if(Return)
        {
            EventSystem.Current.FireEvent(new GotoPreviousLevelContext());
        }
        else
        {
            EventSystem.Current.FireEvent(new GenerateNextLevelContext(LinkedLevelLocation));
        }
    }
}