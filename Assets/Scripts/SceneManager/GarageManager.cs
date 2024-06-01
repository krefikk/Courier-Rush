using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GarageManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;

    private void Update()
    {
        DisplayMoney();
    }

    public void DisplayMoney() 
    {
        int money = (int) GameManager.gameManager.GetMoney();
        moneyText.text = money.ToString();
    }

    public void ReturnToMainMenu() 
    {
        GameManager.gameManager.CheckSavedGame();
        SceneManager.LoadScene("MainMenu");
    }
}
