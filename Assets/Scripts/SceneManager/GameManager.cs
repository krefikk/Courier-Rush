using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public static SaveHandler saveHandler;
    bool hasSavedGame = true;
    List<int> carIDs = new List<int>{ 0 };
    CarData[] carDatas;
    int day = 1;
    int money = 10000;
    int shopsNeed = 7500;
    
    private void Awake()
    {
        saveHandler = new SaveHandler(Application.persistentDataPath, "courierrush.game");
        if (gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        carDatas = Resources.LoadAll<CarData>("CarData/");
        GameData gameData = saveHandler.Load();
        if (gameData == null)
        { // Means there is no saved game
            hasSavedGame = false;
        }
        else 
        { // If there is a saved game, load the saved data
            hasSavedGame = true;
            carIDs = gameData.GetCarIDs();
            day = gameData.GetDay();
            money = gameData.GetMoney();
            shopsNeed = gameData.GetShopsNeed();
        }
    }

    private void Update()
    {
    }

    public bool HasSavedGame() 
    {
        return hasSavedGame;
    }

    public CarData[] GetCars() 
    {
        int a = 0;
        Debug.Log("Element count: " +carIDs.Count);
        CarData[] ownedCarDatas = new CarData[carIDs.Count];
        foreach (CarData car in carDatas)
        {
            for (int i = 0; i < carIDs.Count; i++)
            {
                if (carIDs[i] == car.CarUniqueID)
                {
                    ownedCarDatas[a] = car;
                    a++;
                }
            }
        }
        return ownedCarDatas;
    }

    public bool CheckForCar(int ID) 
    {
        if (carIDs != null)
        {
            foreach (int carID in carIDs)
            {
                if (carID == ID) { return true; }
            }
            return false;
        }
        else { return false; }      
    }

    public void AddCar(CarData car) 
    {
        carIDs.Add(car.CarUniqueID);
    }

    public float GetMoney() { return money; }
    public int GetCurrentDay() { return day; }

    public int GetShopsNeed() { return shopsNeed; }
    public void IncreaseDay() { day += 1; } // Increases day by one

    public void DecreaseMoney(int amount) // Decreases money by given amount
    {
        StartCoroutine(DecreaseMoneyCO(amount));
    }

    public void SetShopsNeed(int amount) 
    {
        shopsNeed = amount;
    }

    IEnumerator DecreaseMoneyCO(int amount)
    {
        AudioManager.audioManager.PlayMoneySound();
        if (money < amount)
        {
            Debug.LogWarning("Money Error");
            amount = money;
        }
        int targetMoney = money - amount;
        int decrementStep = Mathf.Max(1, amount / 100); // Adjust the step based on the amount
        float updateInterval = 0.01f; // Time interval between each decrement step

        while (money > targetMoney)
        {
            money -= decrementStep;
            if (money < targetMoney)
            {
                money = targetMoney; // Ensure the money doesn't go below the target
            }
            yield return new WaitForSeconds(updateInterval);
        }
    }
    public void IncreaseMoney(int amount) // Increases money by given amount
    {
        StartCoroutine(IncreaseMoneyCO(amount));
    }

    IEnumerator IncreaseMoneyCO(int amount)
    {
        AudioManager.audioManager.PlayMoneySound();
        int targetMoney = money + amount;
        int incrementStep = Mathf.Max(1, amount / 100); // Adjust the step based on the amount
        float updateInterval = 0.01f; // Time interval between each decrement step

        while (money < targetMoney)
        {
            money += incrementStep;
            if (money > targetMoney)
            {
                money = targetMoney; // Ensure the money doesn't go below the target
            }
            yield return new WaitForSeconds(updateInterval);
        }
    }

    public void SaveGame() 
    {
        GameData gameData = new GameData(money, carIDs, day, shopsNeed);
        saveHandler.Save(gameData);
    }

    public void DeleteSave() 
    {
        saveHandler.DeleteSaveFile();
        money = 0;
        day = 1;
        carIDs = new List<int> { 0 };
        hasSavedGame = false;
    }

    public void CheckSavedGame() 
    {
        GameData gameData = saveHandler.Load();
        if (gameData == null)
        { // Means there is no saved game
            hasSavedGame = false;
        }
        else
        { // If there is a saved game, load the saved data
            hasSavedGame = true;
            carIDs = gameData.GetCarIDs();
            day = gameData.GetDay();
            money = gameData.GetMoney();
        }
    }

    public void ResetToDefault() 
    {
        hasSavedGame = false;
        money = 0;
        day = 1;
        carIDs = new List<int> { 0 };
        shopsNeed = 7500;
    }
}
