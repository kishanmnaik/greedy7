using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpTilesDto
{
    private Tile[] tileArray;
    private int tileIdToBeBlocked = -1;
    private bool reverse = false;

    public PowerUpTilesDto(Tile[] tileArray, int tileIdToBeBlocked, bool reverse)
    {
        this.tileArray = tileArray;
        this.tileIdToBeBlocked = tileIdToBeBlocked;
        this.reverse = reverse;
    }

    public Tile[] TileArray { get; set; }

    public int TileIdToBeBlocked { get; set; }

    public bool Reverse { get; set; }

}
