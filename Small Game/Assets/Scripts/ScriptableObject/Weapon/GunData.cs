using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class GunData : ScriptableObject
{
    public bool FixShootingIssue = false;
    public GameObject Bullet;
    public float fireForce;
    public float TimeBetweenShots;
    //BulletPerShot must be more than 1 and SpreadAngle must be more than 0 to perform multishot
    public float SpreadAngle;
    public int BulletPerShot;
    public float Damage;
}
