using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPowerUp : PowerUpBase
{
    public string GetType()
    {
        return "shield";
    }

    public PowerUpTilesDto Use(PowerUpTilesDto powerUpTilesDto)
    {
        powerUpTilesDto.TileArray[powerUpTilesDto.TileIdToBeBlocked].setBlock();

        for (int i = 0; i < 10; i++)
        {
            Debug.Log("ID: " + i + " : " + powerUpTilesDto.TileArray[i].getIsBlocked());
        }

        return powerUpTilesDto;
    }
}
