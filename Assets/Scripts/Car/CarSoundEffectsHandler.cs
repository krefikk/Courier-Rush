using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSoundEffectsHandler : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource tireScreechingAudioSource;
    public AudioSource carEngineAudioSource;
    public AudioSource carHitAudioSource;

    float desiredEnginePitch = 0.5f;
    float tireScreechPitch = 0.5f;
    Car car;

    void Awake()
    {
        car = GetComponentInParent<Car>();
    }

    void Update()
    {
        UpdateEngineSFX();
        UpdateTireScreechingSFX();
    }

    void UpdateEngineSFX() 
    {
        float velocityMagnitude = car.GetVelocityMagnitude();
        // Handling engine volume according to car's velocity
        float desiredEngineVolume = velocityMagnitude * 0.05f;
        // Defining a minimum volume so player can always hear a sound
        desiredEngineVolume = Mathf.Clamp(desiredEngineVolume, 0.2f, 1.0f);
        // Car will slowly reach the maximum volume if accelerates regularly
        carEngineAudioSource.volume = Mathf.Lerp(carEngineAudioSource.volume, desiredEngineVolume, Time.deltaTime * 10);

        // Same operations but for engine's pitch
        desiredEnginePitch = velocityMagnitude * 0.2f;
        desiredEnginePitch = Mathf.Clamp(desiredEnginePitch, 0.5f, 2.0f);
        carEngineAudioSource.pitch = Mathf.Lerp(carEngineAudioSource.pitch, desiredEnginePitch, Time.deltaTime * 1.5f);
    }

    void UpdateTireScreechingSFX() 
    {
        // Tires are screeching
        if (car.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            if (isBraking || (car.GetBraking() && car.GetSpeed() > 5))
            {
                // Car is braking
                tireScreechingAudioSource.volume = Mathf.Lerp(tireScreechingAudioSource.volume, 1.0f, Time.deltaTime * 10f);
                tireScreechPitch = Mathf.Lerp(tireScreechPitch, 0.5f, Time.deltaTime * 10f);
            }
            else
            {
                // Car is drifting
                tireScreechingAudioSource.volume = Mathf.Abs(lateralVelocity) * 0.05f;
                tireScreechPitch = Mathf.Abs(lateralVelocity) * 0.1f;
            }
        }
        // Tires are not screeching
        else 
        {
            // Fading out the screech SFX slowly
            tireScreechingAudioSource.volume = Mathf.Lerp(tireScreechingAudioSource.volume, 0, Time.deltaTime * 10);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Getting the relative velocity of collision
        float relativeVelocity = collision.relativeVelocity.magnitude;
        // Handling volume of hit sound according to relative velocity
        float volume = relativeVelocity * 0.1f;
        carHitAudioSource.volume = volume;
        // Handling pitch of hit sound randomly in a small interval
        carHitAudioSource.pitch = Random.Range(0.95f, 1.05f);
        // Play the sound if it is not already playing
        if (!carHitAudioSource.isPlaying) 
        {
            carHitAudioSource.Play();
        }
    }
}
