using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Transform PlayerTranform;
    private SpriteRenderer CurrentSprite;
    public GunData CurrentGunData;
    private Transform firePoint;
    bool Shooting = false;
    bool AllowShoot;
    //for some buff
    [HideInInspector] public float TempAttackSpeedReduce = 1f;
    [HideInInspector] public float TempDamageBuff;
    [HideInInspector] public bool HaveTheAttackSpeedReduce = false;
    //sound
    AudioSource AudioSrc;
    AudioClip GunShootingSound;
    void Start()
    {
        AllowShoot = true;
        firePoint = gameObject.GetComponent<Transform>();
        CurrentSprite = GetComponent<SpriteRenderer>();
        PlayerTranform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        GunShootingSound = Resources.Load<AudioClip>("RealShooting");
        AudioSrc = GameObject.Find("GunSound").GetComponent<AudioSource>();
    }
    void Update()
    {
        Shooting = Input.GetButton("Fire1");
        if(Shooting && AllowShoot && Time.timeScale != 0)
        {
            Fire();
        }
        //flip Weapon
        float Rotation;
        if(PlayerTranform.eulerAngles.z <= 180f)
        {
            Rotation = PlayerTranform.eulerAngles.z;
        }
        else
        {
            Rotation = PlayerTranform.eulerAngles.z - 360f;
        }
        if(Rotation > 0f && (!(gameObject.name == "Bow") && !(gameObject.name == "Slingshot")))
        {
            CurrentSprite.flipY = true;
        }
        else
        {
            CurrentSprite.flipY = false;
        }
    }
    //BulletPerShot must be more than 1 and SpreadAngle must be more than 0 to perform multishot
    void Fire()
    {
        AudioSrc.PlayOneShot(GunShootingSound);
        //perform multishot
        if(CurrentGunData.SpreadAngle != 0 && CurrentGunData.BulletPerShot > 1)
        {
            float StartRotation = PlayerMovement.CurrentAngle + CurrentGunData.SpreadAngle / 2f; 
            float AngleIncrease = CurrentGunData.SpreadAngle / ((float)CurrentGunData.BulletPerShot - 1f);
            for(int i = 0; i < CurrentGunData.BulletPerShot; i++)
            {
                float TempRotation = StartRotation - AngleIncrease * i;
                GameObject Projectile = Instantiate(CurrentGunData.Bullet, firePoint.position, Quaternion.Euler(0f,0f,TempRotation));
                Projectile.GetComponent<Rigidbody2D>().AddForce(Projectile.transform.up * CurrentGunData.fireForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
                Projectile.GetComponent<Bullet>().Damage = CurrentGunData.Damage + TempDamageBuff;
            }
        }
        //perform normal shot
        else
        {
            GameObject ProjectileWithoutSpread = Instantiate(CurrentGunData.Bullet, firePoint.position, firePoint.rotation);
            if(!CurrentGunData.FixShootingIssue)
            {
                ProjectileWithoutSpread.GetComponent<Rigidbody2D>().AddForce(ProjectileWithoutSpread.transform.right * CurrentGunData.fireForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
            }
            if(CurrentGunData.FixShootingIssue)
            {
                ProjectileWithoutSpread.GetComponent<Rigidbody2D>().AddForce(ProjectileWithoutSpread.transform.up * CurrentGunData.fireForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
            }
            ProjectileWithoutSpread.GetComponent<Bullet>().Damage = CurrentGunData.Damage + TempDamageBuff;
        }
        AllowShoot = false;
        if(!HaveTheAttackSpeedReduce)
        {
            Invoke("ResetShot", CurrentGunData.TimeBetweenShots);
        }
        if(HaveTheAttackSpeedReduce)
        {
            Invoke("ResetShot", CurrentGunData.TimeBetweenShots / TempAttackSpeedReduce);
        }
    }
    void ResetShot()
    {
        AllowShoot = true;
    }
}
