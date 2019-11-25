using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LoadSettings
{
    public string beadTheme;

    public string toJSONString()
    {
        return JsonUtility.ToJson(this);
    }

    public LoadSettings getJSONObject(string jsonString)
    {
        return JsonUtility.FromJson<LoadSettings>(jsonString);
    }

    public void writeJSONObject(string savedData)
    {
        JsonUtility.FromJsonOverwrite(savedData, this);
    }
}
