using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReversePowerUp : PowerUpBase
{
    bool reverse = false;

    public string GetType()
    {
        return "reverse";
    }

    public PowerUpTilesDto Use(PowerUpTilesDto powerUpTilesDto)
    {
        powerUpTilesDto.Reverse = !powerUpTilesDto.Reverse;
        return powerUpTilesDto;
    }
}
