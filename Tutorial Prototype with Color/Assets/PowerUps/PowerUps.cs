using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PowerUps
{
    private int maxPowerUpCount = 3;
    private Dictionary<PowerUpBase, bool> powerUps = new Dictionary<PowerUpBase, bool>(); 

    public void Init(List<PowerUpBase> pList)
    {
        /* powerUps.Add(new BlockPowerUp(), true);
        powerUps.Add(new ReversePowerUp(), true);
        powerUps.Add(new BlockPowerUp(), true);
        powerUps.Add(new SwapPowerUp(), false); */
        foreach (var pUp in pList)
        {
            powerUps.Add(pUp, true);
        }
        Debug.Log(this.GetPowerUpCount());
    }

    public int GetPowerUpCount()
    {        
        return powerUps.Where(x => x.Value == true).Count();
    }

    public List<PowerUpBase> GetActivePowerUps()
    {
        return powerUps.Where(x => x.Value == true).Select(x => x.Key).ToList();
    } 

    public bool IsActive(PowerUpBase powerUpBase)
    {
        bool isActive;
        powerUps.TryGetValue(powerUpBase, out isActive);
        return isActive;
    }
}
