using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile
{
    private int val;
    private bool isBlocked = false;
    private int id;
    private bool isEnabled = true;
    private Button button;

    public Tile(int val, int id, Button button)
    {
        this.val = val;
        this.id = id;
        this.button = button;
    }

    public void setVal(int val)
    {
        this.val = val;
    }

    public void setText(string value)
    {
        this.button.GetComponentInChildren<Text>().text = value;
    }

    public void setBlock()
    {
        this.isBlocked = true;
    }

    public void setEnable()
    {
        this.isEnabled = true;
        this.button.interactable = this.isEnabled;
    }

    public void setUnblock()
    {
        this.isBlocked = false;
    }

    public void setDisable()
    {
        this.isEnabled = false;
        this.button.interactable = this.isEnabled;
    }

    public void toggleBlock()
    {
        this.isBlocked = !this.isBlocked;
    }

    public void toggleEnable()
    {
        this.isEnabled = !this.isEnabled;
        this.button.interactable = this.isEnabled;
    }

    public int getVal()
    {
        return this.val;
    }

    public int getId()
    {
        return this.id;
    }

    public bool getIsBlocked()
    {
        return isBlocked;
    }

    public bool getIsEnabled()
    {
        return isEnabled;
    }

    public Button getButton()
    {
        return this.button;
    }
}
