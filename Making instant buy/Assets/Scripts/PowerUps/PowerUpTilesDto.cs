using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpTilesDto
{
    public Tile[] TileArray { get; set; }

    public Tile[] PreviousTileArray { get; set; }

    public int TileIdToBeBlocked { get; set; }

    public int TileIdToBeExploded { get; set; }

    public int ScoreA { get; set; }

    public int ScoreB { get; set; }

    public int PreviousScoreA { get; set; }

    public int PreviousScoreB { get; set; }

    public bool Reverse { get; set; }
}
