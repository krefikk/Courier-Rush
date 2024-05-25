using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectCarUIHandlerGarage : MonoBehaviour
{
    [Header("Car Prefab")]
    public GameObject carPrefab; // Car prefab we gonna display on screen

    [Header("Spawn On")]
    public Transform spawnOnTransform; // Parent object that has a mask linked to it

    [Header("Car Properties")]
    public GameObject speedStatBarObject;
    public GameObject gripStatBarObject;
    public GameObject durabilityStatBarObject;
    public GameObject accelerationStatBarObject;
    public GameObject firstCircleObject;
    public GameObject secondCircleObject;
    public GameObject thirdCircleObject;
    StatBar speedBar;
    StatBar durabilityBar;
    StatBar gripBar;
    StatBar accelerationBar;
    Image firstCircle;
    Image secondCircle;
    Image thirdCircle;

    bool changingCar = false; // Checks if car is changing at that moment
    CarUIHandler carUIHandler = null;

    // CarData array to store cars to display
    CarData[] carDatas;
    int selectedCarIndex = 0;

    void Start()
    {
        // Load all owned car datas
        carDatas = GameManager.gameManager.GetCars();
        BubbleSort(carDatas);
        // Initialize statistic bars
        speedBar = speedStatBarObject.GetComponent<StatBar>();
        durabilityBar = durabilityStatBarObject.GetComponent<StatBar>();
        gripBar = gripStatBarObject.GetComponent<StatBar>();
        accelerationBar = accelerationStatBarObject.GetComponent<StatBar>();
        // Initialize circle images
        firstCircle = firstCircleObject.GetComponent<Image>();
        secondCircle = secondCircleObject.GetComponent<Image>();
        thirdCircle = thirdCircleObject.GetComponent<Image>();
        // Display first car
        StartCoroutine(SpawnCar(true));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnClickedRightArrow();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnClickedLeftArrow();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            OnSelectCar();
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
        carUIHandler.SetUpCar(carDatas[selectedCarIndex], speedBar, accelerationBar, gripBar, durabilityBar, firstCircle, secondCircle, thirdCircle);
        carUIHandler.StartCarEntranceAnimation(isCarAppearOnRightSide);
        yield return new WaitForSeconds(0.25f); // 0.25 is animation's required time to complete
        changingCar = false;
    }

    public void OnClickedLeftArrow()
    {
        if (!changingCar)
        {
            selectedCarIndex--;
            if (selectedCarIndex < 0)
            {
                selectedCarIndex = carDatas.Length - 1;
            }
            // Spawn a new car from right
            StartCoroutine(SpawnCar(false));
        }
    }

    public void OnClickedRightArrow()
    {
        if (!changingCar)
        {
            selectedCarIndex++;
            if (selectedCarIndex > carDatas.Length - 1)
            {
                selectedCarIndex = 0;
            }
            // Spawn a new car from left
            StartCoroutine(SpawnCar(true));
        }
    }

    public void OnSelectCar()
    {
        PlayerPrefs.SetInt("SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainGame");
    }

    public void OnClickShop()
    {
        SceneManager.LoadScene("Shop");
    }

    void BubbleSort(CarData[] carDatas) // To sort cars according to their IDs
    {
        int n = carDatas.Length;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (carDatas[j].CarUniqueID > carDatas[j + 1].CarUniqueID)
                {
                    CarData temp = carDatas[j];
                    carDatas[j] = carDatas[j + 1];
                    carDatas[j + 1] = temp;
                }
            }
        }
    }
}
