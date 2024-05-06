using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour
{
    [SerializeField]float steerSpeed=350;
    [SerializeField]float moveSpeed=15;
    [SerializeField]float bumpSpeed=10f;
    [SerializeField]float boostSpeed=20f;

    
    void Update()
    {
        float steerAmount=Input.GetAxis("Horizontal")*steerSpeed*Time.deltaTime;
        float moveAmount=Input.GetAxis("Vertical")*moveSpeed*Time.deltaTime;
        transform.Rotate(0,0,-steerAmount);
        transform.Translate(0,moveAmount,0);
    }
    void OnCollisionEnter2D(Collision2D other) {
        
            moveSpeed=bumpSpeed;
        
        
    }
   
   
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag=="Boost")
        {
       
        moveSpeed=boostSpeed;

        }

    }
   
}
