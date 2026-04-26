using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerProfile
{
    public float highScore;
    public int totalCoins;
    public List<string> unlockedUpgrades;

    public void SaveData()
    {
        // Logic to save High Score and Coins to local storage (JSON/PlayerPrefs)
        Debug.Log("Data Saved successfully.");
    }

    public void LoadData()
    {
        // Logic to load existing user data
    }
}