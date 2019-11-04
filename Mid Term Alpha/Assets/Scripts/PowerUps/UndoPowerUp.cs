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

        Debug.Log("   in use fun A" + powerUpTilesDto.PreviousScoreA);
        Debug.Log("   in use fun  B" + powerUpTilesDto.PreviousScoreB);

        int size = powerUpTilesDto.TileArray.Length;
        for (int i = 0; i < size ; i++)
        {
            int prevVal = powerUpTilesDto.PreviousTileArray[i].getVal();
            powerUpTilesDto.TileArray[i].setVal(prevVal);
        }

        powerUpTilesDto.ScoreA = powerUpTilesDto.PreviousScoreA;
        powerUpTilesDto.ScoreB = powerUpTilesDto.PreviousScoreB;

        Debug.Log("   in use fun after A" + powerUpTilesDto.ScoreA);
        Debug.Log("   in use fun after B" + powerUpTilesDto.ScoreB);

        return powerUpTilesDto;
    }
}
