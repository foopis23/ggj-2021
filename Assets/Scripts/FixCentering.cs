using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixCentering : MonoBehaviour
{
    public GameObject prefab;
    public float dx;
    public float dy;
    public float dz;

    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer[] meshs = prefab.GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer mesh in meshs) {
            Vector3 old = mesh.gameObject.transform.position;
            mesh.gameObject.transform.position = old - new Vector3(dx, dy, dz);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
