using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text[] scoreTexts;
    public static int scoreValA = 0, scoreValB = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Intializes scores of Player A and Player B's to 0
        scoreTexts[0].text = "A: " + scoreValA;
        scoreTexts[1].text = "B: " + scoreValB;
    }

    // Update is called once per frame
    void Update()
    {
        // Updates scores of Player A and Player B's to 
        // their respective scoreVal
        scoreTexts[0].text = "A: " + scoreValA;
        scoreTexts[1].text = "B: " + scoreValB;
    }
}
