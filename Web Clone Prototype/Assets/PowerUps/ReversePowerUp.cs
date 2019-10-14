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

    public void Use() {
        reverse = !reverse;
    }
}
