using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("Car Properties")]
    float accelerationFactor = 5.0f;
    float turnFactor = 3.5f;

    [Header("Local Variables")]
    float accelerationInput = 0;
    float steeringInput = 0;
    float rotationAngle = 0; 
    Rigidbody2D rb; // To reach car's RigidBody2D component from here

    void Awake()
    {
        // Initializing RigidBody2D component of car
        rb = GetComponent<Rigidbody2D>();
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
        ApplySteering();
    }

    void ApplyEngineForce() 
    {
        // Creating a force that represents engine power
        Vector2 engineForce = transform.up * accelerationInput * accelerationFactor;
        //Pushing the car's body
        rb.AddForce(engineForce, ForceMode2D.Force);
    }

    void ApplySteering() 
    {
        // Calculate the rotation angle if it exists
        rotationAngle -= steeringInput * turnFactor;
        // Apply rotation to the car
        rb.MoveRotation(rotationAngle);
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

}
