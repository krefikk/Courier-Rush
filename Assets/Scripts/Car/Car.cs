using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("Car Properties")]
    float driftFactor = 0.95f;
    public float accelerationFactor = 20.0f;
    public float turnFactor = 3.5f;
    float maxSpeed = 20; // Vector magnitude one
    public float maxSpeedOnRealWorld = 180; // Real speed
    float maxSpeedFactor;
    public float antiGrip = 16; // If get's bigger, grip decreases
    float maxCarHealth = 100;
    public float carHealth = 100;
    public float maxPackage = 1;
    bool braking = false;

    [Header("Local Variables")]
    float accelerationInput = 0;
    float steeringInput = 0;
    float rotationAngle = 0;
    float velocityVsUp = 0;
    Rigidbody2D rb; // To reach car's RigidBody2D component from here

    [Header("Scene Managers")]
    InGameManager inGameManager;

    void Awake()
    {
        // Initializing RigidBody2D component of car
        rb = GetComponent<Rigidbody2D>();
        rb.mass = 3;
        // Initializing in game manager
        inGameManager = FindAnyObjectByType<InGameManager>();
    }

    private void Start()
    {
        maxSpeedFactor = maxSpeed / 8.5f;
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
        if (velocityVsUp < maxSpeed * -0.5f && accelerationInput < 0)
        {
            return;
        }
        if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
        {
            return;
        }
        if (GetSpeed() >= maxSpeedOnRealWorld)
        {
            return;
        }

        if (braking)
        {
            // Apply a strong drag to simulate braking
            rb.drag = 16.0f / antiGrip;
            return;
        }
        else if (accelerationInput == 0)
        {
            rb.drag = Mathf.Lerp(rb.drag, 3.0f, Time.fixedDeltaTime);
        }
        else
        {
            rb.drag = 1;
        }
        // Creating a force that represents engine power
        Vector2 engineForce = transform.up * accelerationInput * maxSpeedFactor * accelerationFactor;
        //Pushing the car's body
        rb.AddForce(engineForce, ForceMode2D.Force);
    }

    void ApplySteering()
    {
        // Limiting the car's ability to turn while moving slow
        float minSpeedToTurnFactor = rb.velocity.magnitude / antiGrip;
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
        accelerationInput = inputVector.y;
        steeringInput = inputVector.x;
    }

    void GetInputs()
    {
        // Getting move inputs
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        SetInputVector(new Vector2(xAxis, yAxis));
        // Getting brake input
        braking = Input.GetKey(KeyCode.Space);
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

        if (braking) { return true; }

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

    public float GetSpeed()
    {
        return Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.y, 2);
    }

    public float GetVelocityMagnitude()
    {
        return rb.velocity.magnitude;
    }

    public bool GetBraking()
    {
        return braking;
    }

    public float getCurrentHealth()
    {
        return carHealth;
    }

    public float GetMaxHealth()
    {
        return maxCarHealth;
    }

    public float GetDamage()
    {
        return maxCarHealth - carHealth;
    }

    public void SetDamage(float value)
    {
        carHealth = maxCarHealth - value;
        if (carHealth < 0) { carHealth = 0; }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        float damage = GetSpeed() / 6;
        if (damage <= 3)
        {
            damage = 0;
        }
        carHealth -= damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DropPoint")) 
        {
            foreach (Delivery delivery in inGameManager.deliveriesToDisplay) 
            {
                if (delivery.GetDropPoint() == collision.gameObject) 
                {
                    // Delivery sound
                    delivery.SetAsDelivered();
                    inGameManager.HandleArrowPointersOnDelivery(); // For changing the color and direction of arrow pointer on delivery
                }
            }
        }
        if (collision.CompareTag("SpawnPoint")) 
        {
            inGameManager.OnArriveToShop();
        }
    }

}