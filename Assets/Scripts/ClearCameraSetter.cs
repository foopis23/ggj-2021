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
            ASCII.Instance.pixelate = false;
            ASCII.Instance.tranparency = 1.0f;
        }
    }

        private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            ASCII.Instance.pixelate = true;
            ASCII.Instance.tranparency = 0.0f;
        }
    }
}
