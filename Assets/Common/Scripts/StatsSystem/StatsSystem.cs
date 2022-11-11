using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsSystem
{
    private float agility = 1.5f;
    private float strength = 1;
    private float endurance = 1;
    private float ability = 1;
    private float luck = 1;
    private float fistRange = 1;

    private int currentHP;

    public StatsSystem()
    {
        currentHP = GetMaxHP();
    }
    public float GetSpeed()
    {
        return 5 + agility * 0.2f;
    }
    public float GetFistRange()
    {
        return fistRange;
    }

    public int GetCurrentHP() {
        return currentHP;
    }
    public int GetMaxHP() {
        return (int)(Mathf.Log(2, 1 + endurance) * 30);
    }
    public int GetMeleeDamage() {
        return -(int)(Mathf.Log(2, 1 + strength) * 5);
    }
    public void ChangeHP(int n) 
    {
        currentHP=Mathf.Clamp(currentHP+n, 0, GetMaxHP());
    }
}
