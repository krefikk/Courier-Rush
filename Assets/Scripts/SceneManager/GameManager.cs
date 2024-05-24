using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    bool hasSavedGame = false;
    List<int> carIDs;
    CarData[] carDatas;
    int day = 1;
    int money = 0;
    
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

    private void Start()
    {
    }

    public bool HasSavedGame() 
    {
        return hasSavedGame;
    }

    public CarData[] GetCars() 
    {
        int a = 0;
        foreach (CarData car in carDatas) 
        {
            for (int i = 0; i < carIDs.Count; i++) 
            {
                if (carIDs[i] == car.CarUniqueID) 
                {
                    carDatas[a] = car;
                    a++;
                }
            }
        }
        return carDatas;
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
}
