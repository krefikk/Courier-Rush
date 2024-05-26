using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    bool hasSavedGame = true;
    List<int> carIDs = new List<int>{ 0 };
    CarData[] carDatas;
    int day = 1;
    int money = 10000;
    
    private void Awake()
    {
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
    public void IncreaseDay() { day += 1; } // Increases day by one

    public void DecreaseMoney(int amount) // Decreases money by given amount
    {
        StartCoroutine(DecreaseMoneyCO(amount));
    }

    IEnumerator DecreaseMoneyCO(int amount)
    {
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
        int targetMoney = money + amount;
        int incrementStep = Mathf.Max(1, amount / 100); // Adjust the step based on the amount
        float updateInterval = 0.01f; // Time interval between each decrement step

        while (money > targetMoney)
        {
            money += incrementStep;
            if (money > targetMoney)
            {
                money = targetMoney; // Ensure the money doesn't go below the target
            }
            yield return new WaitForSeconds(updateInterval);
        }
    }
}
