using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Player Singleton
    public static Player player;

    void Awake()
    {
        if (player != null && player != this)
        {
            Destroy(gameObject);
        }
        else
        {
            player = this;
        }
    }

    void Update()
    {
        GetInputs();
    }

    void GetInputs() 
    {
    }
}
