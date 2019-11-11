using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullifyPowerUp : PowerUpBase
{
    public string GetType()
    {
        return "nullify";
    }

    public PowerUpTilesDto Use(PowerUpTilesDto powerUpTilesDto)
    {
        return powerUpTilesDto;
    }
}
