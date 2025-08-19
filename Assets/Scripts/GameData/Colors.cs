using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EColors
{
    Red = 0, 
    Green = 1, 
    Blue = 2, 
    Yellow = 4,
    Orange = 5,
    Purple = 6,
    Pink = 7,
    Black = 8
}
[CreateAssetMenu(fileName = "ColorSet", menuName = "Color/ColorSet", order = 1)]
public class ColorSet : ScriptableObject
{
    public Material[] materials;
}
