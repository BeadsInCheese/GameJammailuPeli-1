using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage = 1;
    public float attackSpeed = 0.5f;
    public float range = 5;
    public GameObject bulletPrefab = null;
    public Transform muzzle = null;
}
