using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // OnClick function for the tile 
    public void clickFunction()
    {
        // Sends index of the tile to the TileManager Script
        TileManager.updateScore(index);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
