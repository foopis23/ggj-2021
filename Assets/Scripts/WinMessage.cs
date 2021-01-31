using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinMessage : TerminalController
{
    private int score;
    private string message;

    void Start()
    {
        Init();
        score = GameObject.FindGameObjectWithTag("Score Holder").GetComponent<ScoreHolder>().Score;
        if(score == 0)
        {
            message = "... nothing. I'm making a note of this on your permanent record.";
        }
        else if(score >= 10)
        {
            message = $" {score} documents. Not bad.";
        }
        else
        {
            message = $" {score} document{(score == 1 ? "" : "s")}. You're going to have to do better than that if you want to keep your job.";
        }

        setCursorPos(0, 0);
        write("You made it back with" + message);
    }

    // Update is called once per frame
    void Update()
    {
        Run();

        if (Input.GetKeyDown("return") || Input.GetKeyDown("space"))
        {
            SceneManager.LoadScene(0);
        }
    }
}
