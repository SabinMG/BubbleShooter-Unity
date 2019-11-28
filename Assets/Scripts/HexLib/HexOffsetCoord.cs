using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// note - code reference https://www.redblobgames.com/grids/hexagons/implementation.html#offset

public struct HexOffsetCoord 
{
    public HexOffsetCoord (int row, int col)
    {
        this.col = col;
        this.row = row;
    }

    public readonly int col;
    public readonly int row;
    static public int EVEN = 1;
    static public int ODD = -1;

    static public Vector2[][] OddRdirections = new Vector2[][]
    {
        new Vector2 [] {new Vector2(0,  +1), new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(0 ,- 1), new Vector2 (+1, - 1), new Vector2(+1, 0)},
        new Vector2 [] {new Vector2(0, +1), new Vector2(-1, +1), new Vector2(-1, 0), new Vector2 (0, -1), new Vector2(+1, 0), new Vector2(+1, +1)}
    };

    static public Vector2[][] EvenRdirections = new Vector2[][]
    {
       new Vector2 [] {new Vector2(0, +1), new Vector2(-1, +1), new Vector2(-1, 0), new Vector2 (0, -1), new Vector2(+1, 0), new Vector2(+1, +1)},
       new Vector2 [] {new Vector2(0,  +1), new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(0 ,- 1), new Vector2 (+1, - 1), new Vector2(+1, 0)}
    };

    static public Vector2 OddROffsetToPixel(float radius, HexOffsetCoord off)
    {
        double x = radius * Mathf.Sqrt(3) * (off.col + 0.5f * ((off.row & 1))); // shift all odd rows by .5
        double y = radius * 3 / 2 * off.row;
        return new Vector2((float)x, (float)y);
    }

    static public Vector2 EvenROffsetToPixel(float radius, HexOffsetCoord off)
    {
        int even = off.row % 2 == 0? 1: 0;
        double x = radius * Mathf.Sqrt(3) * (off.col + .5f * even);// shift all even rows by .5
        double y = radius * 3 / 2 * off.row;
        return new Vector2((float)x, (float)y);
    }

    static public HexOffsetCoord OddROffsetNeighbor(HexOffsetCoord offC, int direction)
    {
        var parity = offC.row & 1;
        var dir = OddRdirections[parity][direction];
        return new HexOffsetCoord(offC.row + (int)dir.x, offC.col + (int)dir.y);
    }

    static public HexOffsetCoord EvenROffsetNeighbor(HexOffsetCoord offC, int direction)
    {
        var parity = offC.row & 1;
        var dir = EvenRdirections[parity][direction];
        return new HexOffsetCoord(offC.row + (int)dir.x, offC.col + (int)dir.y);
    }
}
