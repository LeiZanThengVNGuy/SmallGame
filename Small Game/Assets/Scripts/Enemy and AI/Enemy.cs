using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    bool PlayerSpotted;
    public EnemyData CurrentEnemyData;
    public Transform Target;
    private Rigidbody2D rb;
    private float CurrentHP;
    //isShooterVariables
    private Transform firePoint;
    bool AllowShoot;
    private float EnemyCurrentAngle;
    //isFigterVariables
    bool AllowDash;
    bool isDashing;
    bool AllowHurtsPlayer;
    //patrol variables
    PatrolPointData PatrolPointsDat;
    int RandomPatrolPoint;
    Vector2 Destination;
    public float TimeTilNextPatrol = 0.3f;
    //pointsCounter
    PointsCounter PointCounterInstance;
    //buff purpose
    PlayerMovement playerScript;
    //particle and stuff
    Color startColor;
    Color endColor = Color.white;
    float HurtDuration = 0.08f;
    //particle
    public GameObject DeadParticle;
    //sound
    AudioSource audioSource;
    AudioClip Argh;
    AudioClip EnemyDie;
    void Start()
    {
        Target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        rb = this.GetComponent<Rigidbody2D>();
        CurrentHP = CurrentEnemyData.HP;
        AllowShoot = true;
        firePoint = gameObject.GetComponent<Transform>();
        AllowDash = true;
        AllowHurtsPlayer = true;
        PatrolPointsDat = GameObject.FindWithTag("PatrolPointList").GetComponent<PatrolPointData>();
        SetPatrolPoint();
        PointCounterInstance = GameObject.FindGameObjectWithTag("PointsCounter").GetComponent<PointsCounter>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        startColor = gameObject.GetComponent<SpriteRenderer>().color;
        DeadParticle.GetComponent<ParticleSystem>().startColor = gameObject.GetComponent<SpriteRenderer>().color;
        audioSource = GetComponent<AudioSource>();
        Argh = Resources.Load<AudioClip>("EnemyHurt");
        audioSource.volume = 1f;
        EnemyDie = Resources.Load<AudioClip>("EnemyDie");
    }
   void Update()
   {    
        //check if enemy see player
        if(Vector2.Distance(transform.position, Target.position) > CurrentEnemyData.SightRange || !playerScript.IsBait)
        {
            PlayerSpotted = false;
        }
        if(Vector2.Distance(transform.position, Target.position) <= CurrentEnemyData.SightRange || playerScript.IsBait)
        {
            PlayerSpotted = true;
            Debug.Log("pLAYER sPOTTED");
        }
        //rotate enemy
        if(!isDashing)
        {
            Vector3 direction = Target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
            direction.Normalize();
            EnemyCurrentAngle = angle;
            //kill enemy
            if(CurrentHP <= 0)
            {
                //gives player points
                PointCounterInstance.AddScore(CurrentEnemyData.Points);
                Instantiate(DeadParticle, transform.position, Quaternion.identity);
                audioSource.PlayOneShot(EnemyDie);
                Destroy(gameObject, 0.005f);
            }
        }
        //move enemy
        if(PlayerSpotted)
        {
            //movement and attack
            if(!isDashing && !(CurrentEnemyData.isShooter && Vector2.Distance(transform.position, Target.position) <= CurrentEnemyData.AttackRange))
            {
                moveCharacter();
            }
            if(Vector2.Distance(transform.position, Target.position) <= CurrentEnemyData.AttackRange)
            {
                //attack
                if(CurrentEnemyData.isShooter)
                {
                    if(AllowShoot && !isDashing)
                    {
                        EnemyFire();
                    }
                }
                if(CurrentEnemyData.isFighter)
                {
                    if(AllowDash)
                    {
                        StartCoroutine("Dash");
                    }
                }
            }
        }
        //enemy patrol
        if(!PlayerSpotted)
        {
            Patrolling();
        }
    }
    void moveCharacter()
    {
        transform.position = Vector2.MoveTowards(transform.position, Target.position, CurrentEnemyData.Speed * Time.deltaTime);
    }
    public void TakeDamage(float DamageTaken)
    {
        CurrentHP -= DamageTaken;
        HurtColor();
        audioSource.PlayOneShot(Argh);
    }
    void EnemyFire()
    {
        //perform multishot
        if(CurrentEnemyData.SpreadAngle != 0 && CurrentEnemyData.BulletPerShot > 1)
        {
            float StartRotation = EnemyCurrentAngle + CurrentEnemyData.SpreadAngle / 2f; 
            float AngleIncrease = CurrentEnemyData.SpreadAngle / ((float)CurrentEnemyData.BulletPerShot - 1f);
            for(int i = 0; i < CurrentEnemyData.BulletPerShot; i++)
            {
                float TempRotation = StartRotation - AngleIncrease * i;
                GameObject Projectile = Instantiate(CurrentEnemyData.BulletPrefab, firePoint.position, Quaternion.Euler(0f,0f,TempRotation));
                if(!CurrentEnemyData.FixShootingIssue)
                {
                    Projectile.GetComponent<Rigidbody2D>().AddForce(Projectile.transform.up * CurrentEnemyData.fireForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
                }
                if(CurrentEnemyData.FixShootingIssue)
                {
                    Projectile.GetComponent<Rigidbody2D>().AddForce(Projectile.transform.right * CurrentEnemyData.fireForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
                }
                //set the projectile damage
                Projectile.GetComponent<EnemyBullet>().Damage = CurrentEnemyData.ShootingDamage;
            }
        }
        //perform normal shot
        else
        {
            GameObject ProjectileWithoutSpread = Instantiate(CurrentEnemyData.BulletPrefab, firePoint.position, firePoint.rotation);
            if(!CurrentEnemyData.FixShootingIssue)
            {
                ProjectileWithoutSpread.GetComponent<Rigidbody2D>().AddForce(ProjectileWithoutSpread.transform.right * CurrentEnemyData.fireForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
            }
            if(CurrentEnemyData.FixShootingIssue)
            {
                ProjectileWithoutSpread.GetComponent<Rigidbody2D>().AddForce(ProjectileWithoutSpread.transform.up * CurrentEnemyData.fireForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
            }
            //also set the projectile damage
            ProjectileWithoutSpread.GetComponent<EnemyBullet>().Damage = CurrentEnemyData.ShootingDamage;
        }
        AllowShoot = false;
        Invoke("ResetShot", CurrentEnemyData.TimeBetweenShots);
    }
    void ResetShot()
    {
        AllowShoot = true;
    }
    //fighter attack
    void OnCollisionStay2D(Collision2D other) 
    {
        if((other.gameObject.tag == "Player") && (CurrentEnemyData.isFighter))
        {
            AllowDash = false;
            if(AllowHurtsPlayer)
            {
                other.gameObject.GetComponent<PlayerMovement>().GetHurt(CurrentEnemyData.MeleeDamage);
                AllowHurtsPlayer = false;
                Invoke("ResetMeleeAttack", CurrentEnemyData.TimeBetweenAttack);
            }
        }
    }
    IEnumerator Dash()
    {  
        if(!CurrentEnemyData.FixDashingIssue && AllowDash)
        {
            rb.velocity = transform.up * CurrentEnemyData.DashForce;
        }
        if(CurrentEnemyData.FixDashingIssue && AllowDash)
        {
            rb.velocity = transform.right * CurrentEnemyData.DashForce;
        }
        isDashing = true;
        yield return new WaitForSeconds(CurrentEnemyData.DashTime);
        AllowDash = false;
        isDashing = false;
        yield return new WaitForSeconds(CurrentEnemyData.TimeBetweenDash);
        AllowDash = true;
    }
    void ResetMeleeAttack()
    {
        AllowHurtsPlayer = true;
    }
    //patrol Stuff
    void Patrolling()
    {
        transform.position = Vector2.MoveTowards(transform.position, PatrolPointsDat.PatrolPoints[RandomPatrolPoint].position, CurrentEnemyData.Speed * Time.deltaTime);
        if(Vector2.Distance(transform.position,PatrolPointsDat.PatrolPoints[RandomPatrolPoint].position) < 0.2f)
        {
            Invoke("SetPatrolPoint", TimeTilNextPatrol);
        }
    }
    void SetPatrolPoint()
    {
        RandomPatrolPoint = Random.Range(0, PatrolPointsDat.PatrolPoints.Length);
    }
    //change color when get hit
    void HurtColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = endColor;
        Invoke("ReturnNormalColor", HurtDuration);
    }
    void ReturnNormalColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = startColor;
    }
}