using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathError : TerminalController
{
    public Image image;
    public float opacityDamp;
    private float opacity;

    // Start is called before the first frame update
    void Start()
    {
        opacity = 0;
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        if (consoleElements.Count < 1) {
            opacity = Mathf.Lerp(opacity, 1.0f, opacityDamp * Time.deltaTime);
            image.color = new Color(image.color.r, image.color.g, image.color.b, opacity);

            if (opacity > 0.99) {
                SceneManager.LoadScene(0);
            }
        }
    }
}
