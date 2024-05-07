using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTrailRenderHandler : MonoBehaviour
{
    Car car;
    TrailRenderer trailRenderer;

    void Awake()
    {
        // Getting the car and trail renderer components
        car = GetComponentInParent<Car>();
        trailRenderer = GetComponent<TrailRenderer>();
        // Not displaying trail at the start of the game
        trailRenderer.emitting = false;
    }

    void Update()
    {
        if (car.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            trailRenderer.emitting = true;
        }
        else 
        {
            trailRenderer.emitting = false;
        }
    }
}
