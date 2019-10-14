using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class gridMain : MonoBehaviour
{
    public static int gridSize = 10;
    public static int initialNumOfBeads = 3;
    public int totalCoins = gridSize * initialNumOfBeads;

    public Button[] btnList;

    int[] gridArray = new int[gridSize];
    public Text winText, inHand;
    public Text scoreA, scoreB;
    public int scoreValA = 0, scoreValB = 0, acHand = 0;

    private Tile[] tileArray = new Tile[gridSize];

    public GameObject inhand1 ;
    public GameObject inhand2 ;

    public int turn = 0;
    public bool inTurn = false;

    // Start is called before the first frame update
    void Start()
    {
        var coinToss = new coinToss();
        turn = coinToss.getTurn();
        Debug.Log("Turn: " + turn);
        winText.gameObject.SetActive(false);
        initializeGrid();
        turnDisable();
        var powerUpsA = new PowerUps();
        var powerUpsB = new PowerUps();
        powerUpsA.Init();
        powerUpsB.Init();
        Debug.Log("A PU: " + powerUpsA.GetPowerUpCount());
        Debug.Log(powerUpsB.GetPowerUpCount());
    }

    public void initializeGrid()
    {
        for (int i = 0; i < gridSize; i++)
        {
            // gridArray[i] = initialNumOfBeads;
            // btnList[i].GetComponentInChildren<Text>().text = initialNumOfBeads.ToString();
            tileArray[i] = new Tile(initialNumOfBeads, i, btnList[i]);
        }
    }

    public void onClickTile()
    {
        //Debug.Log("clicked");
        string buttonName = EventSystem.current.currentSelectedGameObject.name;
        //Debug.Log(buttonName);

        string buttonText = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        //Debug.Log(buttonText);

        int index = System.Int32.Parse(buttonName);
        updateScore(index);

        //int value = System.Int32.Parse(buttonText) + 1;
        // btnList[index+1].GetComponentInChildren<Text>().text = value.ToString();
        // btnList[index+1].Select();
        //var ut = new util();
        //ut.someFunction2("something non static");
        //EventSystem.current.SetSelectedGameObject(null);
        //util.someFunction("some thing in static");
    }

    public void updateScore(int ind)
    {
        int i = ind;
        if (!inTurn)
        {
            // acHand = gridArray[i];
            acHand = tileArray[i].getVal();
            if (acHand == 0)
            {
                turn *= -1;
                turnDisable();
                checkWinCon();
                return;
            }
            inHand.GetComponentInChildren<Text>().text = "InHand: " + acHand;
            // gridArray[i] = 0;
            tileArray[i].setVal(0);
            inTurn = true;
        } else {
            inHand.GetComponentInChildren<Text>().text = "InHand: " + (--acHand);
            // gridArray[i]++;
            tileArray[i].setVal(tileArray[i].getVal() + 1);
            if (acHand == 0)
                inTurn = false;
        }
        enableTiles();
        disableTiles(i);
        // if (gridArray[(i + 1) % gridSize] == 0 && acHand == 0)
        if (tileArray[(i + 1) % gridSize].getVal() == 0 && acHand == 0)
        {
            if (turn == 1)
            {
                // scoreValA += gridArray[(i + 2) % gridSize];
                scoreValA += tileArray[(i + 2) % gridSize].getVal();
                scoreA.GetComponentInChildren<Text>().text = "ScoreA: " + scoreValA;
            }
            else
            {
                // scoreValB += gridArray[(i + 2) % gridSize];
                scoreValB += tileArray[(i + 2) % gridSize].getVal();
                scoreB.GetComponentInChildren<Text>().text = "ScoreB: " + scoreValB;
            }
            // totalCoins -= gridArray[(i + 2) % gridSize];
            totalCoins -= tileArray[(i + 2) % gridSize].getVal();
            // gridArray[(i + 2) % gridSize] = 0;
            tileArray[(i + 2) % gridSize].setVal(0);
            turn *= -1;
            turnDisable();
            checkWinCon();
        }
    }

    public void enableTiles()
    {
        for (int j = 0; j < gridSize; j++)
        {
            // btnList[j].interactable = true;
            tileArray[j].setEnable();
        }
    }

    public void disableTiles(int id)
    {
        for (int k = 1; k <= gridSize; k++)
        {
            int j = (id + k) % gridSize;
            if (k != 1)
            {
                // btnList[j].interactable = false;
                tileArray[j].setDisable();
            }
            else
            {
                // btnList[j].interactable = true;
                tileArray[j].setEnable();
            }
        }
    }
    
    public void turnDisable()
    {
        // Checks turn (1 => A, -1 => B)
        if (turn == 1)
        {
            // Disables Player B buttons
            for (int i = (gridSize / 2); i < gridSize; i++)
            {
                // btnList[i].interactable = false;
                tileArray[i].setDisable();
            }
            // Enables Player A buttons
            for (int i = 0; i < gridSize / 2; i++)
            {
                // btnList[i].interactable = true;
                tileArray[i].setEnable();
            }

        }
        else if (turn == -1)
        {
            // Enables Player B buttons
            for (int i = (gridSize / 2); i < gridSize; i++)
            {
                // btnList[i].interactable = false;
                tileArray[i].setEnable();
            }
            // Disables Player A buttons
            for (int i = 0; i < gridSize / 2; i++)
            {
                // btnList[i].interactable = true;
                tileArray[i].setDisable();
            }
        }
    }

    /* public void updateScore(int ind)
    {
        Debug.Log("Index: " + ind + " : value: " + gridArray[ind]);
        int i = ind;
        // Checks if the tile has zero coins or not
        if (gridArray[i] != 0)
        {
            // Main Loop
            while (gridArray[i] != 0)
            {
                int j = i;
                int temp = gridArray[i];
                // No. of coins to be distributed => 1 to coins in gridArray[i]
                for (int k = 1; k <= temp; k++)
                {
                    Debug.Log("k: " + k + " i, a[i]: " + i + ", " + gridArray[i] + " j, a[j]: " + j + ", " + gridArray[j]);
                    j = (j + 1) % gridSize;
                    // Decrement the no of coins in gridArray[i] by 1 and
                    // Increment the no of coins in gridArray[j] by 1 where j = (j + 1) % length
                    gridArray[i] -= 1;
                    gridArray[j] += 1;
                }
                i = (j + 1) % gridSize;
                Debug.Log("i ind: " + i + " | gridArray[i]: " + gridArray[i]);
            }
            // Adds value of gridArray[i -> next] to either Player A's score or B's
            if (turn == 1)
            {
                scoreValA += gridArray[(i + 1) % gridSize];
                scoreA.GetComponentInChildren<Text>().text = "ScoreA: " + scoreValA;
            }
            else
            {
                scoreValB += gridArray[(i + 1) % gridSize];
                scoreB.GetComponentInChildren<Text>().text = "ScoreB: " + scoreValB;
            }

            Debug.Log("Coins this turn: " + gridArray[(i + 1) % gridSize]); // Coins earned this turn
            totalCoins -= gridArray[(i + 1) % gridSize]; // Deducts coins earned from Total coins 
            // Resets the value of gridArray[i -> next] to 0
            gridArray[(i + 1) % gridSize] = 0;
            Debug.Log("Total: " + totalCoins);
        }
        // Toggles turn
        turn *= -1;
    } */

    public void checkWinCon()
    {
        if (totalCoins <= 2)
        {
            Debug.Log("WIN SITUATION!");
            // Disables all the tiles
            for (int z = 0; z < gridSize; z++)
            {
                // btnList[z].interactable = false;
                tileArray[z].setDisable();
            }

            // Unhides the win text object
            winText.gameObject.SetActive(true);
            if (scoreValA > scoreValB)
                winText.text = "Player A wins!";
            else if (scoreValA < scoreValB)
                winText.text = "Player B wins!";
            else
                winText.text = "It's a tie!";
        }
    }

    // Updates the board for every turn
    void updateBoard()
    {
        // Checks if the number of buttons in the list is the same as the number of tiles
        if (gridSize == gridSize)
        {
            // Rewrites all the values of the changed array to the button's text
            for (int i = 0; i < gridSize; i++)
            {
                // btnList[i].GetComponentInChildren<Text>().text = "" + gridArray[i];
                tileArray[i].setText((tileArray[i].getVal()).ToString());
            }
        }
        // No. of buttons in the list != Number of Tiles (Check Initialization)
        else
        {
            Debug.Log("ERROR INITIALIZING TILES!");
            Debug.Log("Length of board != No. of tiles");
        }
    }

    // Update is called once per frame
    void Update()
    {
        updateBoard();
        // Checks if the game has a winner
    }

    
}
