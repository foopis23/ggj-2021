using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public GameObject ModelPistol;
    public GameObject ModelAssault;
    public GameObject ModelShotgun;
    public GameObject ModelLauncher;

    void Awake()
    {
        __Current = this;
    }

    private static GunManager __Current;
    public static GunManager Current
    {
        get
        {
            if (__Current == null)
            {
                __Current = GameObject.FindObjectOfType<GunManager>();
            }

            return __Current;
        }
    }

    public GameObject GetModel(GunData gunData)
    {
        switch(gunData.Model)
        {
            case "Pistol":
                return ModelPistol;

            case "Assault":
                return ModelAssault;

            case "Shotgun":
                return ModelShotgun;

            case "Launcher":
                return ModelLauncher;
            
            default:
                return null;
        }
    }
}
