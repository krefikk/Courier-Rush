using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class GameData
{
    private int money = 0;
    List<int> carIDs = new List<int>();
    int day = 1;
    int shopsNeed = 3000;

    public GameData(int money, List<int> carIDs, int day, int shopsNeed)
    {
        this.money = money;
        this.carIDs = carIDs;
        this.day = day;
        this.shopsNeed = shopsNeed;
    }

    public int GetMoney() { return money; }
    public int GetDay() { return day; }
    public List<int> GetCarIDs() {  return carIDs; }
    public int GetShopsNeed() { return shopsNeed; }
    public void SetMoney(int money) { this.money = money; }
    public void SetDay(int day) {  this.day = day; }
    public void SetCarIDs(List<int> carIDs) {  this.carIDs = carIDs; }
    public void SetShopsNeed(int shopsNeed) {  this.shopsNeed = shopsNeed;}

}
