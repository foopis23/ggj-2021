using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCameraSetter : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            ASCII ascii = other.gameObject.GetComponentInChildren<ASCII>();
            ascii.pixelate = false;
            ascii.tranparency = 1.0f;
        }
    }

        private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            ASCII ascii = other.gameObject.GetComponentInChildren<ASCII>();
            ascii.pixelate = true;
            ascii.tranparency = 0.0f;
        }
    }
}
