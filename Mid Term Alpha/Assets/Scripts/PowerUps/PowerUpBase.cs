using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PowerUpBase
{
    string GetType();

    PowerUpTilesDto Use(PowerUpTilesDto powerUpTilesDto);
}
