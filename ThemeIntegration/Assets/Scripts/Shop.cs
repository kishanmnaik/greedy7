using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System.Timers;
using UnityEngine.SceneManagement;
using System;
using System.Text;
using System.IO;

public class Shop : MonoBehaviour
{
    public Button[] beadThemeList;
    public Text selectedText;
    // private LoadSettings ls = new LoadSettings();

    private string pSettings = "Assets/Resources/nums/config.txt";

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            StreamReader sr = new StreamReader(pSettings);
            string line = sr.ReadLine();

            /* ls = ls.getJSONObject(line);

            Debug.Log("CONFIG LINE: " + line);
            Debug.Log("JSON String: " + ls.beadTheme); */

            // string acPath = "nums/" + line;
            // Debug.Log("PATH: " + acPath);

         

            selectedText.GetComponentInChildren<Text>().text = "Selected: " + line;

            switch (line)
            {
                case "red":
                    beadThemeList[1].interactable = false;
                    break;
                case "blue":
                    beadThemeList[2].interactable = false;
                    break;
                case "green":
                    beadThemeList[3].interactable = false;
                    break;
                default:
                    beadThemeList[0].interactable = false;
                    break;
            }

            sr.Close();
        } catch (Exception e)
        {
            Debug.Log("Lol doesn't exist buddy.");
        }
    }

    public void updateBeadTheme()
    {
        string buttonName = EventSystem.current.currentSelectedGameObject.name;

        for (int i = 0; i < beadThemeList.Length; i++)
        {
            beadThemeList[i].interactable = true;
        }

        switch (buttonName)
        {
            case "red":
                beadThemeList[1].interactable = false;
                break;
            case "blue":
                beadThemeList[2].interactable = false;
                break;
            case "green":
                beadThemeList[3].interactable = false;
                break;
            default:
                beadThemeList[0].interactable = false;
                break;
        }

        try
        {
            System.IO.File.WriteAllText(@pSettings, buttonName);
        } catch (Exception e)
        {
            Debug.Log("Can't write my dude.");
        }

        selectedText.GetComponentInChildren<Text>().text = "Selected: " + buttonName;
    }

    public void gotoMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
