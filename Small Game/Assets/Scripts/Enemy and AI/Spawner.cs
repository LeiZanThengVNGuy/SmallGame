using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float TimeBetweenSpawn = 2f;
    [SerializeField] float[] percentages;
    [SerializeField] GameObject[] objectsToSpawn;
    [SerializeField] Transform spawnPoint;
    [SerializeField] int MaxEnemyInScene = 100;
    float currentCountDown = 0f;
    bool allowSpawn = true;

    [HideInInspector]public bool isIncreaseSpawnRate = false;
    [HideInInspector]public float IncreaseAmount;
    void Update()
    {
        Debug.Log(GameObject.FindGameObjectsWithTag("Enemy").Length);
        if(currentCountDown > 0f && !isIncreaseSpawnRate)
        {
            currentCountDown -= Time.deltaTime;
        }
        //increase the spawnrate
        if(currentCountDown > 0f && isIncreaseSpawnRate)
        {
            currentCountDown -= Time.deltaTime * IncreaseAmount;
        }
        if(currentCountDown <= 0f && allowSpawn)
        {
            Instantiate(objectsToSpawn[getRandomSpawn()], spawnPoint);
            currentCountDown = TimeBetweenSpawn;
        }
        //prevent tremendous amount of enemy
        if(GameObject.FindGameObjectsWithTag("Enemy").Length > MaxEnemyInScene)
        {
            allowSpawn = false;
        }
        if(GameObject.FindGameObjectsWithTag("Enemy").Length < MaxEnemyInScene)
        {
            allowSpawn = true;
        }
    }
    int getRandomSpawn()
    {
        float random = Random.Range(0f, 1f);
        float numForAdding = 0f;
        float Total = 0f;
        for(int i = 0; i < percentages.Length; i++)
        {
            Total += percentages[i];
        }
        for(int i = 0; i < objectsToSpawn.Length; i++)
        {
            if(percentages[i] / Total + numForAdding >= random)
            {
                return i;
            }
            else
            {
                numForAdding += percentages[i] / Total;
            }
        }
        return 0;
    }
}
