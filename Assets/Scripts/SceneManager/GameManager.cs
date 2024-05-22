using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    bool hasSavedGame = true;
    List<int> carIDs;
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
    }

    public bool HasSavedGame() 
    {
        return hasSavedGame;
    }
}
