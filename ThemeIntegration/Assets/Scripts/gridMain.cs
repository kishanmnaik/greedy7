using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System.Timers;

using System;
using System.Text;
using System.IO;

using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

[RequireComponent(typeof(Button))]

public class gridMain : MonoBehaviour
{
    public static int gridSize = 10;
    public static int initialNumOfBeads = 3;
    public int totalCoins = gridSize * initialNumOfBeads;

    public Button[] btnList;

    public Button[] apList, bpList;

    public Sprite[] picture_tile;

    int[] gridArray = new int[gridSize];
    public Text winText, inHand, explodeSelect;
    public Text scoreA, scoreB;

    public Text nullTextA, nullTextB;
    public bool isPowerUpButtonClicked = false;

    public int scoreValA = 0, scoreValB = 0, acHand = 0;
    public int preScoreValA = 0, preScoreValB = 0;

    private Tile[] tileArray = new Tile[gridSize];
    private Tile[] previousTileArray = new Tile[gridSize];

    public Image dirCW, dirCCW;

    private bool reverse;

    public int turn = 0, falseTurn = 0;
    public bool inTurn = false;
    public bool win = false;

    private PowerUps listPowerUp1 = new PowerUps();
    private PowerUps listPowerUp2 = new PowerUps();

    private bool explodePowerUpInUse = false, shieldPowerUpInUse = false;

    /* public Image undoImg;
    public int undoQ = 0; */

    public int blockedIndex = -1;

    public Image[] pQtyImgA;
    public Image[] pQtyImgB;

    public Sprite[] spriteQtyList;

    public int[] pQtyListA = { 0, 0, 0, 0, 0, 0 }, pQtyListB = { 0, 0, 0, 0, 0, 0 };

    public GameObject endScreen;

    public Image disableLayer;
    public GameObject allPopUps, nullPop, undoPop, shieldPop;

    private bool undoCancel = false, undoSuccess = false, shieldCancel = false;

    private int totalTurns = 0;
    private float totalTime = 0.0f;
    private StringBuilder powerUpList = new StringBuilder();
    private StringBuilder usedPowerUpList = new StringBuilder();

    public Image[] inHands;

    // private int -380 = -380, 300 = 300;

    private string pSettings = "Assets/Resources/nums/config.txt";

    // Start is called before the first frame update
    void Start()
    {
        var coinToss = new coinToss();
        turn = coinToss.getTurn();
        falseTurn = turn;

        Debug.Log("Turn: " + turn);
        winText.gameObject.SetActive(false);
        explodeSelect = GameObject.Find("explodeSelect").GetComponent<Text>();
        explodeSelect.gameObject.SetActive(false);

        initializeGrid();
        turnDisable();

        listPowerUp1 = Loading.allP1;
        listPowerUp2 = Loading.allP2;

        // Tracking Analytics: (Handling) Power Up List 
        foreach (var px in listPowerUp1.GetActivePowerUps())
        {
            string pTempText = px.GetType();
            powerUpList.Append(pTempText + "|");
        }

        foreach (var px in listPowerUp2.GetActivePowerUps())
        {
            string pTempText = px.GetType();
            powerUpList.Append(pTempText + "|");
        }

        Debug.Log(powerUpList.ToString());

        initializePowerUp();

        endScreen.gameObject.SetActive(false);
        allPopUps.gameObject.SetActive(false);

        inHands[0].gameObject.SetActive(false);
        inHands[1].gameObject.SetActive(false);

        setInHand();

        lSettings();

        disableLayer.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
    }

    void lSettings()
    {
        try
        {
            StreamReader sr = new StreamReader(pSettings);
            string line = sr.ReadLine();

            //LoadSettings ls = new LoadSettings();
            //ls = ls.getJSONObject(line);

            Debug.Log("CONFIG LINE: " + line);
            //Debug.Log("JSON String: " + ls.beadTheme);

            string acPath = "nums/" + line;
            Debug.Log("PATH: " + line);           

            object[] tempSprites = Resources.LoadAll(acPath, typeof(Sprite));
            picture_tile = new Sprite[tempSprites.Length];
            Debug.Log("Sprites Length: " + tempSprites.Length);

            for (var x = 0; x < tempSprites.Length; x++)
            {
                picture_tile[x] = (Sprite) tempSprites[x];
            }

            sr.Close();
        }
        catch (Exception e)
        {
            Debug.Log("Lol doesn't exist buddy.");
        }
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

    void initializeQty()
    {
        for (int i = 0; i < 6; i++)
        {
            pQtyListA[i] = 0;
            pQtyListB[i] = 0;
        }

        foreach (var px in listPowerUp1.GetActivePowerUps())
        {
            string pTempText = px.GetType();
            if (pTempText.Contains("undo"))
            {
                pQtyListA[0]++;
            }
            else if (pTempText.Contains("swap"))
            {
                pQtyListA[1]++;
            }
            else if (pTempText.Contains("explode"))
            {
                pQtyListA[2]++;
            }
            else if (pTempText.Contains("reverse"))
            {
                pQtyListA[3]++;
            }
            else if (pTempText.Contains("shield"))
            {
                pQtyListA[4]++;
            }
            else if (pTempText.Contains("nullify"))
            {
                pQtyListA[5]++;
            }
        }

        foreach (var px in listPowerUp2.GetActivePowerUps())
        {
            string pTempText = px.GetType();
            if (pTempText.Contains("undo"))
            {
                pQtyListB[0]++;
            }
            else if (pTempText.Contains("swap"))
            {
                pQtyListB[1]++;
            }
            else if (pTempText.Contains("explode"))
            {
                pQtyListB[2]++;
            }
            else if (pTempText.Contains("reverse"))
            {
                pQtyListB[3]++;
            }
            else if (pTempText.Contains("shield"))
            {
                pQtyListB[4]++;
            }
            else if (pTempText.Contains("nullify"))
            {
                pQtyListB[5]++;
            }
        }

        /* for (int i = 0; i < 6; i++)
        {
            Debug.Log("QTY A" + (i + 1) + ": " +pQtyListA[i]);
            Debug.Log("QTY B" + (i + 1) + ": " + pQtyListB[i]);
        } */

        setQtyImage();
    }

    void setQtyImage()
    {
        for (int i = 0; i < 6; i++)
        {
            if (pQtyListA[i] > 0)
                pQtyImgA[i].sprite = spriteQtyList[pQtyListA[i] - 1];
            else
                pQtyImgA[i].gameObject.SetActive(false);

            if (pQtyListB[i] > 0)
                pQtyImgB[i].sprite = spriteQtyList[pQtyListB[i] - 1];
            else
                pQtyImgB[i].gameObject.SetActive(false);
        }
    }

    void powerListInteractable(bool both = true, int a = 0)
    {
        if (both)
        {
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
        } else
        {
            if (a == 1)
            {
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
            } else
            {
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
        }
    }

    void initializePowerUp()
    {
        disablePowerUp();
        initializeQty();
        checkTurnPowerUp();
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
    }

    void takeSnapShot(PowerUpTilesDto powerUpTilesDto)
    {
        preScoreValA = scoreValA;
        preScoreValB = scoreValB;
        for (int i = 0; i < tileArray.Length; i++)
        {
            int currentVal = powerUpTilesDto.TileArray[i].getVal();
            previousTileArray[i].setVal(currentVal);
        }
    }


    public void ExplodedPowerUpInitiated()
    {
        isPowerUpButtonClicked = true;

        explodePowerUpInUse = true;
        explodeSelect.gameObject.SetActive(true);

        explodeSelect.text = "Select the tile to explode! BOOM!";
        turn *= -1; //to explode opponent's tile
        turnDisable();

        isPowerUpButtonClicked = false;
    }

    public void ShieldPowerUpInitiated()
    {
        isPowerUpButtonClicked = true;

        shieldPowerUpInUse = true;

        explodeSelect.gameObject.SetActive(true);
        explodeSelect.text = "Select the tile to shield!";

        shieldPop.transform.localPosition = new Vector3(0, Screen.height + 200, 0);
        allPopUps.gameObject.SetActive(false);

        turn *= -1;
        enableTiles();
        turnDisable();

        isPowerUpButtonClicked = false;
    }

    public void UseShieldPowerUp(int index)
    {
        try
        {
            tileArray[blockedIndex].setUnblock();
        } catch (Exception e)
        {

        }
        
        blockedIndex = index;

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

        var shieldPowerUp = playerPowerUps.FirstOrDefault(x => x.GetType().Equals("shield"));
        PowerUpTilesDto powerUpTilesDto = getPowerUpDto(index, -1, ScoreA: scoreValA, ScoreB: scoreValB, PreviousScoreA: preScoreValA, PreviousScoreB: preScoreValB);
        powerUpTilesDto = shieldPowerUp.Use(powerUpTilesDto);

        if (turn == 1)
        {
            listPowerUp1.DisablePowerUp(shieldPowerUp);
        }
        else
        {
            listPowerUp2.DisablePowerUp(shieldPowerUp);
        }

        usedPowerUpList.Append("shield|");

        takeSnapShot(powerUpTilesDto);
        initializePowerUp();

        turn *= -1;
        checkTurnPowerUp();
        turnDisable();
        checkWinCon();

        shieldCancel = true;
        shieldPowerUpInUse = false;
        EventSystem.current.SetSelectedGameObject(null);
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

        usedPowerUpList.Append("explode|");

        takeSnapShot(powerUpTilesDto);
        initializePowerUp(); //to disable used powerup
        enableTiles();

        turn *= -1;
        checkTurnPowerUp();
        turnDisable();
        checkWinCon();

        explodePowerUpInUse = false;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void UseReversePowerUp()
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
        usedPowerUpList.Append("reverse|");
        initializePowerUp(); //to disable used powerup

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void UseUndoPowerUp()
    {
        turn *= -1;

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
        powerUpTilesDto = undoPowerUp.Use(powerUpTilesDto);

        if (turn == 1)
        {
            listPowerUp1.DisablePowerUp(undoPowerUp);
        }
        else
        {
            listPowerUp2.DisablePowerUp(undoPowerUp);
        }

        scoreA.GetComponentInChildren<Text>().text = "ScoreA: " + powerUpTilesDto.ScoreA;
        scoreB.GetComponentInChildren<Text>().text = "ScoreB: " + powerUpTilesDto.ScoreB;
        scoreValA = powerUpTilesDto.ScoreA;
        scoreValB = powerUpTilesDto.ScoreB;

        // undoPop.transform.localPosition = new Vector3(0, -380, 0);
        allPopUps.gameObject.SetActive(false);

        undoSuccess = true;
        undoCancel = true;

		usedPowerUpList.Append("undo|");

        // turn *= -1;
        checkTurnPowerUp();
        turnDisable();
        checkWinCon();

        initializePowerUp();
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void UseSwapPowerUp()
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

        if (turn == 1)
        {
            listPowerUp1.DisablePowerUp(swapPowerUp);
        }
        else
        {
            listPowerUp2.DisablePowerUp(swapPowerUp);
        }

        usedPowerUpList.Append("swap|");

        initializePowerUp(); //to disable used powerup

        takeSnapShot(powerUpTilesDto);
        EventSystem.current.SetSelectedGameObject(null);
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

    public void setInHand()
    {
        if (acHand > 16)
            return;
        if (turn == 1)
        {
            inHands[0].sprite = picture_tile[acHand];

            inHands[0].gameObject.SetActive(true);
            inHands[1].gameObject.SetActive(false);
        } else
        {
            inHands[1].sprite = picture_tile[acHand];

            inHands[0].gameObject.SetActive(false);
            inHands[1].gameObject.SetActive(true);
        }
        if (win)
        {
            inHands[0].gameObject.SetActive(false);
            inHands[1].gameObject.SetActive(false);
        }
    }

    public void updateScore(int ind)
    {
        disablePowerUp();

        if (explodePowerUpInUse)
        {
            UseExplodePowerUp(ind);
            return;
        }

        if (shieldPowerUpInUse)
        {
            UseShieldPowerUp(ind);
            return;
        }

        // Debug.Log("Score A: " + scoreValA);

        int i = ind;
        if (!inTurn)
        {
            // inHand.color = new Color32(255, 241, 118, 255);
            // acHand = gridArray[i];
            if (!tileArray[i].getIsBlocked())
                acHand = tileArray[i].getVal();
            if (acHand == 0)
            {
                // change user turn
                // changeUserTurn();
                changeTurn();
                // turn *= -1;
                // inHand.color = Color.white;
                // turnDisable();
                // checkWinCon();
                return;
            }

            // inHand.GetComponentInChildren<Text>().text = "InHand: " + acHand;
            setInHand();
            // gridArray[i] = 0;
            if (!tileArray[i].getIsBlocked())
                tileArray[i].setVal(0);
            inTurn = true;
        }
        else
        {
            // gridArray[i]++;
            if (!tileArray[i].getIsBlocked())
            {
                tileArray[i].setVal(tileArray[i].getVal() + 1);
                // inHand.GetComponentInChildren<Text>().text = "InHand: " + (--acHand);
                --acHand;
                setInHand();
            } else
            {
                blockedIndex = i;
            }
            if (acHand == 0)
            {
                // inHand.color = Color.white;
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
                if (turn == 1)
                {
                    preScoreValA = scoreValA;
                    if (!tileArray[(i - 2) % gridSize].getIsBlocked())
                        scoreValA += tileArray[(i - 2) % gridSize].getVal();
                    scoreA.GetComponentInChildren<Text>().text = "ScoreA: " + scoreValA;
                }
                else
                {
                    preScoreValB = scoreValB;
                    if (!tileArray[(i - 2) % gridSize].getIsBlocked())
                        scoreValB += tileArray[(i - 2) % gridSize].getVal();
                    scoreB.GetComponentInChildren<Text>().text = "ScoreB: " + scoreValB;
                }
                if (!tileArray[(i - 2) % gridSize].getIsBlocked())
                {
                    totalCoins -= tileArray[(i - 2) % gridSize].getVal();
                    tileArray[(i - 2) % gridSize].setVal(0);
                }
                // change user turn
                // changeUserTurn();
                changeTurn();
                //turn *= -1;
                //turnDisable();
                //checkWinCon();
            }
        }
        else if (tileArray[(i + 1) % gridSize].getVal() == 0 && acHand == 0)
        {
            if (turn == 1)
            {
                // scoreValA += gridArray[(i + 2) % gridSize];
                preScoreValA = scoreValA;
                if (!tileArray[(i + 2) % gridSize].getIsBlocked())
                    scoreValA += tileArray[(i + 2) % gridSize].getVal();
                scoreA.GetComponentInChildren<Text>().text = "ScoreA: " + scoreValA;
            }
            else
            {
                // scoreValB += gridArray[(i + 2) % gridSize];
                preScoreValB = scoreValB;
                if (!tileArray[(i + 2) % gridSize].getIsBlocked())
                    scoreValB += tileArray[(i + 2) % gridSize].getVal();
                scoreB.GetComponentInChildren<Text>().text = "ScoreB: " + scoreValB;
            }
            // totalCoins -= gridArray[(i + 2) % gridSize
            if (!tileArray[(i + 2) % gridSize].getIsBlocked())
            {
                totalCoins -= tileArray[(i + 2) % gridSize].getVal();
                // gridArray[(i + 2) % gridSize] = 0;
                tileArray[(i + 2) % gridSize].setVal(0);
            }
            // change user turn
            // changeUserTurn();
            // changeUserTurn();
            changeTurn();
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
                if (tileArray[j].getIsBlocked() && acHand == 0)
                {
                    changeTurn();
                    return;
                }
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

            //Disables PowerUps
            disablePowerUp();

            allPopUps.gameObject.SetActive(false);
            endScreen.transform.position -= new Vector3(0, 900, 0);
            endScreen.gameObject.SetActive(true);

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

                if (tileArray[i].getVal() > 16)
                {
                    tileArray[i].setText(val);
                    tileArray[i].setPicture(null);
                }
                else
                {
                    // Debug.Log(tileArray[i].getVal() + "?");
                    tileArray[i].setText("");
                    tileArray[i].setPicture(picture_tile[tileArray[i].getVal()]);
                }

                if (tileArray[i].getIsBlocked())
                {
                    // Debug.Log("IS BLOCKED");
                    ColorBlock tileColor = tileArray[i].getButton().colors;
                    tileColor.highlightedColor = new Color32(239, 83, 80, 255);
                    tileColor.normalColor = new Color32(229, 115, 115, 255);
                    tileColor.pressedColor = new Color32(211, 47, 47, 255);
                    tileColor.disabledColor = new Color32(229, 115, 115, 255);
                    tileArray[i].getButton().colors = tileColor;
                } else
                {
                    // Debug.Log("IS NOT BLOCKED");
                    ColorBlock tileColor = tileArray[i].getButton().colors;
                    tileColor.highlightedColor = new Color32(38, 189, 218, 255);
                    tileColor.normalColor = new Color32(255, 255, 255, 255);
                    tileColor.pressedColor = new Color32(0, 172, 193, 255);
                    tileColor.disabledColor = new Color32(200, 200, 200, 128);
                    tileArray[i].getButton().colors = tileColor;
                }
            }
        }
        // No. of buttons in the list != Number of Tiles (Check Initialization)
        else
        {
            Debug.Log("ERROR INITIALIZING TILES!");
            Debug.Log("Length of board != No. of tiles");
        }
    }

    void disablePowerUp()
    {
        foreach (var btnTemp in apList)
        {
            btnTemp.interactable = false;
        }

        foreach (var btnTemp in bpList)
        {
            btnTemp.interactable = false;
        }
    }

    void checkTurnPowerUp()
    {
        // Debug.Log("CheckTurnPowerUp: " + turn);
        disablePowerUp();
        if (turn == 1)
        {
            powerListInteractable(false, 1);
        } else
        {
            powerListInteractable(false, -1);
        }
    }

    public void checkPowerUps(List<PowerUpBase> playerPowerUps, int z)
    {
        bool undoTest = false, shieldTest = false;

        var undoPowerUp = playerPowerUps.FirstOrDefault(x => x.GetType().Equals("undo"));
        var shieldPowerUp = playerPowerUps.FirstOrDefault(x => x.GetType().Equals("shield"));

        if (undoPowerUp != null)
            undoTest = true;
        if (shieldPowerUp != null)
            shieldTest = true;

        if (undoTest)
        {
            allPopUps.gameObject.SetActive(true);
            // undoPop.transform.position += new Vector3(0, 389, 0);
            
            Debug.Log("A1: " + undoPop.transform.position);
            if (z != 1)
            {
                undoPop.transform.localPosition = new Vector3(0, -10, 0);
                undoPop.transform.eulerAngles = new Vector3(0, 0, 180);
            }
            else
            {
                undoPop.transform.localPosition = new Vector3(0, 9, 0);
                undoPop.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            // Debug.Log("A2: " + undoPop.transform.eulerAngles.z);
            undoPop.gameObject.SetActive(true);
            undoCancel = false;
            StartCoroutine(waitForUndo(shieldTest, z));
            return;
        } else if (shieldTest)
        {
            Debug.Log("TEST0");
            shieldCancel = false;
            allPopUps.gameObject.SetActive(true);
            if (z != 1)
            {
                shieldPop.transform.localPosition = new Vector3(0, -10, 0);
                shieldPop.transform.eulerAngles = new Vector3(0, 0, 180);
            }
            else
            {
                shieldPop.transform.localPosition = new Vector3(0, 9, 0);
                shieldPop.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            shieldPop.gameObject.SetActive(true);
            StartCoroutine(waitForShield());
            return;
        }

        storePrevValues();
    }

    IEnumerator waitForUndo(bool shieldTest, int z)
    {
        Debug.Log("TEST1");
        yield return new WaitUntil(() => undoCancel != false);
        Debug.Log("TEST2");
        if (!undoSuccess)
        {
            Debug.Log("TEST-1");
            storePrevValues();
            shieldCancel = false;
            if (shieldTest)
            {
                // turn *= -1;
                allPopUps.gameObject.SetActive(true);
                if (z != 1)
                {
                    shieldPop.transform.localPosition = new Vector3(0, -10, 0);
                    shieldPop.transform.eulerAngles = new Vector3(0, 0, 180);
                }
                else
                {
                    shieldPop.transform.localPosition = new Vector3(0, 9, 0);
                    shieldPop.transform.eulerAngles = new Vector3(0, 0, 0);
                }
                shieldPop.gameObject.SetActive(true);
                StartCoroutine(waitForShield());
            }
        }
    }

    IEnumerator waitForShield()
    {
        Debug.Log("TEST3");
        yield return new WaitUntil(() => shieldCancel != false);
        Debug.Log("TEST4");
    }

    void changeTurn()
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

        // checkUndo(playerPowerUps);
        // checkShield(playerPowerUps);

        checkPowerUps(playerPowerUps, turn);

        Debug.Log("After Player Power Ups Check!");
       
        if (blockedIndex != -1)
        {
            Debug.Log("Blocked Index in Changed Turn: " + blockedIndex);
            tileArray[blockedIndex].setUnblock();
            blockedIndex = -1;
        } else
        {
            Debug.Log("Blocked Index is -1");
        }

        totalTurns++;
        turn *= -1;
        checkTurnPowerUp();
        turnDisable();
        checkWinCon();

        setInHand();
    }

    public void storePrevValues()
    {
        preScoreValA = scoreValA;
        preScoreValB = scoreValB;
        for (int i = 0; i < tileArray.Length; i++)
        {
            int currentVal = tileArray[i].getVal();
            previousTileArray[i].setVal(currentVal);
        }
    }

    public void cancel()
    {
        try
        {
            string cancelName = EventSystem.current.currentSelectedGameObject.name;
            
            if (cancelName.Contains("undoCancel"))
            {
                if (turn == 1)
                {
                    undoPop.transform.localPosition = new Vector3(0, 300, 0);
                } else
                {
                    undoPop.transform.localPosition = new Vector3(0, -380, 0);
                }
                undoSuccess = false;
                undoCancel = true;
            } else if (cancelName.Contains("nullifyCancel"))
            {
                if (turn == 1)
                {
                    nullPop.transform.localPosition = new Vector3(0, 300, 0);
                }
                else
                {
                    nullPop.transform.localPosition = new Vector3(0, -380, 0);
                }
            } else if (cancelName.Contains("shieldCancel"))
            {
                if (turn == 1)
                {
                    shieldPop.transform.localPosition = new Vector3(0, 300, 0);
                }
                else
                {
                    shieldPop.transform.localPosition = new Vector3(0, -380, 0);
                }
                shieldCancel = true;
            }
        } catch (Exception e)
        {

        }

        allPopUps.gameObject.SetActive(false);
        
        // turn *= -1;
        checkTurnPowerUp();
        turnDisable();
        checkWinCon();
    }

    public void gotoMainMenu()
    {
        /* try
        {
            UnityEditor.EditorApplication.isPlaying = false;
        } catch (Exception e)
        {
            Application.Quit();
        } */
        sendAnalytics();
        SceneManager.LoadScene(0);
    }

    public void restartGame()
    {
        sendAnalytics();
        SceneManager.LoadScene(1);
    }

    public void sendAnalytics()
    {
        // KD
        string pListNU = powerUpList.ToString(), pListU = usedPowerUpList.ToString();
        Debug.Log(pListNU);
        Debug.Log(pListU);
        Analytics.CustomEvent("sendAllData",
            new Dictionary<string, object>
            {
                {"totalTurns", totalTurns},
                {"totalTime", (int)(totalTime)},
                {"powerUpList", pListNU},
                {"usedPowerUpList", pListU}
            }
        );

        TrackingAnalytics ta = new TrackingAnalytics();
        Debug.Log(totalTurns + " " + (int)(totalTime) + " " + powerUpList.ToString() + " " + usedPowerUpList.ToString());
        ta.updateFile(totalTurns, (int)(totalTime), powerUpList.ToString(), usedPowerUpList.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        updateBoard();
        handleReverseDirection();

        // Checks if the game has a winner
        if (!win)
        {
        	totalTime += Time.deltaTime;

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
