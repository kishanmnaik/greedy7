using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapPowerUp : PowerUpBase
{

    public string GetType()
    {
        return "swap";
    }

    public PowerUpTilesDto Use(PowerUpTilesDto powerUpTilesDto) {
        int size = powerUpTilesDto.TileArray.Length;
        for (int i = 0, j = size - 1; i < size / 2; i++, j--)
        {
            if (!powerUpTilesDto.TileArray[i].getIsBlocked() && !powerUpTilesDto.TileArray[j].getIsBlocked())
            {
                var tempValue = powerUpTilesDto.TileArray[i].getVal();
                powerUpTilesDto.TileArray[i].setVal(powerUpTilesDto.TileArray[j].getVal());
                powerUpTilesDto.TileArray[j].setVal(tempValue);
            }
        }

        return powerUpTilesDto;
    }
}
