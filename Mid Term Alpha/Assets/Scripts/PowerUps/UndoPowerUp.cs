using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoPowerUp : PowerUpBase
{
    public string GetType()
    {
        return "undo";
    }

    public PowerUpTilesDto Use(PowerUpTilesDto powerUpTilesDto)
    {
        for(int i=0; i<powerUpTilesDto.TileArray.Length; i++)
        {
            powerUpTilesDto.TileArray[i] = powerUpTilesDto.PreviousTileArray[i];
        }
        return powerUpTilesDto;
    }
}
