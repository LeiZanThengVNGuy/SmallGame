using UnityEngine;
using System.Collections;

// BachNob was here owo

public class PlayerMovement : MonoBehaviour
{
    public GameObject PausedScreen;
    public HealthBar healthBar;
    public Rigidbody2D rb;
    public Camera Cam;
    public float MoveSpeed;
    [HideInInspector] public Vector2 MoveDir;
    Vector2 MousePos;
    public float MaxHP = 100f;
    [HideInInspector]public float HP;
    static public float CurrentAngle;
    //gameover
    public GameScreen gameScreen;
    //inverse controller variables
    [HideInInspector] public float InverseControllerVar = 1f;
    //bait debuff
    [HideInInspector] public bool IsBait = false;
    //sound
    AudioSource AudioSrc;
    AudioClip OuchSound;
    //hurt effect
    SpriteRenderer PlayerSpriteRenderer;
    private void Start() {
        HP = MaxHP;
        healthBar.SetMaxHealth(MaxHP);
        AudioSrc = GetComponent<AudioSource>();
        OuchSound = Resources.Load<AudioClip>("hitHurt");
        PlayerSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        PlayerInput();
        
        MousePos = Cam.ScreenToWorldPoint(Input.mousePosition);
        //kill player
        if(HP <= 0)
        {
            gameScreen.GameOver();
            gameObject.SetActive(false);
        }
        //show health
        healthBar.SetHealth(HP);
        //pause the game
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }
    void PlayerInput()
    {
        float MoveX = Input.GetAxisRaw("Horizontal");
        float MoveY = Input.GetAxisRaw("Vertical");
        MoveDir = new Vector2(MoveX, MoveY).normalized * InverseControllerVar;
    }
    void FixedUpdate() 
    {
        Move();
        Vector2 LookDir = MousePos - rb.position;
        float Angle = Mathf.Atan2(LookDir.y, LookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = Angle;
        CurrentAngle = Angle;
    }
    void Move()
    {
        rb.velocity = MoveDir * MoveSpeed;
    }
    public void GetHurt(float Damage)
    {
        HP -= Damage;
        AudioSrc.PlayOneShot(OuchSound);
        StartCoroutine("HurtEffect");
    }
    //pause
    public void PauseGame()
    {
        PausedScreen.SetActive(true);
        Time.timeScale = 0;
    }
    public void Resume()
    {
        AudioSrc.PlayOneShot(GameScreen.ClickSound);
        PausedScreen.SetActive(false);
        Time.timeScale = 1;
    }
    //hurt effect
    IEnumerator HurtEffect()
    {
        float BlinkingSpeed = 0.1f;
        PlayerSpriteRenderer.enabled = false;
        yield return new WaitForSeconds(BlinkingSpeed);
        PlayerSpriteRenderer.enabled = true;
        yield return new WaitForSeconds(BlinkingSpeed);
        PlayerSpriteRenderer.enabled = false;
        yield return new WaitForSeconds(BlinkingSpeed);
        PlayerSpriteRenderer.enabled = true;
    }
}
