using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPalette : MonoBehaviour
{
    public Color red;
    public Color blue;
    public Color yellow;

    public Color purple;
    public Color green;
    public Color orange;

    public static ColorPalette instance;

    void Awake ()
    {
        instance = this;
    }
}
