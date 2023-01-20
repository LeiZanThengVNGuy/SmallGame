using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuffAndDebuff : MonoBehaviour
{
    public TextMeshProUGUI BuffText;
    public PlayerMovement playerScript;
    //countdown for buff changing
    [SerializeField] float CountDownTime = 10f;
    float time = 0f;
    bool ChangeBuff;
    int BuffIndex;
    //regen
    bool AllowRegen = false;
    //sound
    AudioSource SoundPlayer;
    AudioClip DebuffSound;
    AudioClip BuffSound;
    AudioClip RickRoll;
    void Start()
    {
        ChangeBuff = true;
        SoundPlayer = GetComponent<AudioSource>();
        DebuffSound = Resources.Load<AudioClip>("DebuffSound");
        BuffSound = Resources.Load<AudioClip>("BuffSound");
        RickRoll = Resources.Load<AudioClip>("RickRoll");
    }
    void Update()
    {
        //get the enemy script
        //idea to fix the issue: should add the isBait to player (also make it public) in order to make all enemy spotted player without using any loop or something complicate
        //ChangeBuffWhenCountDownIsOver
        if(ChangeBuff)
        {
            //to know how many time we change the index
            int ChangeIndexTime = 0;
            ChangeBuff = false;
            BuffIndex = Random.Range(0,13); // the maxExclusive is the largests number case + 1
            ChangeIndexTime++;
            Debug.Log(ChangeIndexTime);
            switch(BuffIndex)
            {
                case 0:
                StartCoroutine("SpeedBuff");
                break;

                case 1:
                StartCoroutine("RegenBuff");
                break;

                case 2:
                StartCoroutine("MultiplyPoints");
                break;
                
                case 3:
                StartCoroutine("StrengthEffect");
                break;

                case 4:
                StartCoroutine("ReduceAttackSpeed");
                break;

                case 5:
                StartCoroutine("Blind");
                break;

                case 6:
                StartCoroutine("Weak");
                break;

                case 7:
                StartCoroutine("x0Point");
                break;

                case 8:
                StartCoroutine("AttackSlower");
                break;

                case 9:
                StartCoroutine("InverseController");
                break;

                case 10:
                StartCoroutine("X2SpawnRate");
                break;

                case 11:
                StartCoroutine("Bait");
                break;

                case 12:
                StartCoroutine("RickRolled");
                break;
            }
        }
    }
    //SpeedBuff
    IEnumerator SpeedBuff()
    {
        BuffText.SetText("SpeedBuff");
        float IncreasedSpeed = 10f;
        playerScript.MoveSpeed += IncreasedSpeed;
        yield return new WaitForSeconds(CountDownTime);
        playerScript.MoveSpeed -= IncreasedSpeed;
        ChangeBuff = true;
    }
    //regen (special)
    IEnumerator RegenBuff()
    {
        SoundPlayer.PlayOneShot(BuffSound);
        BuffText.SetText("RegenBuff");
        float RegenAmount = 2.5f;
        float RegenSpeed = 0.69f;
        AllowRegen = true;
        while(!ChangeBuff)
        {
            if(playerScript.HP < playerScript.MaxHP && AllowRegen)
            {
                AllowRegen = false;
                Invoke("ResetRegen", RegenSpeed);
                playerScript.HP += RegenAmount;
            }
            if(time < CountDownTime)
            {
                time += Time.deltaTime;
            }
            if(time >= CountDownTime)
            {
                //end the buff
                ChangeBuff = true;
                AllowRegen = false;
                time = 0f;
                yield return 0;
            }
            yield return null;
        }
    }
    void ResetRegen()
    {
        AllowRegen = true;
    }
    //2x points
    IEnumerator MultiplyPoints()
    {
        SoundPlayer.PlayOneShot(BuffSound);
        BuffText.SetText("X2 Points");
        int MultipliedAmount = 2;
        PointsCounter PointCounterManipulate = GameObject.FindGameObjectWithTag("PointsCounter").GetComponent<PointsCounter>();
        PointCounterManipulate.MultipledPoints += MultipliedAmount;
        yield return new WaitForSeconds(CountDownTime);
        PointCounterManipulate.MultipledPoints -= MultipliedAmount;
        ChangeBuff = true;
    }
    //strength
    IEnumerator StrengthEffect()
    {
        SoundPlayer.PlayOneShot(BuffSound);
        BuffText.SetText("StrengthEffect");
        float AddStrengthAmount = 50f;
        Weapon WeaponScript = GameObject.FindGameObjectWithTag("Gun").GetComponent<Weapon>();
        WeaponScript.TempDamageBuff += AddStrengthAmount;
        yield return new WaitForSeconds(CountDownTime);
        WeaponScript.TempDamageBuff -= AddStrengthAmount;
        ChangeBuff = true;
    }
    //reduce attack speed
    IEnumerator ReduceAttackSpeed()
    {
        SoundPlayer.PlayOneShot(DebuffSound);
        BuffText.SetText("ReduceAttackSpeed");
        float ReduceAmount = 2f; // player attack 2 times faster
        Weapon WeaponScript = GameObject.FindGameObjectWithTag("Gun").GetComponent<Weapon>();
        WeaponScript.HaveTheAttackSpeedReduce = true;
        WeaponScript.TempAttackSpeedReduce = ReduceAmount;
        yield return new WaitForSeconds(CountDownTime);
        WeaponScript.HaveTheAttackSpeedReduce = false;
        ChangeBuff = true;
    }
    //blind
    IEnumerator Blind()
    {
        SoundPlayer.PlayOneShot(DebuffSound);
        BuffText.SetText("Blind");
        EffectController effectController = GameObject.FindGameObjectWithTag("CameraEffect").GetComponent<EffectController>();
        effectController.IsBlind = true;
        yield return new WaitForSeconds(CountDownTime);
        effectController.IsBlind = false;
        ChangeBuff = true;
    }
    //weak 
    IEnumerator Weak()
    {
        SoundPlayer.PlayOneShot(DebuffSound);
        BuffText.SetText("Weak");
        float TookStrengthAmount = 50f;
        Weapon WeaponScript = GameObject.FindGameObjectWithTag("Gun").GetComponent<Weapon>();
        WeaponScript.TempDamageBuff -= TookStrengthAmount;
        yield return new WaitForSeconds(CountDownTime);
        WeaponScript.TempDamageBuff += TookStrengthAmount;
        ChangeBuff = true;
    }
    //x0 point
    IEnumerator x0Point()
    {
        SoundPlayer.PlayOneShot(DebuffSound);
        BuffText.SetText("x0Point");
        PointsCounter PointCounterManipulate = GameObject.FindGameObjectWithTag("PointsCounter").GetComponent<PointsCounter>();
        PointCounterManipulate.AllowAddScore = false;
        yield return new WaitForSeconds(CountDownTime);
        PointCounterManipulate.AllowAddScore = true;
        ChangeBuff = true;
    }
    //attack slower
    IEnumerator AttackSlower()
    {
        SoundPlayer.PlayOneShot(DebuffSound);
        BuffText.SetText("AttackSlower");
        float SlowerAmount = 0.5f; // player attack 2 times slower
        Weapon WeaponScript = GameObject.FindGameObjectWithTag("Gun").GetComponent<Weapon>();
        WeaponScript.HaveTheAttackSpeedReduce = true;
        WeaponScript.TempAttackSpeedReduce = SlowerAmount;
        yield return new WaitForSeconds(CountDownTime);
        WeaponScript.HaveTheAttackSpeedReduce = false;
        ChangeBuff = true;
    }
    //inverse controller
    IEnumerator InverseController()
    {
        SoundPlayer.PlayOneShot(DebuffSound);
        BuffText.SetText("InverseController");
        playerScript.InverseControllerVar = -1f;
        yield return new WaitForSeconds(CountDownTime);
        playerScript.InverseControllerVar = 1f;
        ChangeBuff = true;
    }
    //x2 spawnrate
    IEnumerator X2SpawnRate()
    {
        SoundPlayer.PlayOneShot(DebuffSound);
        BuffText.SetText("X2SpawnRate");
        float TotalIncrease = 2f; //I'm so bad at naming variables
        Spawner spawnerScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
        spawnerScript.IncreaseAmount = TotalIncrease;
        spawnerScript.isIncreaseSpawnRate = true;
        yield return new WaitForSeconds(CountDownTime);
        spawnerScript.isIncreaseSpawnRate = false;
        ChangeBuff = true;
    }
    //bait
    IEnumerator Bait()
    {
        SoundPlayer.PlayOneShot(DebuffSound);
        BuffText.SetText("Bait");
        playerScript.IsBait = true;
        yield return new WaitForSeconds(CountDownTime);
        playerScript.IsBait = false;
        ChangeBuff = true;
    }
    //rickroll
    IEnumerator RickRolled()
    {
        SoundPlayer.PlayOneShot(DebuffSound);
        BuffText.SetText("GetRickRolled");
        SoundPlayer.PlayOneShot(RickRoll);
        yield return new WaitForSeconds(CountDownTime);
        SoundPlayer.Stop();
        ChangeBuff = true;
    }
}