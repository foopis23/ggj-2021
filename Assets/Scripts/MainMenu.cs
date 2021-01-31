using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : TerminalController
{
    public TutorialMessage TutorialScreen;
    public Image fadeBackground;
    public float opacityDamp;
    private float opacity;

    private CursorPos[] menuCursorLocations = new CursorPos[] {
        new CursorPos(13, 12),
        new CursorPos(13, 13),
        new CursorPos(13, 14),
        new CursorPos(13, 15),
        new CursorPos(13, 16)
    };

    int menuIndex = 0;
    int displayedMenu = 0;

    void Start()
    {
        Init();
        opacity = 1.0f;
    }

    private void drawMenuCursor(int di)
    {
        setCursorPos(menuCursorLocations[menuIndex].x, menuCursorLocations[menuIndex].y);
        write(' ');
        int next = menuIndex + di;
        if (next < 0)
        {
            menuIndex = menuCursorLocations.Length + next;
        }
        else
        {
            menuIndex = next % menuCursorLocations.Length;
        }
        setCursorPos(menuCursorLocations[menuIndex].x, menuCursorLocations[menuIndex].y);
        write('>');
    }

    private void logClear()
    {
        for (int y = 1; y < 6; y++)
        {
            for (int x = 1; x < 120; x++)
            {
                setCursorPos(x, y);
                write(' ');
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        opacity = Mathf.Lerp(opacity, 0.0f, opacityDamp * Time.deltaTime);
        fadeBackground.color = new Color(fadeBackground.color.r, fadeBackground.color.g, fadeBackground.color.b, opacity);

        Run();
        
        if (consoleElements.Count < 1)
        {
            if (displayedMenu == 0)
            {
                setCursorPos(15, 9);
                write("ITRecovery Employee Hub Terminal");
                setCursorPos(15, 10);
                write("Please select an option:");
                setCursorPos(15, 12);
                write("Enter Cyberspace\n");
                setCursorPos(15, 13);
                write("Upgrades\n");
                setCursorPos(15, 14);
                write("Settings\n");
                setCursorPos(15, 15);
                write("Logs\n");
                setCursorPos(15, 16);
                write("Quit\n");
                drawMenuCursor(0);
            }
            if (displayedMenu == 1)
            {
                setCursorPos(15, 9);
                write("Use those CYBERSPACE points to upgrade the equipment available to you.");
                setCursorPos(15, 10);
                write("Recovery Unit: 177 ");
                setCursorPos(15, 11);
                write("WARNING: Does not work");
                setCursorPos(15, 12);
                write("Gun Damage");
                setCursorPos(15, 13);
                write("Player Health");
                setCursorPos(15, 14);
                write("Health Recovery");
                setCursorPos(15, 15);
                write("Object Spawn Rate");
                setCursorPos(15, 16);
                write("Exit");
                drawMenuCursor(0);
            }
            if (displayedMenu == 2)
            {
                setCursorPos(15, 9);
                write("ITRecovery Cyberspace Settings Recovery Unit: 177");
                setCursorPos(15, 10);
                write("THIS SCREEN IS CURRENTLY NOT OPERATIONAL AND IS ONLY HERE TO MAKE");
                setCursorPos(15, 11);
                write("ITRECOVERY EMPLOYEES FEEL AS IF WE CARE ABOUT THEM");
                setCursorPos(15, 12);
                write("Mouse Sensitivity:   ");
                setCursorPos(15, 13);
                write("Interact Binding:    ");
                setCursorPos(15, 14);
                write("Game Volume:         ");
                setCursorPos(15, 15);
                write("FOV:                 ");
                setCursorPos(15, 16);
                write("Exit");
                drawMenuCursor(0);
            }
            if (displayedMenu == 3)
            {
                setCursorPos(15, 9);
                write("ITRecovery Logs");
                setCursorPos(15, 10);
                write("Recovery Unit: 177");
                setCursorPos(15, 12);
                write("Terminal.txt");
                setCursorPos(15, 13);
                write("ReturnTerminal.txt");
                setCursorPos(15, 14);
                write("Documents.txt");
                setCursorPos(15, 15);
                write("Email from: Boss@ITRecovery.org");
                setCursorPos(15, 16);
                write("Exit");
                drawMenuCursor(0);
            }

            if (Input.GetKeyDown("s") || Input.GetKeyDown("down"))
            {
                drawMenuCursor(1);
            }

            if (Input.GetKeyDown("w") || Input.GetKeyDown("up"))
            {
                drawMenuCursor(-1);
            }

            if (Input.GetKeyDown("return") || Input.GetKeyDown("space"))
            {
                if (displayedMenu == 3)
                {
                    if (menuIndex == 0)
                    {
                        logClear();
                        setCursorPos(1, 1);
                        write("This yellow terminal will send you one layer deeper into CYBERSPACE! The deeper you can go into cyberspace, the more");
                        setCursorPos(1, 2);
                        write("documents YOU can find. The more documents you can find, the more documents WE can return to our customers.");
                        setCursorPos(1, 3);
                        write("This means more money for us. Maybe you too (don't count on it). Get to it!");
                    }
                    if (menuIndex == 1)
                    {
                        logClear();
                        setCursorPos(1, 1);
                        write("This purple terminal does the opposite of the yellow ones. These return you up one level in cyberspace. Use these when");
                        setCursorPos(1, 2);
                        write("you feel as if you can't collect anymore documents.");
                    }
                    if (menuIndex == 2)
                    {
                        logClear();
                        setCursorPos(1, 1);
                        write("This is where the money is baby! Find these sweet white rectangles hidden in CYBERSPACE. Each one increases points");
                        setCursorPos(1, 2);
                        write("which in turn increases profits. WARNING: The more you collect, the more you will be targeted by the hackers.");
                        setCursorPos(1, 4);
                        write("By reading this sentence you agree that ITRecovery is not responsible for any Accidental Loss of Life in the Field.");
                    }
                    if (menuIndex == 3)
                    {
                        logClear();
                        setCursorPos(1, 1);
                        write("UNIT 177:");
                        setCursorPos(1, 3);
                        write("You have been terminated and rehired 59 times. Our company has strict rules againt the rehiring of an employee for the");
                        setCursorPos(1, 4);
                        write("60th time. This is your last chance to prove yourself here. Make it count.");
                        setCursorPos(1, 5);
                        write(" - Your Boss");
                    }
                    if (menuIndex == 4)
                    {
                        clear();
                        displayedMenu = 0;
                    }
                }
                if (displayedMenu == 2)
                {
                    if (menuIndex == 0)
                    {
                        
                    }
                    if (menuIndex == 1)
                    {
                        
                    }
                    if (menuIndex == 2)
                    {
                        
                    }
                    if (menuIndex == 3) 
                    {
                        
                    }
                    if (menuIndex == 4)
                    {
                        clear();
                        displayedMenu = 0;
                    }
                }

                if (displayedMenu == 1)
                {
                    if (menuIndex == 0)
                    {

                    }
                    if (menuIndex == 1)
                    {

                    }
                    if (menuIndex == 2)
                    {

                    }
                    if (menuIndex == 3)
                    {

                    }
                    if (menuIndex == 4)
                    {
                        clear();
                        displayedMenu = 0;
                    }
                }

                if (displayedMenu == 0)
                {
                    if (menuIndex == 0)
                    {
                        TutorialScreen.Enabled = true;
                        gameObject.SetActive(false);
                    }
                    if (menuIndex == 1)
                    {
                        displayedMenu = 1;
                        clear();
                        drawMenuCursor(-1);
                    }
                    if (menuIndex == 2)
                    {
                        displayedMenu = 2;
                        clear();
                        drawMenuCursor(-2);
                    }
                    if (menuIndex == 3)
                    {
                        displayedMenu = 3;
                        clear();
                        drawMenuCursor(-3);
                    }
                    if (menuIndex == 4)
                    {
                        Application.Quit();
                    }
                }
            }

        }
    }
}
