using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime;

public class coinToss
{
    private int toss = 0;
    // Start is called before the first frame update
    void Start()
    {
        var rnd = new System.Random();
        toss = rnd.Next() % 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getTurn()
    {
        Start();
        if(toss == 1)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
}
