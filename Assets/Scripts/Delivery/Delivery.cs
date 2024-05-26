using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery
{
    GameObject shop;
    GameObject dropPoint;
    bool isChosen = false;
    float distanceToShop;
    float worth;
    bool delivered = false;
    bool canceled = false;
    public Delivery(GameObject shop, GameObject dropPoint) 
    {
        this.shop = shop;
        SetDropPoint(dropPoint);
        CalculateWorth();
    }

    public void SetChosen() 
    {
        isChosen = true;
    }

    public void SetAsDelivered()
    {
        delivered = true;
    }

    public bool IsChosen() 
    {
        return isChosen;
    }

    public bool IsDelivered() 
    {
        return delivered;
    }

    public float GetDistance() { return distanceToShop; }
    public float GetWorth() { return worth; }
    public GameObject GetDropPoint() 
    {
        return dropPoint;
    }

    public void SetDropPoint(GameObject dropPoint) 
    {
        this.dropPoint = dropPoint;
        distanceToShop = Vector2.Distance(this.dropPoint.transform.position, shop.transform.position);
    }

    public void CalculateWorth() 
    {
        if (distanceToShop > 0) 
        {
            worth = (int) (distanceToShop * 1.5f);
        }
    }
}
