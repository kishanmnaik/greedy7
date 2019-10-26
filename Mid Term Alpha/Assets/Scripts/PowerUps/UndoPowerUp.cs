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
        return powerUpTilesDto;
    }
}
