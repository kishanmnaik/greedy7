using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReversePowerUp : PowerUpBase
{
    // TODO: GET REVERSE FROM GRIDMAIN
    bool reverse = false;

    public string GetType()
    {
        return "reverse";
    }

    public PowerUpTilesDto Use(PowerUpTilesDto powerUpTilesDto)
    {
        powerUpTilesDto.Reverse = !reverse;
        return powerUpTilesDto;
    }
}
