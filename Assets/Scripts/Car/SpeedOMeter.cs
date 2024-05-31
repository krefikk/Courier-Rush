using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedOMeter : MonoBehaviour
{
    GameObject carObject;
    Car car;
    public Image needle;
    public Transform rotatePoint;
    float maxSpeed = 220;
    float maxNeedleRotation = -270;

    private void Start()
    {
        carObject = GameObject.FindGameObjectWithTag("PlayerCar");
        car = carObject.GetComponent<Car>();
    }

    private void Update()
    {
        FindNeedleRotation();
    }

    void FindNeedleRotation()
    {
        // Get the current speed of the car
        float speed = car.GetSpeed();
        // Normalize speed to a value between 0 and 1
        float normalizedSpeed = Mathf.Clamp01(speed / maxSpeed);
        // Calculate the needle rotation angle
        float needleRotation = Mathf.Lerp(0, maxNeedleRotation, normalizedSpeed);
        // Rotate the needle around the rotatePoint
        needle.rectTransform.RotateAround(rotatePoint.position, Vector3.forward, needleRotation - needle.rectTransform.localEulerAngles.z);
    }
}
