using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinMessage : TerminalController
{
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        Run();

        if (consoleElements.Count < 1)
        {
            if (Input.GetKeyDown("return") || Input.GetKeyDown("space"))
            {
                SceneManager.LoadScene(0);
            }

        }
    }
}
