using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    GameObject CurrentEnemy;
    [HideInInspector] public float Damage;
    public float DestroyDelay = 0.005f;
    public float LifeTime = 5f;
    private void Start() {
        Destroy(gameObject, LifeTime);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(!(other.tag == "Bullet") && !(other.tag == "Enemy"))
        {
            if(other.tag == "Player")
            {
                other.gameObject.GetComponent<PlayerMovement>().GetHurt(Damage);
            }
            if(!(other.tag == "Bullet") && !(other.tag == "Enemy"))
            {
                Destroy(gameObject, DestroyDelay);
            }
        }
    }
}
