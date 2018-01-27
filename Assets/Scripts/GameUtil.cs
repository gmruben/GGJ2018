using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtil
{
    public static float explosionRadius = 1.5f;
    public static float ExplosionRadiusSquared { get { return explosionRadius * explosionRadius; } }

    public static Color GetColor(WaveColour colour)
    {
        if (colour == WaveColour.Red) return Color.red;
        else if (colour == WaveColour.Blue) return Color.blue;
        else if (colour == WaveColour.Yellow) return Color.yellow;
        else if (colour == WaveColour.Purple) return new Color(0.5f, 0.0f, 1.0f);
        else if (colour == WaveColour.Green) return Color.green;
        else if (colour == WaveColour.Orange) return new Color (1.0f, 0.5f, 0.0f);

        return Color.white;
    }

    public static WaveColour GetCombinedColor (WaveColour c1, WaveColour c2)
    {
        if (c1 == WaveColour.Red)
        {
            if (c2 == WaveColour.Red) return WaveColour.Red;
            else if (c2 == WaveColour.Blue) return WaveColour.Purple;
            else if (c2 == WaveColour.Yellow) return WaveColour.Orange;
        }
        else if (c1 == WaveColour.Blue)
        {
            if (c2 == WaveColour.Red) return WaveColour.Purple;
            else if (c2 == WaveColour.Blue) return WaveColour.Blue;
            else if (c2 == WaveColour.Yellow) return WaveColour.Green;
        }
        else if (c1 == WaveColour.Yellow)
        {
            if (c2 == WaveColour.Red) return WaveColour.Orange;
            else if (c2 == WaveColour.Blue) return WaveColour.Green;
            else if (c2 == WaveColour.Yellow) return WaveColour.Yellow;
        }

        return WaveColour.None;
    }
}
