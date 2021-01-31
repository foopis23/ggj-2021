using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialMessage : TerminalController
{
    public bool Enabled = false;

    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if(Enabled)
        {
            Run();

            if (consoleElements.Count < 1)
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}
