using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CallbackEvents;

public class HealthOverlay : MonoBehaviour
{
    public int x = 0;
    public int y = 0;
    public int width = 20;
    public int height = 5;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.Current.RegisterEventListener<PlayerHealthChangedCtx>(OnHealthChange);
        ASCII.Instance.DrawBox('#', Color.white, Color.black, x, y, width, height);
        drawHealth(1.0f);
        ASCII.Instance.PutChar('#', Color.white, 60, 30);
    }

    void drawHealth(float percent)
    {
        int count = (int)(percent * (width - 2));

        ASCII.Instance.ClearArea(x + 1, y + 1, width - 2, height - 2);
        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < count + 1; x++)
            {
                ASCII.Instance.PutChar('#', Color.red, x, y);
            }
        }
    }

    void OnHealthChange(PlayerHealthChangedCtx ctx)
    {
        float percent = ctx.currentHealth / ctx.player.maxHealth;
        drawHealth(percent);
    }
}
