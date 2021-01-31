using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CallbackEvents;

public class PickupItemContext : EventContext
{
    public ItemPickup pickup;

    public PickupItemContext(ItemPickup pickup)
    {
        this.pickup = pickup;
    }
}
public class ItemPickup : MonoBehaviour, IInteractable
{
    // public variables
    public float RotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, RotationSpeed, 0));
    }

    public void OnInteractable()
    {
        //TODO: show UI prompt
    }

    public void Interact()
    {
        EventSystem.Current.FireEvent(new PickupItemContext(this));
        Destroy(gameObject);
    }
}
