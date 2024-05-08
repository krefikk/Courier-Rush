using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarUIHandler : MonoBehaviour
{

    Animator anim = null;

    void Awake()
    {
        // Animator component is not on any children but this also checks the current object
        anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        
    }

    public void StartCarEntranceAnimation(bool carAppearOnRightSide) 
    {
        if (carAppearOnRightSide)
        {
            anim.Play("Car UI Appear From Right");
        }
        else 
        {
            anim.Play("Car UI Appear From Left");
        }
    }

    public void StartCarExitAnimation(bool carExitOnRightSide)
    {
        if (carExitOnRightSide)
        {
            anim.Play("Car UI Disappear To Right");
        }
        else
        {
            anim.Play("Car UI Disappear To Left");
        }
    }

    public void OnCarExitAnimationCompleted() 
    {
        Destroy(gameObject);
    }

}
