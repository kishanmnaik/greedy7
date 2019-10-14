using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPowerUp : PowerUpBase
{
    // TODO: GET INDEX, GRIDARRAY AND TILESTATEARRAY FROM GRIDMAIN
    int index = 0;
    int[] gridArray;
    bool[] tileStateArray;

    public string GetType()
    {
        return "block";
    }

    public PowerUpTilesDto Use(PowerUpTilesDto powerUpTilesDto)
    {
        for (int i = 0; i < tileStateArray.Length; i++)
        {
            if (i == index)
            {
                tileStateArray[i] = false;
            } else
            {
                tileStateArray[i] = true;
            }
        }

        return powerUpTilesDto;
    }
}
