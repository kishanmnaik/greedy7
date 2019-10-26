using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapPowerUp : PowerUpBase
{
    // TODO: GET GRIDARRAY FROM GRIDMAIN
    int[] gridArray;

    public string GetType()
    {
        return "swap";
    }

    public PowerUpTilesDto Use(PowerUpTilesDto powerUpTilesDto) {
        int size = powerUpTilesDto.TileArray.Length;
        for (int i = 0, j = size - 1; i < size / 2; i++, j--)
        {
            gridArray[i] += gridArray[j];
            gridArray[j] = gridArray[i] - gridArray[j];
            gridArray[i] = gridArray[i] - gridArray[j];
        }

        return powerUpTilesDto;
    }
}
