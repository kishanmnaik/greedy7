using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public static PowerUps allP1 = new PowerUps();
    public static PowerUps allP2 = new PowerUps();

    public int selectSize = 7, totalSelect = 6;
    public Button[] pbtnList;
    public Button initPU;
    public Text listA, listB;
    public int selectTurn = 1;
    List<PowerUpBase> pListA = new List<PowerUpBase>();
    List<PowerUpBase> pListB = new List<PowerUpBase>();

    // Start is called before the first frame update
    void Start()
    {
        listA.GetComponentInChildren<Text>().text = "";
        listB.GetComponentInChildren<Text>().text = "";
        initPU.interactable = false;
    }

    public void selectPowerUp()
    {
        string powerText = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log(powerText);
        Text tempText;
        List<PowerUpBase> tempList;

        if (selectTurn == 1)
        {
            tempText = listA;
            tempList = pListA;
        }
        else
        {
            tempText = listB;
            tempList = pListB;
        }
        if (powerText.Contains("undo"))
        {
            // Debug.Log("Undo");
            tempText.GetComponentInChildren<Text>().text += "\n> Undo";
            UndoPowerUp un = new UndoPowerUp();
            tempList.Add(un);
        } else if (powerText.Contains("none"))
        {
            Debug.Log("None");
            tempText.GetComponentInChildren<Text>().text += "\n> None";
        } else if (powerText.Contains("swap"))
        {
            // Debug.Log("Swap");
            tempText.GetComponentInChildren<Text>().text += "\n> Swap";
            SwapPowerUp sw = new SwapPowerUp();
            tempList.Add(sw);
        } else if (powerText.Contains("rev"))
        {
            // Debug.Log("Reverse");
            tempText.GetComponentInChildren<Text>().text += "\n> Reverse";
            ReversePowerUp re = new ReversePowerUp();
            tempList.Add(re);
        } else if (powerText.Contains("exp"))
        {
            // Debug.Log("Explode");
            tempText.GetComponentInChildren<Text>().text += "\n> Explode";
            ExplodePowerUp ex = new ExplodePowerUp();
            tempList.Add(ex);
        } else if (powerText.Contains("shield"))
        {
            // Debug.Log("Shield");
            tempText.GetComponentInChildren<Text>().text += "\n> Shield";
            BlockPowerUp bl = new BlockPowerUp();
            tempList.Add(bl);
        } else // Nullify
        {
            // Debug.Log("Nullify");
            tempText.GetComponentInChildren<Text>().text += "\n> Nullify";
            NullifyPowerUp nu = new NullifyPowerUp();
            tempList.Add(nu);
        }
        selectTurn *= -1;
        if (--totalSelect == 0)
        {
            initPU.interactable = true;
            for (int i = 0; i < selectSize; i++)
            {
                pbtnList[i].interactable = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void initPowerUp()
    {
        // PowerUps pNew = new PowerUps();
        allP1.Init(pListA);
        allP2.Init(pListB);
        SceneManager.LoadScene(1);
    }
}
