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

    private int[,] disablePUs = new int[2, 7] { { 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0 } };

    // Start is called before the first frame update
    void Start()
    {
        listA.GetComponentInChildren<Text>().text = "";
        listB.GetComponentInChildren<Text>().text = "";
        initPU.interactable = false;
    }

    void checkPowerUp()
    {
        if (selectTurn == 1)
        {
            for (int i = 0; i < selectSize; i++)
            {
                if (disablePUs[0, i] == 0)
                {
                    pbtnList[i].interactable = true;
                } else
                {
                    pbtnList[i].interactable = false;
                }
            }
        } else
        {
            for (int i = 0; i < selectSize; i++)
            {
                if (disablePUs[1, i] == 0)
                {
                    pbtnList[i].interactable = true;
                }
                else
                {
                    pbtnList[i].interactable = false;
                }
            }
        }
    }

    public void selectPowerUp()
    {
        string powerText = EventSystem.current.currentSelectedGameObject.name;
        int ind;
        Debug.Log(powerText);
        Text tempText;
        List<PowerUpBase> tempList;

        if (selectTurn == 1)
        {
            tempText = listA;
            tempList = pListA;
            ind = 0;
        }
        else
        {
            tempText = listB;
            tempList = pListB;
            ind = 1;
        }
        if (powerText.Contains("undo"))
        {
            // Debug.Log("Undo");
            tempText.GetComponentInChildren<Text>().text += "\n> Undo";
            UndoPowerUp un = new UndoPowerUp();
            tempList.Add(un);
            disablePUs[ind, 1] = 1;              
        } else if (powerText.Contains("none"))
        {
            Debug.Log("None");
            tempText.GetComponentInChildren<Text>().text += "\n> None";
            disablePUs[ind, 0] = 1;
        } else if (powerText.Contains("swap"))
        {
            // Debug.Log("Swap");
            tempText.GetComponentInChildren<Text>().text += "\n> Swap";
            SwapPowerUp sw = new SwapPowerUp();
            tempList.Add(sw);
            disablePUs[ind, 2] = 1;
        } else if (powerText.Contains("rev"))
        {
            // Debug.Log("Reverse");
            tempText.GetComponentInChildren<Text>().text += "\n> Reverse";
            ReversePowerUp re = new ReversePowerUp();
            tempList.Add(re);
            disablePUs[ind, 4] = 1;
        } else if (powerText.Contains("exp"))
        {
            // Debug.Log("Explode");
            tempText.GetComponentInChildren<Text>().text += "\n> Explode";
            ExplodePowerUp ex = new ExplodePowerUp();
            tempList.Add(ex);
            disablePUs[ind, 3] = 1;
        } else if (powerText.Contains("shield"))
        {
            // Debug.Log("Shield");
            tempText.GetComponentInChildren<Text>().text += "\n> Shield";
            BlockPowerUp bl = new BlockPowerUp();
            tempList.Add(bl);
            disablePUs[ind, 5] = 1;
        } else // Nullify
        {
            // Debug.Log("Nullify");
            tempText.GetComponentInChildren<Text>().text += "\n> Nullify";
            NullifyPowerUp nu = new NullifyPowerUp();
            tempList.Add(nu);
            disablePUs[ind, 6] = 1;
        }
        selectTurn *= -1;
        checkPowerUp();
        if (--totalSelect == 0)
        {
            initPU.interactable = true;
            initPU.GetComponentInChildren<Text>().text = "Begin Game!";
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
