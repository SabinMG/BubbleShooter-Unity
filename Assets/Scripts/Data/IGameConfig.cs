using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameConfig 
{
    HorizontalRowLayout hexGridInitLayout { get; }
    Vector2Int hexGridSize { get; }
    float hexCellRadius { get; }
    int bubbleInitRows { get; }
}
