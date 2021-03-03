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
        ResetModel();
    }

    public void ResetModel()
    {
        Model = Instantiate(GunManager.Current.GetModel(gunData), this.transform);
        Model.SetActive(true);
        Model.transform.localPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnInteractable()
    {
        EventSystem.Current.FireEvent(new ShowInteractionDialogueContext("Pick Up (E)"));
    }

    public void Interact()
    {
        EventSystem.Current.FireEvent(new PickupWeaponContext(this));
        Destroy(Model);
        ResetModel();
    }
}
