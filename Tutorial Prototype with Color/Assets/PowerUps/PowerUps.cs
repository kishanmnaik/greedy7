using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PowerUps
{
    private int maxPowerUpCount = 3;
    private Dictionary<PowerUpBase, bool> powerUps = new Dictionary<PowerUpBase, bool>(); 

    public void Init()
    {
        powerUps.Add(new BlockPowerUp(), true);
        powerUps.Add(new ReversePowerUp(), true);
        powerUps.Add(new BlockPowerUp(), true);
        powerUps.Add(new SwapPowerUp(), false);
    }

    public int GetPowerUpCount()
    {        
        return powerUps.Where(x => x.Value == true).Count();
    }

    public bool IsActive(PowerUpBase powerUpBase)
    {
        bool isActive;
        powerUps.TryGetValue(powerUpBase, out isActive);
        return isActive;
    }
}
