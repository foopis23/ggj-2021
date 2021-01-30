﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : TerminalController
{
    private bool displayedMenu;
    private CursorPos[] menuCursorLocations = new CursorPos[] {
        new CursorPos(13, 13),
        new CursorPos(13, 14),
        new CursorPos(13, 15),
        new CursorPos(13, 16)
    };

    int menuIndex = 0;

    void Start()
    {
        Init();
    }

    private void drawMenuCursor(int di)
    {
        setCursorPos(menuCursorLocations[menuIndex].x, menuCursorLocations[menuIndex].y);
        write(' ');
        int next = menuIndex + di;
        if (next < 0) {
            menuIndex = menuCursorLocations.Length + next;
        }else{
            menuIndex = next % menuCursorLocations.Length;
        }
        setCursorPos(menuCursorLocations[menuIndex].x, menuCursorLocations[menuIndex].y);
        write('>');
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        if (consoleElements.Count < 1)
        {
            if (!displayedMenu)
            {
                setCursorPos(15, 10);
                write("Title of our game");
                setCursorPos(15, 11);
                write("This is the epic subtitle of josh's game");
                setCursorPos(15, 13);
                write("Enter Cyberspace\n");
                setCursorPos(15, 14);
                write("Upgrades\n");
                setCursorPos(15, 15);
                write("Settings\n");
                setCursorPos(15, 16);
                write("Quit\n");
                drawMenuCursor(0);
            }

            if (Input.GetKeyDown("s")) {
                drawMenuCursor(1);
            }

            if (Input.GetKeyDown("w")) {
                drawMenuCursor(-1);
            }

            if (Input.GetKeyDown("return")) {
                if (menuIndex == 0) {
                    SceneManager.LoadScene(1);
                }
            }
        }
    }
}