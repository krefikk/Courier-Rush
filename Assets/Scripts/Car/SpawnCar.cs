using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCar : MonoBehaviour
{
    GameObject spawnPoint;
    CarData[] carDatas;

    private void Awake()
    {
        // Debug log to check if Awake is being called twice
        Debug.Log("Awake called on SpawnCar instance ID: " + gameObject.name);

        carDatas = Resources.LoadAll<CarData>("CarData/");
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");

        if (spawnPoint == null)
        {
            Debug.LogError("Spawn point not found!");
            return;
        }

        Transform spawn = spawnPoint.transform;
        int selectedCarID = PlayerPrefs.GetInt("SelectedCarID", 0);
        Debug.Log("SelectedCarID: " + selectedCarID);

        foreach (CarData carData in carDatas)
        {
            if (carData.CarUniqueID == selectedCarID)
            {
                Debug.Log("Spawning car with ID: " + carData.CarUniqueID);
                GameObject car = Instantiate(carData.CarPrefab, spawn.position, spawn.rotation);
                break;
            }
        }
    }
}
