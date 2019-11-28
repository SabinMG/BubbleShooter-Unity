using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public static class GameContextExtensions
{
    public static GameEntity GetNeighbourWithCellValue(this GameContext context, Vector2Int value)
    {
        return ((PrimaryEntityIndex<GameEntity, Vector2Int>)context.GetEntityIndex("HexCell")).GetEntity(value);
    }
}
