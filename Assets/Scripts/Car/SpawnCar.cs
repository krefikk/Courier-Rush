using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCar : MonoBehaviour
{
    GameObject spawnPoint;
    CarData[] carDatas;

    private void Awake()
    {
        carDatas = Resources.LoadAll<CarData>("CarData/");
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
        Transform spawn = spawnPoint.transform;
        int selectedCarID = PlayerPrefs.GetInt("SelectedCarID", 0);
        foreach (CarData carData in carDatas) 
        {
            if (carData.CarUniqueID == selectedCarID) 
            {
                GameObject car = Instantiate(carData.CarPrefab, spawn.position, spawn.rotation);
                break;
            }
        }
    }
}
