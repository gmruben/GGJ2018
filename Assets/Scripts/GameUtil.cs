﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtil
{
    public static float ScreenXRange = 2.5F;
    public static float explosionRadius = 1.5f;
    public static float ExplosionRadiusSquared { get { return explosionRadius * explosionRadius; } }

    public static Color GetColor(WaveColour colour)
    {
        if (colour == WaveColour.Red) return ColorPalette.instance.red;
        else if (colour == WaveColour.Blue) return ColorPalette.instance.blue;
        else if (colour == WaveColour.Yellow) return ColorPalette.instance.yellow;
        else if (colour == WaveColour.Purple) return ColorPalette.instance.purple;
        else if (colour == WaveColour.Green) return ColorPalette.instance.green;
        else if (colour == WaveColour.Orange) return ColorPalette.instance.orange;

        return Color.white;
    }

    public static WaveColour GetCombinedColor(WaveColour c1, WaveColour c2)
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

    public static WaveColour RandomColor
    {
        get
        {
            int rand = Random.Range(0, 3);
            switch (rand)
            {
                case 0: return WaveColour.Green;
                case 1: return WaveColour.Purple;
                case 2: return WaveColour.Orange;
            }
            return WaveColour.Green;
        }
    }

    private static float timefactor_min = 1.0F, timefactor_max = 2.5F;
    private static float timefactor_current = 0.0F;
    public static float TimeFactor
    {
        get{
            timefactor_current = Mathf.Clamp(timefactor_current + (Time.deltaTime / 5), timefactor_min, timefactor_max);
            return timefactor_current;
            }
    }

    private static float minSpeed = 0.5F, maxSpeed = 1.5F;
    public static float RandomSpeed
    {
        get
        {
            return Random.Range(minSpeed, maxSpeed);
        }
    }
}
