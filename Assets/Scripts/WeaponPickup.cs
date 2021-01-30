using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CallbackEvents;

public class PickupWeaponContext : EventContext
{
    public WeaponPickup pickup;

    public PickupWeaponContext(WeaponPickup pickup)
    {
        this.pickup = pickup;
    }
}

public class WeaponPickup : MonoBehaviour, IInteractable
{
    public GunData gunData;

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
    }

    public void Interact()
    {
        EventSystem.Current.FireEvent(new PickupWeaponContext(this));
    }
}
