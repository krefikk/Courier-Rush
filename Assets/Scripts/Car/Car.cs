using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("Car Properties")]
    float driftFactor = 0.95f;
    float accelerationFactor = 30.0f;
    float turnFactor = 3.5f;
    float maxSpeed = 20;

    [Header("Local Variables")]
    float accelerationInput = 0;
    float steeringInput = 0;
    float rotationAngle = 0;
    float velocityVsUp = 0;
    Rigidbody2D rb; // To reach car's RigidBody2D component from here

    void Awake()
    {
        // Initializing RigidBody2D component of car
        rb = GetComponent<Rigidbody2D>();
        rb.mass = 3;
    }

    void Update()
    {
        // Game loop for general functions
        GetInputs();
    }

    void FixedUpdate()
    {
        // Game loop for move related functions to display a smoother game
        ApplyEngineForce();
        KillOrthogonalVelocity();
        ApplySteering();
    }

    void ApplyEngineForce() 
    {
        // Calculating how much forward car is going
        velocityVsUp = Vector2.Dot(transform.up, rb.velocity);
        // Don't letting loop to run if car reached it's maximum speed for both forward or backward directions
        if (velocityVsUp > maxSpeed && accelerationInput > 0) 
        {
            return;
        }
        if (velocityVsUp < maxSpeed * 0.5f && accelerationInput < 0)
        {
            return;
        }
        if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
        {
            return;
        }
        // Applying drag to the car
        if (accelerationInput == 0)
        {
            rb.drag = Mathf.Lerp(rb.drag, 3.0f, Time.fixedDeltaTime * 3);
        }
        else 
        {
            rb.drag = 1;
        }
        // Creating a force that represents engine power
        Vector2 engineForce = transform.up * accelerationInput * accelerationFactor;
        //Pushing the car's body
        rb.AddForce(engineForce, ForceMode2D.Force);
    }

    void ApplySteering() 
    {
        // Limiting the car's ability to turn while moving slow
        float minSpeedToTurnFactor = rb.velocity.magnitude / 8;
        minSpeedToTurnFactor = Mathf.Clamp01(minSpeedToTurnFactor);
        // Calculate the rotation angle if it exists
        rotationAngle -= steeringInput * turnFactor * minSpeedToTurnFactor;
        // Apply rotation to the car
        rb.MoveRotation(rotationAngle);
    }

    void KillOrthogonalVelocity() 
    {
        // Prevents car to slide all the way while turning by ignoring orthogonal velocity
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.velocity, transform.right);
        rb.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    public void SetInputVector(Vector2 inputVector) 
    {
        // Setting car properties according to inputs
        accelerationInput = inputVector.x;
        steeringInput = inputVector.y;
    }

    void GetInputs()
    {
        // Getting move inputs
        float xAxis = Input.GetAxis("Vertical");
        float yAxis = Input.GetAxis("Horizontal");
        SetInputVector(new Vector2(xAxis, yAxis));
    }

    float GetLateralVelocity() 
    {
        // Returns how fast the car going sideways
        return Vector2.Dot(transform.right, rb.velocity);
    }

    public bool IsTireScreeching(out float lateralVelocity, out bool isBraking) 
    {
        lateralVelocity = GetLateralVelocity();
        isBraking = false;

        // Checks that is car breaking but still going forward
        if (accelerationInput < 0 && velocityVsUp > 0) 
        {
            isBraking = true;
            return true;
        }

        // Checks if lateral velocity is greater than a specific value
        if (Mathf.Abs(lateralVelocity) > 2.0f) 
        {
            return true;
        }

        return false;
    }

}
