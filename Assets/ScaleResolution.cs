using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleResolution : MonoBehaviour
{
    public const float screenRatio = 140.0f / 1440.0f;
    public ASCII ascii;

    private float lastHeight;
    
    void Awake() {
        if (ascii == null)
            ascii = GetComponent<ASCII>();

        lastHeight = Screen.height;
        float rows = (float)Screen.height * screenRatio;
        float cols = rows * 2;
        ascii.rows = (uint)rows;
        ascii.columns = (uint)cols;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastHeight != Screen.height)
        {
            lastHeight = Screen.height;
            float rows = (float)Screen.height * screenRatio;
            float cols = rows * 2;
            ascii.rows = (uint)rows;
            ascii.columns = (uint)cols;
        }
    }
}
