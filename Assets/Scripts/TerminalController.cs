using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

class CursorPos
{
    public int x;
    public int y;

    public CursorPos()
    {
        x = 0;
        y = 0;
    }
}

public class TerminalController : MonoBehaviour
{
    private CursorPos cursorPos;
    private char[,] buffer;
    private bool displayIsDirty;

    public int terminalWidth = 120;
    public int terminalHeight = 60;
    public string[] test;
    public TMP_Text GUI;

    // Start is called before the first frame update
    void Start()
    {
        cursorPos = new CursorPos();
        displayIsDirty = true;
        resetBuffer();
        SlowType(0.001f, arrayToString(test));
    }

    void Update()
    {
        if (displayIsDirty)
        {
            string text = "";
            for (int y = 0; y < buffer.GetLength(0); y++)
            {
                for (int x = 0; x < buffer.GetLength(1); x++)
                {
                    text += buffer[y, x];
                }
            }

            GUI.text = text;
        }
    }

    private string arrayToString(string[] text) {
        string output = "";

        foreach(string line in text) {
            output += line + '\n';
        }

        return output;
    }

    private void resetBuffer()
    {
        if (buffer == null)
        {
            buffer = new char[terminalHeight, terminalWidth];
        }
        for (int y = 0; y < buffer.GetLength(0); y++)
        {
            for (int x = 0; x < buffer.GetLength(1); x++)
            {
                buffer[y, x] = ' ';
            }
        }
    }

    private void write(char c)
    {
        if (c == '\n')
        {
            cursorPos.x = 0;
            cursorPos.y++;
        }
        else
        {
            buffer[cursorPos.y, cursorPos.x] = c;
            cursorPos.x++;
            if (cursorPos.x >= terminalWidth)
            {
                cursorPos.y++;
                cursorPos.x = 0;
            }
        }

        displayIsDirty = true;
    }

    private void write(string text)
    {
        foreach (char c in text)
        {
            write(c);
        }
    }

    private void print(string text)
    {
        write(text);
        cursorPos.y++;
        cursorPos.x = 0;
    }

    private void clear()
    {
        resetBuffer();
        cursorPos.x = 0;
        cursorPos.y = 0;
        displayIsDirty = true;
    }

    private void SlowType(float time, string text)
    {
        StartCoroutine(slowTypeOnDifferentThread(time, text));
    }

    IEnumerator slowTypeOnDifferentThread(float time, string text)
    {
        foreach (char c in text)
        {
            write(c);
            yield return new WaitForSeconds(time);
        }
    }
}
