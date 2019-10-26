using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public Text PlayerDispaly1;
    public Text PlayerDispaly2;

    private void Start()
    {
        if (DBManager.screenName1 !=null && DBManager.screenName2 != null)
        {
            PlayerDispaly1.text = DBManager.screenName1;
            PlayerDispaly2.text = DBManager.screenName2;
        }
    }
    public void GotoRegister()
    {
        SceneManager.LoadScene(1);
    }

    public void GotoPlay()
    {
        SceneManager.LoadScene(2);
    }
}
