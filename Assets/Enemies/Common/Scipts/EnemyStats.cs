using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public StatsSystem stats;
    private void Start()
    {
        stats = new StatsSystem();
    }
    private void Update()
    {
        if (stats.GetCurrentHP() == 0) {
            Destroy(gameObject);
        }
    }
}
