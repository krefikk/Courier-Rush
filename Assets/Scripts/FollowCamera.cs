using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] GameObject thingToFollow;
    public float viewDistance = 10f;

    void FixedUpdate()
    {
        transform.position = thingToFollow.transform.position + new Vector3(0, 0, -1 * viewDistance);
        
    }
}
