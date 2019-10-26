using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodePowerUp : PowerUpBase
{
    // TODO: GET REVERSE, INDEX AND GRIDARRAY FROM GRIDMAIN
    bool reverse = false;
    int inTile = 0;
    int index = 0;
    int[] gridArray;

    public string GetType()
    {
        return "explode";
    }

    public PowerUpTilesDto Use(PowerUpTilesDto powerUpTilesDto)
    {
        inTile = gridArray[index];
        gridArray[index] = 0;
        if (!reverse)
        {
            while(inTile != 0)
            {
                index = (index + 1) % gridArray.Length;
                gridArray[index]++;
                inTile--;
            }
        }
        else
        {
            while(inTile != 0)
            {
                if (index == 0)
                {
                    index = gridArray.Length - 1;
                }
                else
                {
                    index -= 1;
                }
                gridArray[index]++;
                inTile--;
            }
        }

        return powerUpTilesDto;
    }
}
