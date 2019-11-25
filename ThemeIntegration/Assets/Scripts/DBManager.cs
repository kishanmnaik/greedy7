using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBManager : MonoBehaviour
{

    public static string screenName1;
    public static int score1;

    public static string screenName2;
    public static int score2;

    public static bool inPlay
    {
        get
        {
            return screenName1 != null && screenName2 != null;
        }
    }

    public static void donePlay()
    {
        screenName1 = null;
        screenName2 = null;
    }
}
