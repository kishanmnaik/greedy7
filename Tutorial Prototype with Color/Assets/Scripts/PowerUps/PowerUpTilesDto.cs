using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpTilesDto
{
    private Tile[] tileArray;
    private Tile[] previousTileArray;
    private int tileIdToBeBlocked = -1;
    private int tileIdToBeExploded = -1;
    private bool reverse = false;

    public PowerUpTilesDto(Tile[] tileArray, Tile[] previousTileArray, int tileIdToBeBlocked, int tileIdToBeExploded, bool reverse)
    {
        this.tileArray = tileArray;
        this.previousTileArray = tileArray;
        this.tileIdToBeBlocked = tileIdToBeBlocked;
        this.tileIdToBeExploded = tileIdToBeExploded;
        this.reverse = reverse;
    }

    public Tile[] TileArray { get; set; }

    public Tile[] PreviousTileArray { get; set; }

    public int TileIdToBeBlocked { get; set; }

    public int TileIdToBeExploded { get; set; }

    public bool Reverse { get; set; }

}
