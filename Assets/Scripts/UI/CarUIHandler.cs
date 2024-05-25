using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarUIHandler : MonoBehaviour
{

    [Header("Car Details")]
    public Image carImage;
    

    // Components
    Animator anim = null;

    void Awake()
    {
        // Animator component is not on any children but this also checks the current object
        anim = GetComponentInChildren<Animator>();
    }

    public void SetUpCar(CarData carData, StatBar speed, StatBar acc, StatBar grip, StatBar dura, Image firstCircle, Image secondCircle, Image thirdCircle) 
    {
        // Changing statistic bars
        Car car = carData.CarPrefab.GetComponent<Car>();
        speed.current = car.maxSpeedOnRealWorld;
        acc.current = car.accelerationFactor;
        grip.current = 6 / car.antiGrip;
        dura.current = car.carHealth;
        // Changing circles
        if (car.maxPackage == 1)
        {
            firstCircle.color = Color.white;
            secondCircle.color = Color.gray;
            thirdCircle.color = Color.gray;
        }
        else if (car.maxPackage == 2)
        {
            firstCircle.color = Color.white;
            secondCircle.color = Color.white;
            thirdCircle.color = Color.gray;
        }
        else 
        {
            firstCircle.color = Color.white;
            secondCircle.color = Color.white;
            thirdCircle.color = Color.white;
        }
        // Changing car sprite on screen
        carImage.sprite = carData.CarUISprite;
        // Setting the buy value on button
        GameObject buyButton = GameObject.FindGameObjectWithTag("BuyButton");
        if (buyButton != null) 
        {
            TextMeshProUGUI buyButtonValue = buyButton.GetComponentInChildren<TextMeshProUGUI>();
            buyButtonValue.text = carData.Price.ToString();
        }
    }

    public void StartCarEntranceAnimation(bool carAppearOnRightSide) 
    {
        if (carAppearOnRightSide)
        {
            anim.Play("carUiAppearFromRight");
        }
        else 
        {
            anim.Play("carUiAppearFromLeft");
        }
    }

    public void StartCarExitAnimation(bool carExitOnRightSide)
    {
        if (carExitOnRightSide)
        {
            anim.Play("carUiDisappearToRight");
        }
        else
        {
            anim.Play("carUiDisappearToLeft");
        }
    }

    public void OnCarExitAnimationCompleted() 
    {
        Destroy(gameObject);
    }

}
