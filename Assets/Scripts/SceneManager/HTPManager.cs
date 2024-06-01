using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HTPManager : MonoBehaviour
{
    public void OnClickBack() 
    {
        AudioManager.audioManager.PlayClickSound();
        SceneManager.LoadScene("MainMenu");
    }
}
