using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodePowerUp : PowerUpBase
{
    public string GetType()
    {
        return "explode";
    }

    public PowerUpTilesDto Use(PowerUpTilesDto powerUpTilesDto)
    {
        Debug.Log(powerUpTilesDto.TileIdToBeExploded);
        int valueInTile = powerUpTilesDto.TileArray[powerUpTilesDto.TileIdToBeExploded].getVal();
        int index = powerUpTilesDto.TileIdToBeExploded;
        int gridSize = powerUpTilesDto.TileArray.Length;

        powerUpTilesDto.TileArray[index].setVal(0);
        if (powerUpTilesDto.Reverse == false)
        {
            while(valueInTile != 0)
            {
                index = (index + 1) % gridSize;
                powerUpTilesDto.TileArray[index].setVal(powerUpTilesDto.TileArray[index].getVal() + 1);
                valueInTile--;
            }
        }
        else
        {
            while (valueInTile != 0)
            {
                if(index == 0)
                {
                    index = gridSize - 1;
                }
                else
                {
                    index = index - 1;
                }
                powerUpTilesDto.TileArray[index].setVal(powerUpTilesDto.TileArray[index].getVal() + 1);
                valueInTile--;
            }
        }
        return powerUpTilesDto;
    }
}
