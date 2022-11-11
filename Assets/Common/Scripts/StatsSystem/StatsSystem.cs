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

    private int maxHP;
    private int currentHP;
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
        return (int)(endurance * 1);
    
    }
}
