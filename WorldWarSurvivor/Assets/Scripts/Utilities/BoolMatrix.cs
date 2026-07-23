using System;
using UnityEngine;

[Serializable]
public class BoolMatrix
{
    public int width = 3;
    public int height = 3;
    public BoolRow[] rows;
}

[Serializable]
public struct BoolRow
{
    public bool[] cols;
}
