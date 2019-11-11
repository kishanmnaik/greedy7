using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class mainScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void gotoPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void quitGame()
    {
        try
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        catch (Exception e)
        {
            Application.Quit();
        }
        // Application.Quit();
    }
}
