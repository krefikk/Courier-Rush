using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        
    }

    public void SetUpCar(CarData carData) 
    {
        carImage.sprite = carData.CarUISprite;
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
