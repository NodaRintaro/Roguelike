using System;
using System.Collections.Generic;
using UnityEngine;

static public class MapData
{
    [SerializeField] private static int _gridSize;

    public static int GridSize => _gridSize;
}
