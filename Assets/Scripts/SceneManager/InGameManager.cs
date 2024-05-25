using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameManager : MonoBehaviour
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
}
