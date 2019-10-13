using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{

    public Button[] btnList;
    public Text winText;

    public static int turn = 1;
    public static int tiles_length = 6;
    public static int coins_no = 3;
    public static int[] ar = new int[tiles_length];
    public static int total_coins = coins_no * tiles_length;
    
    // Start is called before the first frame update
    void Start()
    {
        // Hides the win text object
        winText.gameObject.SetActive(false);
        // Checks if the number of buttons in the list is the same as the number of tiles
        if (btnList.Length == tiles_length)
        {
            // Initializes the array to the no of coins and 
            // writes the value to the button's text
            for (int i = 0; i < btnList.Length; i++)
            {
                ar[i] = coins_no;
                btnList[i].GetComponentInChildren<Text>().text = "" + ar[i];

                //if (i < tiles_length)
                //    btnList[i].interactable = true;
                //else
                //    btnList[i].interactable = false;
            }
        }
        // No. of buttons in the list != Number of Tiles (Check Initialization)
        else
        {
            Debug.Log("ERROR INITIALIZING TILES!");
            Debug.Log("Length of board != No. of tiles");
        }
    }

    // Updates the score values for either Player A or Player B
    public static void updateScore(int ind)
    {
        Debug.Log("Index: " + ind + " : value: " + ar[ind]);
        int i = ind;
        // Checks if the tile has zero coins or not
        if (ar[i] != 0)
        {
            // Main Loop
            while (ar[i] != 0)
            {
                int j = i;
                int temp = ar[i];
                // No. of coins to be distributed => 1 to coins in ar[i]
                for (int k = 1; k <= temp; k++)
                {
                    Debug.Log("k: " + k + " i, a[i]: " + i + ", " + ar[i] + " j, a[j]: " + j + ", " + ar[j]);
                    j = (j + 1) % tiles_length;
                    // Decrement the no of coins in ar[i] by 1 and
                    // Increment the no of coins in ar[j] by 1 where j = (j + 1) % length
                    ar[i] -= 1;
                    ar[j] += 1;
                }
                i = (j + 1) % tiles_length;
                Debug.Log("i ind: " + i + " | ar[i]: " + ar[i]);
            }
            // Adds value of ar[i -> next] to either Player A's score or B's
            if (turn == 1)
                ScoreManager.scoreValA += ar[(i + 1) % tiles_length];
            else
                ScoreManager.scoreValB += ar[(i + 1) % tiles_length];

            Debug.Log("Coins this turn: " + ar[(i + 1) % tiles_length]); // Coins earned this turn
            total_coins -= ar[(i + 1) % tiles_length]; // Deducts coins earned from Total coins 
            // Resets the value of ar[i -> next] to 0
            ar[(i + 1) % tiles_length] = 0;
            Debug.Log("Total: " + total_coins);
        }
        // Toggles turn
        turn *= -1;
    }

    // Updates the board for every turn
    void updateBoard()
    {
        // Checks if the number of buttons in the list is the same as the number of tiles
        if (btnList.Length == tiles_length)
        {
            // Checks turn (1 => A, -1 => B)
            if (turn == 1)
            {
                // Disables Player B buttons
                for (int i = (tiles_length / 2); i < btnList.Length; i++)
                    btnList[i].interactable = false;
                // Enables Player A buttons
                for (int i = 0; i < tiles_length / 2; i++)
                    btnList[i].interactable = true;
            }
            else
            {
                // Disables Player A buttons
                for (int i = 0; i < tiles_length / 2; i++)
                    btnList[i].interactable = false;
                // Enables Player B buttons
                for (int i = (tiles_length / 2); i < btnList.Length; i++)
                    btnList[i].interactable = true;
            }
            // Rewrites all the values of the changed array to the button's text
            for (int i = 0; i < btnList.Length; i++)
            {
                btnList[i].GetComponentInChildren<Text>().text = "" + ar[i];
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
        if (total_coins <= 2)
        {
            Debug.Log("WIN SITUATION!");
            // Disables all the tiles
            for (int z = 0; z < tiles_length; z++)
                btnList[z].interactable = false;
            // Unhides the win text object
            winText.gameObject.SetActive(true);
            if (ScoreManager.scoreValA > ScoreManager.scoreValB)
                winText.text = "Player A wins!";
            else if (ScoreManager.scoreValA < ScoreManager.scoreValB)
                winText.text = "Player B wins!";
            else
                winText.text = "It's a tie!";
        }
    }
}
