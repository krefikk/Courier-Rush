using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    GameObject thingToFollow;
    public float viewDistance = 10f;

    private void Start()
    {
        thingToFollow = GameObject.FindGameObjectWithTag("PlayerCar");
    }

    void FixedUpdate()
    {
        transform.position = thingToFollow.transform.position + new Vector3(0, 0, -1 * viewDistance);
        
    }
}
