using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class TrackingAnalytics : MonoBehaviour
{
    /* public int totalTurns = 0;
    public float totalTime = 0.0f;
    public string powerUpList = ""; */

    private string path = "Assets/Tracking/t_beta_111119.csv";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // int totalTurns, float totalTime, string powerUpList, string usedPowerUpList
    public void updateFile(int totalTurns, int totalTime, string powerUpList, string usedPowerUpList)
    {
        string line = "";
        int gameCount = 1; 
        try
        {
            StreamReader sr = new StreamReader(path);
            line = sr.ReadLine();
            while(true)
            {
                line = sr.ReadLine();
                if (line == null)
                    break;
                gameCount++;
                Debug.Log(line);
            }

            sr.Close();
        } catch (Exception e)
        {
            Debug.Log("READ: File Does Not Exist.");
        }

        try
        {
            StreamWriter sw = new StreamWriter(path, true);
            sw.Write((gameCount+1) + "," + totalTurns + "," + totalTime + "," + powerUpList + "," + usedPowerUpList + "\n");
            sw.Close();
        } catch (Exception e)
        {
            Debug.Log("WRITE: File Does Not Exist.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
