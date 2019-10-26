using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Registration : MonoBehaviour
{
    public InputField nameField;
    public Button submitButton;

    public void callRegister()
    {
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {
        string nl = "\n";
        string url = "http://localhost:8888/sqlconnect/register.php?name=" + nameField.text;
        Debug.Log(nl);
        Debug.Log(url);
        Debug.Log(nl);
        WWW www = new WWW(url);
        Debug.Log(nl);
        Debug.Log(www.url);
        Debug.Log(nl);
        yield return www;
        Debug.Log(nl);
        Debug.Log(www.text);
        Debug.Log(nl);
        if (www.text == "0")
        {
            Debug.Log("User registered successfully");
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        else
        {
            Debug.Log("User registration failed. Error #" + www.text);
        }
    }

    public void VerifyInputs()
    {
        submitButton.interactable = (nameField.text.Length >= 1);
    }

}
