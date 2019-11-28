using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this service class is not generic, its tightly coupled with type of grid laout game is using at the moment
public static class GexGridService 
{
    // converts hexcoordinates to pixel coordinates
    public static Vector2 HexOffsetCoordToPixel(Contexts contexts, HexOffsetCoord hexOffset)
    {
        HorizontalRowLayout gridLayout  = contexts.game.hexGridLayoutEntity.hexGridLayout.m_layoutType; // unique entity
        float gridCellSize = contexts.game.hexCellRadiusComponetEntity.hexCellRadiusComponet.value;  // unique entity

        Vector2 pixelPosition = new Vector2();

        switch (gridLayout)
        {
            case HorizontalRowLayout.Even:
                pixelPosition = HexOffsetCoord.EvenROffsetToPixel(gridCellSize, hexOffset);
                break;
            case HorizontalRowLayout.Odd:
                pixelPosition = HexOffsetCoord.OddROffsetToPixel(gridCellSize, hexOffset);
                break;
            default:
                break;
        }

        return pixelPosition;
    }

    // returns all neighbour based on the input coord
    public static HexOffsetCoord GetNeightbourCoord(Contexts contexts, HexOffsetCoord hexOffset, int direction)
    {
        HorizontalRowLayout gridLayout = contexts.game.hexGridLayoutEntity.hexGridLayout.m_layoutType; // unique entity
        float gridCellSize = contexts.game.hexCellRadiusComponetEntity.hexCellRadiusComponet.value;  // unique entity

        HexOffsetCoord neighbour = new HexOffsetCoord();

        switch (gridLayout)
        {
            case HorizontalRowLayout.Even:
                neighbour = HexOffsetCoord.EvenROffsetNeighbor(hexOffset, direction);
                break;
            case HorizontalRowLayout.Odd:
                neighbour = HexOffsetCoord.OddROffsetNeighbor(hexOffset, direction);
                break;
            default:
                break;
        }

        return neighbour;
    }

    // returns all 6 neighbour coordinates
    public static List<HexOffsetCoord> GetAllNeightbourCoords(Contexts contexts, HexOffsetCoord hexOffset)
    {
        List<HexOffsetCoord> neighbourCoords = new List<HexOffsetCoord>();
        for (int i = 0; i < 6; i++) // 6 neighbour 
        {
            neighbourCoords.Add(GetNeightbourCoord(contexts, hexOffset,i));

        }
        return neighbourCoords;
    }
}
