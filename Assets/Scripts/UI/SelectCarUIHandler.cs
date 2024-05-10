using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCarUIHandler : MonoBehaviour
{
    [Header("Car Prefab")]
    public GameObject carPrefab; // Car prefab we gonna display on screen

    [Header("Spawn On")]
    public Transform spawnOnTransform; // Parent object that has a mask linked to it

    bool changingCar = false; // Checks if car is changing at that moment
    CarUIHandler carUIHandler = null;

    // CarData array to store cars to display
    CarData[] carDatas;
    int selectedCarIndex = 0;

    void Start()
    {
        // Load all car datas
        carDatas = Resources.LoadAll<CarData>("CarData/");
        SpawnCar(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) 
        { 
            OnClickedLeftArrow();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnClickedRightArrow();
        }
    }

    IEnumerator SpawnCar(bool isCarAppearOnRightSide) 
    {
        changingCar = true;
        // If there is already a car on the scene, first remove it
        if (carUIHandler != null)
        {
            carUIHandler.StartCarExitAnimation(!isCarAppearOnRightSide);
        }    
        // Create the new car prefab and play it's animation
        GameObject instatiatedCar = Instantiate(carPrefab, spawnOnTransform);
        carUIHandler = instatiatedCar.GetComponent<CarUIHandler>();
        carUIHandler.SetUpCar(carDatas[selectedCarIndex]);
        carUIHandler.StartCarEntranceAnimation(isCarAppearOnRightSide);
        yield return new WaitForSeconds(0.25f); // 0.25 is animation's required time to complete
        changingCar = false;
    }

    void OnClickedLeftArrow() 
    {
        if (!changingCar) 
        {
            selectedCarIndex--;
            if (selectedCarIndex < 0)
            {
                selectedCarIndex = carDatas.Length - 1;
            }
            // Spawn a new car from right
            StartCoroutine(SpawnCar(true));
        }
    }

    void OnClickedRightArrow() 
    {
        if (!changingCar) 
        {
            selectedCarIndex++;
            if (selectedCarIndex > carDatas.Length - 1)
            {
                selectedCarIndex = 0;
            }
            // Spawn a new car from left
            StartCoroutine(SpawnCar(false));
        }
    }
}
