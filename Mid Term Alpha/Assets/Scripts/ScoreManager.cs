using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    //public Text[] scoreTexts;
    public static int scoreValA = 0, scoreValB = 0;

    public Text PlayerDispaly1;
    public Text PlayerDispaly2;

    public Text Player1CScore;
    public Text Player2CScore;

    // Start is called before the first frame update
    void Start()
    {
        if (DBManager.screenName1 != null && DBManager.screenName2 != null)
        {
            Debug.Log(DBManager.score1);
            Debug.Log(DBManager.score2);
            PlayerDispaly1.text = DBManager.screenName1 + ":"+ DBManager.score1;
            PlayerDispaly2.text = DBManager.screenName2 + ":" + DBManager.score2;
        }
        // Intializes scores of Player A and Player B's to 0
        Player1CScore.text = "A: " + scoreValA;
        Player2CScore.text = "B: " + scoreValB;
    }

    // Update is called once per frame
    void Update()
    {
        // Updates scores of Player A and Player B's to 
        // their respective scoreVal
        Player1CScore.text = "A: " + scoreValA;
        Player2CScore.text = "B: " + scoreValB;
    }
}
