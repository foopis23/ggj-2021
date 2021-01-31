using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class CursorPos
{
    public int x;
    public int y;

    public CursorPos()
    {
        x = 0;
        y = 0;
    }

    public CursorPos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

[System.Serializable]
public class ConsoleElement
{
    public string text;
    public bool isSlowType;
    public float slowTypeDeltaTime;
    public float waitAfterTime;
    protected bool wrote;
    protected bool isHandled;
    public bool IsHandled
    {
        get
        {
            return isHandled;
        }
    }
    protected Queue<char> slowTypeQueue;
    protected float lastSlowType;

    public void Init()
    {
        text = text.Replace("\\n", "\n");

        if (isSlowType)
        {
            lastSlowType = Time.time;
            slowTypeQueue = new Queue<char>();
            foreach (char c in text)
            {
                slowTypeQueue.Enqueue(c);
            }
        }
    }

    public void Update(TerminalController controller)
    {
        if (isSlowType)
        {
            if (slowTypeQueue.Count > 0)
            {
                if (Time.time - lastSlowType >= slowTypeDeltaTime)
                {
                    controller.write(slowTypeQueue.Dequeue());
                    lastSlowType = Time.time;
                }
            }
            else if (Time.time - lastSlowType >= waitAfterTime)
            {
                isHandled = true;
            }
        }
        else
        {
            if (!wrote)
            {
                controller.write(text);
                wrote = true;
                lastSlowType = Time.time;
            }
            else if (Time.time - lastSlowType >= waitAfterTime)
            {
                isHandled = true;
            }
        }
    }
}

public class TerminalController : MonoBehaviour
{
    public int terminalWidth = 120;
    public int terminalHeight = 60;
    public TMP_Text GUI;
    public ConsoleElement[] ConsoleElements;
    protected Queue<ConsoleElement> consoleElements;
    protected CursorPos cursorPos;
    protected StringBuilder outputBuffer;
    protected bool displayIsDirty;

    // Start is called before the first frame update
    public void Init()
    {
        outputBuffer = new StringBuilder();
        cursorPos = new CursorPos();
        displayIsDirty = true;
        consoleElements = new Queue<ConsoleElement>(ConsoleElements);
        resetBuffer();

        if (consoleElements.Count > 0) consoleElements.Peek().Init();
    }

    public void Run()
    {
        if (displayIsDirty)
        {
            GUI.text = outputBuffer.ToString();
            displayIsDirty = false;
        }

        if (consoleElements.Count > 0)
        {
            consoleElements.Peek().Update(this);

            if (consoleElements.Peek().IsHandled)
            {
                consoleElements.Dequeue();
                if (consoleElements.Count > 0)
                {
                    consoleElements.Peek().Init();
                }
            }
        }
    }

    private string arrayToString(string[] text)
    {
        string output = "";

        foreach (string line in text)
        {
            output += line + '\n';
        }

        return output;
    }

    private void resetBuffer()
    {
        for (int y = 0; y < terminalHeight; y++)
        {
            for (int x = 0; x < terminalWidth; x++)
            {
                outputBuffer.Append(' ');
            }
            outputBuffer.Append('\n');
        }
    }

    public void setCursorPos(int x, int y)
    {
        cursorPos.x = x;
        cursorPos.y = y;
    }

    public int[] getCursorPos()
    {
        return new int[] { cursorPos.x, cursorPos.y };
    }

    public void write(char c)
    {
        if (c == '\n')
        {
            cursorPos.x = 0;
            cursorPos.y++;
        }
        else
        {
            int index = cursorPos.x % (terminalWidth + 1) + cursorPos.y * (terminalWidth + 1);
            outputBuffer[index] = c;

            cursorPos.x++;
            if (cursorPos.x >= terminalWidth)
            {
                cursorPos.y++;
                cursorPos.x = 0;
            }
        }

        displayIsDirty = true;
    }

    public void write(string text)
    {
        foreach (char c in text)
        {
            write(c);
        }
    }

    public void print(string text)
    {
        write(text);
        cursorPos.y++;
        cursorPos.x = 0;
    }

    public void clear()
    {
        resetBuffer();
        cursorPos.x = 0;
        cursorPos.y = 0;
        displayIsDirty = true;
    }
}
