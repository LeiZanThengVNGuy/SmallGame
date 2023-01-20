using UnityEngine;
[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyData : ScriptableObject
{
    [Header("GeneralVariables")]
    public bool isFighter = false;
    public bool isShooter = false;
    public float Speed = 5f;
    public float AttackRange = 5f;
    public float HP = 100f;
    public float SightRange = 20f;
    public int Points;
    //isShooter
    [Header("ShooterBuff")]
    public GameObject BulletPrefab;
    public bool FixShootingIssue = false;
    public float fireForce;
    public float TimeBetweenShots;
    public float ShootingDamage;
    //BulletPerShot must be more than 1 and SpreadAngle must be more than 0 to perform multishot
    public float SpreadAngle;
    public int BulletPerShot;
    [Header("FighterBuff")]
    //isFigter
    public bool FixDashingIssue;
    public float MeleeDamage;
    public float TimeBetweenDash;
    public float DashForce;
    public float DashTime;
    public float TimeBetweenAttack;
}
