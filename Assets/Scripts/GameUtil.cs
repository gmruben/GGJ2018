using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtil
{
    public static float explosionRadius = 1.5f;

    public static Color GetColor(ExplosionColour colour)
    {
        if (colour == ExplosionColour.Blue) return Color.blue;
        else if (colour == ExplosionColour.Red) return Color.red;

        return Color.white;
    }
}
