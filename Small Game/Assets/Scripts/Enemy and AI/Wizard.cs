using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Wizard : MonoBehaviour
{
    public Transform[] SummonPoints;
    public GameObject[] Enemies;
    [SerializeField] float TimeBetweenSummon = 10f;
    bool AllowSummon = true;
    public EnemyData EnemyDat;
    Transform PlayerTarget;
    int SummonIndex;
    // Start is called before the first frame update
    void Start()
    {
        //get all summon points position
        SummonPoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            SummonPoints[i] = transform.GetChild(i);
        }
        //find player
        PlayerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //if player in attack range then summon enemy
        if(EnemyDat.AttackRange >= Vector2.Distance(transform.position, PlayerTarget.position))
        {
            if(AllowSummon)
            {
                AllowSummon = false;
                
                for(int i = 0; i < SummonPoints.Length; i++)
                {
                    RandomSummonIndex();
                    Instantiate(Enemies[SummonIndex], SummonPoints[i]);
                }
                Invoke("ResetSummon", TimeBetweenSummon);
            }
        }
    }
    //to know what enemy will be summoned
    void RandomSummonIndex()
    {
        SummonIndex = Random.Range(0, Enemies.Length);
    }
    void ResetSummon()
    {
        AllowSummon = true;
    }
}
