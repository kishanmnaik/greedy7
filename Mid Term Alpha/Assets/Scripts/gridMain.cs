﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System.Timers;

using UnityEngine.SceneManagement;

public class gridMain : MonoBehaviour
{
    public static int gridSize = 10;
    public static int initialNumOfBeads = 3;
    public int totalCoins = gridSize * initialNumOfBeads;

    public Button[] btnList;

    public Button[] apList, bpList;

    int[] gridArray = new int[gridSize];
    public Text winText, inHand, explodeSelect, blockSelect;
    public Text scoreA, scoreB;

    public Text nullTextA, nullTextB;

    public bool isCalledNullify = false;
    public bool isNullifyButtonClickedA = false;
    public bool isNullifyButtonClickedB = false;
    public bool isPowerUpButtonClicked = false;
    public int count_null = 0;
    public string nullify_powerup = "";
    public static float time_counter = 0;
    public bool isCalledBlock = false;
    public bool isBlockButtonClicked = false;
    public int blockFlag = 0;
    public static float Block_time = 0;
    public int playerWhoBlocked = 0;
    public int blockedIndex = -1;
    public float time = 3f;

    public bool isCalledUndo = false;

    public bool isUndoClickedA = false;
    public bool isUndoClickedB = false;
    public Text undoTextA, undoTextB;

    public int scoreValA = 0, scoreValB = 0, acHand = 0;
    public int preScoreValA = 0, preScoreValB = 0;

    private Tile[] tileArray = new Tile[gridSize];
    private Tile[] previousTileArray = new Tile[gridSize];

    public Image dirCW, dirCCW;

    private bool reverse;

    public int turn = 0;
    public bool inTurn = false;
    public bool win = false;

    private PowerUps listPowerUp1 = new PowerUps();
    private PowerUps listPowerUp2 = new PowerUps();

    private bool explodePowerUpInUse = false;



    // public Button pup1;

    // Start is called before the first frame update
    void Start()
    {
        var coinToss = new coinToss();
        turn = coinToss.getTurn();
        Debug.Log("Turn: " + turn);
        winText.gameObject.SetActive(false);
        explodeSelect = GameObject.Find("explodeSelect").GetComponent<Text>();
        explodeSelect.gameObject.SetActive(false);

        blockSelect = GameObject.Find("blockSelect").GetComponent<Text>();
        blockSelect.gameObject.SetActive(false);

        nullTextA.gameObject.SetActive(false);
        nullTextB.gameObject.SetActive(false);

        undoTextA.gameObject.SetActive(false);
        undoTextB.gameObject.SetActive(false);

        initializeGrid();
        turnDisable();
        /* var powerUpsA = new PowerUps();
        var powerUpsB = new PowerUps();
        powerUpsA.Init();
        powerUpsB.Init();
        Debug.Log("A PU: " + powerUpsA.GetPowerUpCount());
        Debug.Log(powerUpsB.GetPowerUpCount()); */
        listPowerUp1 = Loading.allP1;
        listPowerUp2 = Loading.allP2;

        initializePowerUp();
    }

    void handleReverseDirection()
    {
        if (reverse)
        {
            dirCCW.gameObject.SetActive(false);
            dirCW.gameObject.SetActive(true);
        }
        else
        {
            dirCW.gameObject.SetActive(false);
            dirCCW.gameObject.SetActive(true);
        }
    }

    void initializePowerUp()
    {
        foreach (var btnTemp in apList)
        {
            btnTemp.interactable = false;
        }

        foreach (var btnTemp in bpList)
        {
            btnTemp.interactable = false;
        }

        foreach (var px in listPowerUp1.GetActivePowerUps())
        {
            string pTempText = px.GetType();
            // Debug.Log("A: " + px.GetType());
            if (pTempText.Contains("undo"))
            {
                apList[0].interactable = true;
            }
            else if (pTempText.Contains("swap"))
            {
                apList[1].interactable = true;
            }
            else if (pTempText.Contains("explode"))
            {
                apList[2].interactable = true;
            }
            else if (pTempText.Contains("reverse"))
            {
                apList[3].interactable = true;
            }
            else if (pTempText.Contains("shield"))
            {
                apList[4].interactable = true;
            }
            else if (pTempText.Contains("nullify"))
            {
                apList[5].interactable = true;
            }
        }
        foreach (var px in listPowerUp2.GetActivePowerUps())
        {
            // Debug.Log("B: " + px.GetType());
            string pTempText = px.GetType();
            if (pTempText.Contains("undo"))
            {
                bpList[0].interactable = true;
            }
            else if (pTempText.Contains("swap"))
            {
                bpList[1].interactable = true;
            }
            else if (pTempText.Contains("explode"))
            {
                bpList[2].interactable = true;
            }
            else if (pTempText.Contains("reverse"))
            {
                bpList[3].interactable = true;
            }
            else if (pTempText.Contains("shield"))
            {
                bpList[4].interactable = true;
            }
            else if (pTempText.Contains("nullify"))
            {
                bpList[5].interactable = true;
            }
        }
    }

    public void initializeGrid()
    {
        for (int i = 0; i < gridSize; i++)
        {
            tileArray[i] = new Tile(initialNumOfBeads, i, btnList[i]);
            previousTileArray[i] = new Tile(initialNumOfBeads, i, btnList[i]);
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

    public void ClickedBlock()
    {
        Debug.Log("************Entered ClickedBlock***********");
        isBlockButtonClicked = true;
        time_counter = 5.0f;
        blockSelect.gameObject.SetActive(true);
        blockSelect.text = "Select the tile to Block!";
        turnDisable();
    }


    public void initializeNullifyPowerUp(string powerUpClicked)
    {
        isCalledNullify = false;
        isNullifyButtonClickedA = false;
        isNullifyButtonClickedB = false;

        List<PowerUpBase> playerPowerUps_1;
        List<PowerUpBase> playerPowerUps_2;

        playerPowerUps_1 = listPowerUp1.GetActivePowerUps();
        playerPowerUps_2 = listPowerUp2.GetActivePowerUps();

        if (turn == 1)
        {
            var nullifyPowerUp = playerPowerUps_2.FirstOrDefault(x => x.GetType().Equals("nullify"));
            var explodePowerUp = playerPowerUps_1.FirstOrDefault(x => x.GetType().Equals(powerUpClicked));
            listPowerUp1.DisablePowerUp(explodePowerUp);
            listPowerUp2.DisablePowerUp(nullifyPowerUp);
        }
        else
        {
            var nullifyPowerUp = playerPowerUps_1.FirstOrDefault(x => x.GetType().Equals("nullify"));
            var explodePowerUp = playerPowerUps_2.FirstOrDefault(x => x.GetType().Equals(powerUpClicked));
            listPowerUp2.DisablePowerUp(explodePowerUp);
            listPowerUp1.DisablePowerUp(nullifyPowerUp);
        }
        initializePowerUp();
    }


    public void ClickedNullifyA()
    {
        Debug.Log("nullify A is clicked");

        if (isPowerUpButtonClicked)
        {
            isNullifyButtonClickedA = true;
            time_counter = 5.0f;
        }
        else
        {
            Debug.Log("nullify A is clicked but no power up");
        }
    }

    public void ClickedNullifyB()
    {
        Debug.Log("nullify B is clicked");
        if (isPowerUpButtonClicked)
        {
            isNullifyButtonClickedB = true;
            time_counter = 5.0f;
        }
        else
        {
            Debug.Log("nullify B is clicked but no power up");
        }
    }

    public void ExplodedPowerUpInitiated()
    {
        isPowerUpButtonClicked = true;
        nullify_powerup = "explode";


        if ((turn * -1 == 1 && apList[5].interactable == true) || (turn * -1 == -1 && bpList[5].interactable == true))
        {
            if (turn * -1 == 1)
            {
                nullTextA.gameObject.SetActive(true);
            }
            else
            {
                nullTextB.gameObject.SetActive(true);
            }

            time_counter = 0;
            isCalledNullify = true;

        }
        else
        {
            nullify_powerup = "";
            explodePowerUpInUse = true;
            explodeSelect.gameObject.SetActive(true);

            explodeSelect.text = "Select the tile to explode! BOOM!";
            turn *= -1; //to explode opponent's tile
            turnDisable();
            isPowerUpButtonClicked = false;
        }
        //isPowerUpButtonClicked = true;


        //if (count_null == 0)
        //{
        //    if ((turn * -1 == 1 && apList[5].interactable == true) || (turn * -1 == -1 && bpList[5].interactable == true))
        //    {
        //        if (turn * -1 == 1)
        //        {
        //            nullTextA.gameObject.SetActive(true);
        //        }
        //        else
        //        {
        //            nullTextB.gameObject.SetActive(true);
        //        }
        //        isCalledNullify = true;
        //        count_null += 1;
        //        nullify_powerup = "explode";

        //    }
        //    else
        //    {
        //        explodePowerUpInUse = true;
        //        explodeSelect.gameObject.SetActive(true);

        //        explodeSelect.text = "Select the tile to explode! BOOM!";
        //        turn *= -1; //to explode opponent's tile
        //        turnDisable();
        //    }
        //}
        //else
        //{
        //    count_null = 0;
        //    if (isNullifyButtonClicked == true)
        //    {

        //        isCalledNullify = false;
        //        nullify_powerup = "";
        //        isNullifyButtonClicked = false;

        //        List<PowerUpBase> playerPowerUps_1;
        //        List<PowerUpBase> playerPowerUps_2;

        //        playerPowerUps_1 = listPowerUp1.GetActivePowerUps();
        //        playerPowerUps_2 = listPowerUp2.GetActivePowerUps();

        //        if (turn == 1)
        //        {
        //            var nullifyPowerUp = playerPowerUps_2.FirstOrDefault(x => x.GetType().Equals("nullify"));
        //            var explodePowerUp = playerPowerUps_1.FirstOrDefault(x => x.GetType().Equals("explode"));
        //            listPowerUp1.DisablePowerUp(explodePowerUp);
        //            listPowerUp2.DisablePowerUp(nullifyPowerUp);
        //        }
        //        else
        //        {
        //            var nullifyPowerUp = playerPowerUps_1.FirstOrDefault(x => x.GetType().Equals("nullify"));
        //            var explodePowerUp = playerPowerUps_2.FirstOrDefault(x => x.GetType().Equals("explode"));
        //            listPowerUp2.DisablePowerUp(explodePowerUp);
        //            listPowerUp1.DisablePowerUp(nullifyPowerUp);
        //        }
        //        initializePowerUp();
        //        return;
        //    }
        //    else
        //    {
        //        explodePowerUpInUse = true;
        //        explodeSelect.gameObject.SetActive(true);

        //        explodeSelect.text = "Select the tile to explode! BOOM!";
        //        turn *= -1; //to explode opponent's tile
        //        turnDisable();
        //    }
        //}

        //isPowerUpButtonClicked = false;
    }

    public void UseExplodePowerUp(int indexToBeExploded)
    {
        turn *= -1;
        explodeSelect.gameObject.SetActive(false);
        List<PowerUpBase> playerPowerUps;
        if (turn == 1)
        {
            playerPowerUps = listPowerUp1.GetActivePowerUps();
        }
        else
        {
            playerPowerUps = listPowerUp2.GetActivePowerUps();
        }

        var explodePowerUp = playerPowerUps.FirstOrDefault(x => x.GetType().Equals("explode"));
        PowerUpTilesDto powerUpTilesDto = getPowerUpDto(-1, indexToBeExploded, ScoreA: scoreValA, ScoreB: scoreValB, PreviousScoreA: preScoreValA, PreviousScoreB: preScoreValB);
        powerUpTilesDto = explodePowerUp.Use(powerUpTilesDto);
        powerUpTilesDto.TileArray.CopyTo(tileArray, 0);

        if (turn == 1)
        {
            listPowerUp1.DisablePowerUp(explodePowerUp);
        }
        else
        {
            listPowerUp2.DisablePowerUp(explodePowerUp);
        }
        takeSnapshot(powerUpTilesDto);
        initializePowerUp(); //to disable used powerup
        enableTiles();
        turn *= -1;
        turnDisable();
        explodePowerUpInUse = false;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void UseReversePowerUp()
    {

        isPowerUpButtonClicked = true;
        nullify_powerup = "reverse";


        if ((turn * -1 == 1 && apList[5].interactable == true) || (turn * -1 == -1 && bpList[5].interactable == true))
        {
            if (turn * -1 == 1)
            {
                nullTextA.gameObject.SetActive(true);
            }
            else
            {
                nullTextB.gameObject.SetActive(true);
            }

            time_counter = 0;
            isCalledNullify = true;

        }
        else
        {

            Debug.Log("reverse called :" + reverse);
            List<PowerUpBase> playerPowerUps;
            if (turn == 1)
            {
                playerPowerUps = listPowerUp1.GetActivePowerUps();
            }
            else
            {
                playerPowerUps = listPowerUp2.GetActivePowerUps();
            }

            var reversePowerUp = playerPowerUps.FirstOrDefault(x => x.GetType().Equals("reverse"));
            PowerUpTilesDto powerUpTilesDto = getPowerUpDto(ScoreA: scoreValA, ScoreB: scoreValB, PreviousScoreA: preScoreValA, PreviousScoreB: preScoreValB);
            powerUpTilesDto = reversePowerUp.Use(powerUpTilesDto);
            reverse = powerUpTilesDto.Reverse;
            if (turn == 1)
            {
                listPowerUp1.DisablePowerUp(reversePowerUp);
            }
            else
            {
                listPowerUp2.DisablePowerUp(reversePowerUp);
            }
            initializePowerUp(); //to disable used powerup
        }
    }

    public void UseSwapPowerUp()
    {
        isPowerUpButtonClicked = true;
        nullify_powerup = "swap";


        if ((turn * -1 == 1 && apList[5].interactable == true) || (turn * -1 == -1 && bpList[5].interactable == true))
        {
            if (turn * -1 == 1)
            {
                nullTextA.gameObject.SetActive(true);
            }
            else
            {
                nullTextB.gameObject.SetActive(true);
            }

            time_counter = 0;
            isCalledNullify = true;

        }
        else
        {
            // Debug.Log("swap called :" + reverse);
            List<PowerUpBase> playerPowerUps;
            if (turn == 1)
            {
                playerPowerUps = listPowerUp1.GetActivePowerUps();
            }
            else
            {
                playerPowerUps = listPowerUp2.GetActivePowerUps();
            }

            var swapPowerUp = playerPowerUps.FirstOrDefault(x => x.GetType().Equals("swap"));
            PowerUpTilesDto powerUpTilesDto = getPowerUpDto(ScoreA: scoreValA, ScoreB: scoreValB, PreviousScoreA: preScoreValA, PreviousScoreB: preScoreValB);
            powerUpTilesDto = swapPowerUp.Use(powerUpTilesDto);
            // reverse = powerUpTilesDto.Reverse;
            if (turn == 1)
            {
                listPowerUp1.DisablePowerUp(swapPowerUp);
            }
            else
            {
                listPowerUp2.DisablePowerUp(swapPowerUp);
            }

            takeSnapshot(powerUpTilesDto);
            initializePowerUp(); //to disable used powerup
        }
    }

    public void takeSnapshot(PowerUpTilesDto powerUpTilesDto)
    {
        preScoreValA = scoreValA;
        preScoreValB = scoreValB;
        for (int i = 0; i < tileArray.Length; i++)
        {
            int currentVal = powerUpTilesDto.TileArray[i].getVal();
            previousTileArray[i].setVal(currentVal);

        }
    }
    private PowerUpTilesDto getPowerUpDto(int tileIdToBeBlocked = -1, int tileIdToBeExploded = -1, int ScoreA = 0, int ScoreB = 0, int PreviousScoreA = 0, int PreviousScoreB = 0)
    {
        PowerUpTilesDto powerUpTilesDto = new PowerUpTilesDto();
        powerUpTilesDto.TileArray = new Tile[tileArray.Length];
        tileArray.CopyTo(powerUpTilesDto.TileArray, 0);
        powerUpTilesDto.PreviousTileArray = new Tile[tileArray.Length];
        tileArray.CopyTo(powerUpTilesDto.PreviousTileArray, 0);
        powerUpTilesDto.TileIdToBeBlocked = tileIdToBeBlocked;
        powerUpTilesDto.TileIdToBeExploded = tileIdToBeExploded;
        powerUpTilesDto.ScoreA = ScoreA;
        powerUpTilesDto.ScoreB = ScoreB;
        powerUpTilesDto.PreviousScoreA = PreviousScoreA;
        powerUpTilesDto.PreviousScoreB = PreviousScoreB;
        powerUpTilesDto.Reverse = reverse;
        return powerUpTilesDto;
    }

    public void initiateBlockPowerUp()
    {
        /* Debug.Log("count_null " + count_null);
        Debug.Log("isCalledBlock " + isCalledBlock);
 
        Block_time += Time.deltaTime;
 
        if (Block_time <= 5.0f)
        {
            Debug.Log(Block_time);
            isCalledBlock = true;
            Block_time = 0;
 
            Debug.Log("isBlockButtonClicked: " + isBlockButtonClicked);
 
            if (isBlockButtonClicked)
            {
                blockFlag = 1;
                if (turn == 1)
                {
                    playerWhoBlocked = 1;
 
                }
                else
                {
                    playerWhoBlocked = -1;
 
                }
 
            }
            Debug.Log("entered initiateBlockPowerUp");
        } */
        blockFlag = 1;
        if (turn == 1)
        {
            playerWhoBlocked = 1;

        }
        else
        {
            playerWhoBlocked = -1;

        }


    }

    /*public void isCheckBlockPowerUpAvailable()
    {
        List<PowerUpBase> playerPowerUps;
        if (turn == 1)
        {
            playerPowerUps = listPowerUp1.GetActivePowerUps();
        }
        else
        {
            playerPowerUps = listPowerUp2.GetActivePowerUps();
        }
        Debug.Log("Inside isCheckBlock" + playerPowerUps.FirstOrDefault(x => x.GetType().Equals("shield")));
        var checkBlockPowerUp = playerPowerUps.FirstOrDefault(x => x.GetType().Equals("shield"));
 
 
        if (checkBlockPowerUp != null)
        {
            initiateBlockPowerUp();
        }
 
    }*/

    public void blockTheCell(int indexToBeBlocked)
    {
        List<PowerUpBase> playerPowerUps;
        if (turn == 1)
        {
            playerPowerUps = listPowerUp1.GetActivePowerUps();
        }
        else
        {
            playerPowerUps = listPowerUp2.GetActivePowerUps();
        }

        var blockPowerUp = playerPowerUps.FirstOrDefault(x => x.GetType().Equals("shield"));
        Debug.Log(blockPowerUp);
        Debug.Log("Inside blockTheCell" + indexToBeBlocked);
        PowerUpTilesDto powerUpTilesDto = getPowerUpDto(indexToBeBlocked, -1, ScoreA: scoreValA, ScoreB: scoreValB, PreviousScoreA: preScoreValA, PreviousScoreB: preScoreValB);
        powerUpTilesDto = blockPowerUp.Use(powerUpTilesDto);
        powerUpTilesDto.TileArray.CopyTo(tileArray, 0);

        Debug.Log("entered blockTheCell");

        if (turn == 1)
        {
            listPowerUp1.DisablePowerUp(blockPowerUp);
        }
        else
        {
            listPowerUp2.DisablePowerUp(blockPowerUp);
        }

        isBlockButtonClicked = false;
        initializePowerUp(); //to disable used powerup
        enableTiles();
        turn *= -1;
        turnDisable();
        Debug.Log("Done with block cell.");
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void updateScore(int ind)
    {

        if (explodePowerUpInUse)
        {
            UseExplodePowerUp(ind);
            return;
        }
        if (isBlockButtonClicked)
        {

            blockTheCell(ind);
            blockSelect.gameObject.SetActive(false);
            return;
        }

        Debug.Log("Val: In Update Score");
        int i = ind;
        blockedIndex = i;

        var checkBlockTile1 = tileArray[blockedIndex].getIsBlocked();
        if (checkBlockTile1)
        {
            Debug.Log("Val: In CHECK BLOCK IN CHANGE TURN");
            if (playerWhoBlocked == turn)
            {
                Debug.Log("Val: In Unblock");
                tileArray[blockedIndex].setUnblock();
                Debug.Log("index is free else" + blockedIndex);
            }

        }

        if (!inTurn)
        {
            inHand.color = new Color32(255, 241, 118, 255);
            // acHand = gridArray[i];
            var checkBlockTile = tileArray[i % gridSize].getIsBlocked();
            if (checkBlockTile)
            {
                changeUserTurn();
                return;
            }
            acHand = tileArray[i % gridSize].getVal();
            if (acHand == 0)
            {
                // change user turn
                changeUserTurn();
                //turn *= -1;
                inHand.color = Color.white;
                //turnDisable();
                //checkWinCon();
                return;
            }
            inHand.GetComponentInChildren<Text>().text = "InHand: " + acHand;
            // gridArray[i] = 0;
            tileArray[i].setVal(0);
            inTurn = true;
        }
        else
        {

            // gridArray[i]++;
            var checkBlockTile = tileArray[i % gridSize].getIsBlocked();
            if (!checkBlockTile)
            {
                tileArray[i].setVal(tileArray[i % gridSize].getVal() + 1);
                inHand.GetComponentInChildren<Text>().text = "InHand: " + (--acHand);
            }


            if (acHand == 0)
            {
                inHand.color = Color.white;
                inTurn = false;
            }
        }
        enableTiles();
        disableTiles(i);
        // if (gridArray[(i + 1) % gridSize] == 0 && acHand == 0)

        if (reverse)
        {
            if (i == 0)
            {
                i = gridSize;
            }
            if (tileArray[(i - 1) % gridSize].getVal() == 0 && acHand == 0)
            {
                if (i - 1 == 0)
                {
                    i = gridSize + 1;
                }
                var checkBlockTile = tileArray[(i - 2) % gridSize].getIsBlocked();
                if (!checkBlockTile)
                {
                    if (turn == 1)
                    {
                        scoreValA += tileArray[(i - 2) % gridSize].getVal();
                        scoreA.GetComponentInChildren<Text>().text = "ScoreA: " + scoreValA;
                    }
                    else
                    {
                        scoreValB += tileArray[(i - 2) % gridSize].getVal();
                        scoreB.GetComponentInChildren<Text>().text = "ScoreB: " + scoreValB;
                    }
                    totalCoins -= tileArray[(i - 2) % gridSize].getVal();
                    tileArray[(i - 2) % gridSize].setVal(0);
                }

                // change user turn

                // checkBlock();
                changeUserTurn();

                //turn *= -1;
                //turnDisable();
                //checkWinCon();
            }
        }
        else if (tileArray[(i + 1) % gridSize].getVal() == 0 && acHand == 0)
        {
            var checkBlockTile = tileArray[(i + 2) % gridSize].getIsBlocked();
            if (!checkBlockTile)
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
            }

            // change user turn

            // checkBlock();
            changeUserTurn();
            //turn *= -1;
            //turnDisable();
            //checkWinCon();
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
            if ((!reverse && k != 1) || (reverse && k != gridSize - 1))
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
            win = true;
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
                string val = (tileArray[i].getVal()).ToString();
                tileArray[i].setText(val);
            }
        }
        // No. of buttons in the list != Number of Tiles (Check Initialization)
        else
        {
            Debug.Log("ERROR INITIALIZING TILES!");
            Debug.Log("Length of board != No. of tiles");
        }
    }

    public void undoClickedA()
    {
        isUndoClickedA = true;
        time_counter = 5.0f;

    }

    public void undoClickedB()
    {
        isUndoClickedB = true;
        time_counter = 5.0f;

    }

    public void changeUserTurn()
    {

        if ((turn == 1 && apList[0].interactable == true) || (turn == -1 && bpList[0].interactable == true))
        {
            if (turn * -1 == 1)
            {
                undoTextB.gameObject.SetActive(true);
            }
            else
            {
                undoTextA.gameObject.SetActive(true);
            }

            time_counter = 0;
            isCalledUndo = true;

        }
        else if ((turn == 1 && apList[4].interactable == true) || (turn == -1 && bpList[4].interactable == true))
        {
            // isCheckBlockPowerUpAvailable();
            time_counter = 0;
            isCalledBlock = true;
        }
        else
        {
            changeTurn();
        }

    }

    public void initializeUndoPowerUp()
    {
        Debug.Log("undo Initialized");

        isUndoClickedB = false;
        isUndoClickedA = false;

        List<PowerUpBase> playerPowerUps;
        if (turn == 1)
        {
            playerPowerUps = listPowerUp1.GetActivePowerUps();
        }
        else
        {
            playerPowerUps = listPowerUp2.GetActivePowerUps();
        }

        var undoPowerUp = playerPowerUps.FirstOrDefault(x => x.GetType().Equals("undo"));
        PowerUpTilesDto powerUpTilesDto = getPowerUpDto(ScoreA: scoreValA, ScoreB: scoreValB, PreviousScoreA: preScoreValA, PreviousScoreB: preScoreValB);
        powerUpTilesDto.PreviousTileArray = previousTileArray;
        powerUpTilesDto.TileArray = tileArray;
        powerUpTilesDto = undoPowerUp.Use(powerUpTilesDto);

        if (turn == 1)
        {
            listPowerUp1.DisablePowerUp(undoPowerUp);
        }
        else
        {
            listPowerUp2.DisablePowerUp(undoPowerUp);
        }

        initializePowerUp(); //to disable used powerup

        scoreA.GetComponentInChildren<Text>().text = "ScoreA: " + powerUpTilesDto.ScoreA;
        scoreB.GetComponentInChildren<Text>().text = "ScoreB: " + powerUpTilesDto.ScoreB;
        scoreValA = powerUpTilesDto.ScoreA;
        scoreValB = powerUpTilesDto.ScoreB;

        changeTurn();
        changeTurn();
    }

    void changeTurn()
    {

        preScoreValA = scoreValA;
        preScoreValB = scoreValB;
        for (int i = 0; i < tileArray.Length; i++)
        {
            int currentVal = tileArray[i].getVal();
            previousTileArray[i].setVal(currentVal);

        }

        turn *= -1;
        turnDisable();
        checkWinCon();
    }
    // Update is called once per frame
    void Update()
    {

        if (isCalledNullify)
        {

            time_counter += Time.deltaTime;

            if (time_counter >= 5.0f)
            {
                Debug.Log(time_counter);
                isCalledNullify = false;
                time_counter = 0;
                nullTextA.gameObject.SetActive(false);
                nullTextB.gameObject.SetActive(false);
                Debug.Log("isNullifyButtonClicked A : " + isNullifyButtonClickedA);
                Debug.Log("isNullifyButtonClicked B : " + isNullifyButtonClickedB);
                if (nullify_powerup == "explode")
                {

                    //ExplodedPowerUpInitiated();
                    if ((turn == 1 && isNullifyButtonClickedB) || (turn == -1 && isNullifyButtonClickedA))
                    {
                        initializeNullifyPowerUp(nullify_powerup);
                        nullify_powerup = "";
                        isPowerUpButtonClicked = false;
                    }
                    else
                    {
                        nullify_powerup = "";
                        explodePowerUpInUse = true;
                        explodeSelect.gameObject.SetActive(true);

                        explodeSelect.text = "Select the tile to explode! BOOM!";
                        turn *= -1; //to explode opponent's tile
                        turnDisable();
                        isPowerUpButtonClicked = false;
                    }
                } else if (nullify_powerup == "swap")
                {
                    if ((turn == 1 && isNullifyButtonClickedB) || (turn == -1 && isNullifyButtonClickedA))
                    {
                        initializeNullifyPowerUp(nullify_powerup);
                        nullify_powerup = "";
                        isPowerUpButtonClicked = false;
                    }
                    else
                    {
                        nullify_powerup = "";

                        // Debug.Log("swap called :" + reverse);
                        List<PowerUpBase> playerPowerUps;
                        if (turn == 1)
                        {
                            playerPowerUps = listPowerUp1.GetActivePowerUps();
                        }
                        else
                        {
                            playerPowerUps = listPowerUp2.GetActivePowerUps();
                        }

                        var swapPowerUp = playerPowerUps.FirstOrDefault(x => x.GetType().Equals("swap"));
                        PowerUpTilesDto powerUpTilesDto = getPowerUpDto(ScoreA: scoreValA, ScoreB: scoreValB, PreviousScoreA: preScoreValA, PreviousScoreB: preScoreValB);
                        powerUpTilesDto = swapPowerUp.Use(powerUpTilesDto);
                        // reverse = powerUpTilesDto.Reverse;
                        if (turn == 1)
                        {
                            listPowerUp1.DisablePowerUp(swapPowerUp);
                        }
                        else
                        {
                            listPowerUp2.DisablePowerUp(swapPowerUp);
                        }

                        takeSnapshot(powerUpTilesDto);
                        initializePowerUp(); //to disable used powerup

                    }
                } else if (nullify_powerup == "reverse")
                {
                    if ((turn == 1 && isNullifyButtonClickedB) || (turn == -1 && isNullifyButtonClickedA))
                    {
                        initializeNullifyPowerUp(nullify_powerup);
                        nullify_powerup = "";
                        isPowerUpButtonClicked = false;
                    }
                    else
                    {
                        nullify_powerup = "";
                        Debug.Log("reverse called :" + reverse);
                        List<PowerUpBase> playerPowerUps;
                        if (turn == 1)
                        {
                            playerPowerUps = listPowerUp1.GetActivePowerUps();
                        }
                        else
                        {
                            playerPowerUps = listPowerUp2.GetActivePowerUps();
                        }

                        var reversePowerUp = playerPowerUps.FirstOrDefault(x => x.GetType().Equals("reverse"));
                        PowerUpTilesDto powerUpTilesDto = getPowerUpDto(ScoreA: scoreValA, ScoreB: scoreValB, PreviousScoreA: preScoreValA, PreviousScoreB: preScoreValB);
                        powerUpTilesDto = reversePowerUp.Use(powerUpTilesDto);
                        reverse = powerUpTilesDto.Reverse;
                        if (turn == 1)
                        {
                            listPowerUp1.DisablePowerUp(reversePowerUp);
                        }
                        else
                        {
                            listPowerUp2.DisablePowerUp(reversePowerUp);
                        }
                        initializePowerUp(); //to disable used powerup
                    }
                }
            }

        }

        if (isCalledBlock)
        {
            time_counter += Time.deltaTime;
            if (time_counter >= 5.0f)
            {
                time_counter = 0;
                isCalledBlock = false;
                //undoTextA.gameObject.SetActive(false);
                //undoTextB.gameObject.SetActive(false);

                if (isBlockButtonClicked)
                {
                    initiateBlockPowerUp();

                }
                else
                {
                    changeTurn();
                }
            }
            Debug.Log("Val: Logging inside Timer");


        }


        if (isCalledUndo)
        {

            time_counter += Time.deltaTime;
            if (time_counter >= 5.0f)
            {
                isCalledUndo = false;
                undoTextA.gameObject.SetActive(false);
                undoTextB.gameObject.SetActive(false);

                if ((isUndoClickedB && turn == -1) || (isUndoClickedA && turn == 1))
                {
                    initializeUndoPowerUp();
                }
                else
                {
                    //preScoreValA = scoreValA;
                    //preScoreValB = scoreValB;
                    //for (int i = 0; i < tileArray.Length; i++)
                    //{
                    //    int currentVal = tileArray[i].getVal();
                    //    previousTileArray[i].setVal(currentVal);

                    //}
                    changeTurn();
                }
            }

        }

        updateBoard();
        handleReverseDirection();

        // Checks if the game has a winner
        if (!win)
        {
            if (turn == 1)
            {
                scoreA.color = new Color32(100, 221, 23, 255);
                scoreB.color = Color.white;
            }
            else
            {
                scoreB.color = new Color32(100, 221, 23, 255);
                scoreA.color = Color.white;
            }
        }
        else
        {
            scoreA.color = Color.white;
            scoreB.color = Color.white;
        }
    }


}