﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CallbackEvents;
using System.Threading.Tasks;

public class UpdateHUDContext : EventContext {
    public float rotation;

    public UpdateHUDContext(float rotation) {
        this.rotation = rotation;
    }
}


public class HealthOverlay : MonoBehaviour
{
    public int x = 0;
    public int y = 0;
    public int width = 20;
    public int height = 5;

    public int compassX = 114;
    public int compassY = 1;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.Current.RegisterEventListener<PlayerHealthChangedCtx>(OnHealthChange);
        EventSystem.Current.RegisterEventListener<UpdateHUDContext>(OnUpdateHUD);

        ASCII.Instance.DrawBox('#', Color.white, Color.black, x, y, width, height);
        drawHealth(1.0f);

        ASCII.Instance.PutChar('|', Color.white, 60, 29);
        ASCII.Instance.PutChar('-', Color.white, 58, 30);
        ASCII.Instance.PutChar('-', Color.white, 62, 30);
        ASCII.Instance.PutChar('+', Color.white, 60, 30);
        ASCII.Instance.PutChar('|', Color.white, 60, 31);
    }

    void drawHealth(float percent)
    {
        int count = (int)(percent * (width - 2));

        ASCII.Instance.ClearArea(x + 1, y + 1, width - 2, height - 2);
        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                Color color = (x <= count) ? Color.red : Color.black;

                ASCII.Instance.PutChar('#', color, x, y);
            }
        }
    }

    void DrawCompass(float angle)
    {
        ASCII.Instance.ClearArea(compassX, compassY, 3, 3);
        ASCII.Instance.DrawBox('#', Color.white, Color.black, compassX - 1, compassY - 1, 5, 5);
        // ASCII.Instance.PutChar('N', Color.white, compassX + 1, compassY - 1);
        // ASCII.Instance.PutChar('S', Color.white, compassX + 1, compassY + 3);
        // ASCII.Instance.PutChar('E', Color.white, compassX + 3, compassY + 1);
        // ASCII.Instance.PutChar('W', Color.white, compassX - 1, compassY + 1);

        if (angle > -22.5 && angle <= 22.5)
        { //north
            ASCII.Instance.PutChar('|', Color.red, compassX + 1, compassY);
            ASCII.Instance.PutChar('0', Color.white, compassX + 1, compassY + 1);
            ASCII.Instance.PutChar('|', Color.white, compassX + 1, compassY + 2);
        }
        else if (angle > 22.5 && angle <= 67.5)
        {// northeast
            ASCII.Instance.PutString("\\  ", Color.red, compassX, compassY);
            ASCII.Instance.PutString(" 0 ", Color.white, compassX + 1, compassY + 1);
            ASCII.Instance.PutString("  \\", Color.white, compassX + 2, compassY + 2);
        }
        else if (angle > 67.5 && angle <= 112.5)
        { // east
            ASCII.Instance.PutChar('-', Color.red, compassX, compassY + 1);
            ASCII.Instance.PutChar('0', Color.white, compassX + 1, compassY + 1);
            ASCII.Instance.PutChar('-', Color.white, compassX + 2, compassY + 1);
        }
        else if (angle > 112.5 && angle <= 157.5)
        { // southeast
            ASCII.Instance.PutChar('/', Color.white, compassX + 2, compassY);
            ASCII.Instance.PutChar('0', Color.white, compassX + 1, compassY + 1);
            ASCII.Instance.PutChar('/', Color.red, compassX, compassY + 2);
        }
        else if (angle > 157.5 || angle < -157.5)
        { //south
            ASCII.Instance.PutChar('|', Color.white, compassX, compassY);
            ASCII.Instance.PutChar('0', Color.white, compassX, compassY);
            ASCII.Instance.PutChar('|', Color.red, compassX, compassY);
        }
        else if (angle > -67.5 && angle <= -22.5)
        {// northwest

        }
        else if (angle > -112.5 && angle < -67.5)
        { //west 

        }
        else if (angle > -157.5 && angle < -112.5)
        {//southwest dressing

        }
    }

    void OnHealthChange(PlayerHealthChangedCtx ctx)
    {
        float percent = ctx.currentHealth / ctx.player.maxHealth;
        drawHealth(percent);
    }

    void OnUpdateHUD(UpdateHUDContext ctx) {
        DrawCompass(ctx.rotation);
    }
}
