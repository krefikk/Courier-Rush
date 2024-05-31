using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton
    public static AudioManager audioManager;
    
    // SFX
    public AudioSource clickSoundEffect;
    public AudioSource packageDeliveredSoundEffect;
    public AudioSource completeDeliverySoundEffect;
    public AudioSource deliveryCancelSoundEffect;
    public AudioSource gameOverSoundEffect;
    public AudioSource dayCompletedSoundEffect;
    public AudioSource moneySoundEffect;
    public AudioSource deliveryHUDSoundEffect;

    private void Awake()
    {
        if (audioManager == null)
        {
            audioManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayClickSound()
    {
        clickSoundEffect.Play();
    }

    public void PlayPackageDeliveredSound()
    {
        packageDeliveredSoundEffect.Play();
    }

    public void PlayDeliveryCompletedSound()
    {
        completeDeliverySoundEffect.Play();
    }

    public void PlayCancelDeliverySound()
    {
        deliveryCancelSoundEffect.Play();
    }

    public void PlayGameOverSound()
    {
        gameOverSoundEffect.Play();
    }

    public void PlayDayCompletedSound()
    {
        dayCompletedSoundEffect.Play();
    }

    public void PlayMoneySound()
    {
        moneySoundEffect.Play();
    }

    public void PlayDeliveryHUDSound() 
    {
        deliveryHUDSoundEffect.Play();
    }
}
