using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    public InputField name1Field;
    public InputField name2Field;

    public Button playButton;

    public void callPlay()
    {
        StartCoroutine(startPlay());
    }

    IEnumerator startPlay()
    {
        string url = "http://localhost:8888/sqlconnect/getScore.php?name1=" + name1Field.text + "&name2=" + name2Field.text;
        WWW www = new WWW(url);
        yield return www;

        string[] webResult = www.text.Split('\t');
        if (www.text[0] == '0')
        {
            DBManager.screenName1= name1Field.text;
            DBManager.screenName2 = name2Field.text;

            DBManager.score1 = int.Parse(webResult[1]);
            DBManager.score2 = int.Parse(webResult[2]);

            UnityEngine.SceneManagement.SceneManager.LoadScene(4);

        }
        else
        {
            Debug.Log("Error while executing :" + www.text);
        }



    }


    public void VerifyInputs()
    {
        playButton.interactable = (name1Field.text.Length >= 1 && name2Field.text.Length >= 1);
    }
}
