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
    // public variables
    public GunData gunData;

    // automatic properties
    public GameObject Model { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Model = Instantiate(GunManager.Current.GetModel(gunData), this.transform);
        Model.SetActive(true);
        Debug.Log(Model.GetComponent<Animator>().enabled = false);
        Model.transform.localPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Model.transform.localPosition);
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
