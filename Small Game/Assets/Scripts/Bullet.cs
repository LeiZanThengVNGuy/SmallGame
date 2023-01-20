using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    GameObject CurrentGun;
    public float Damage;
    public float DestroyDelay = 0.005f;
    public float LifeTime = 1.2f;
    private void Start() {
        Destroy(gameObject, LifeTime);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(!(other.tag == "Bullet") || !(other.tag == "Player"))
        {
            if(other.tag == "Enemy")
            {
                other.gameObject.GetComponent<Enemy>().TakeDamage(Damage);
            }
            if(!(other.tag == "Bullet") && !(other.tag == "Player"))
            {
                Destroy(gameObject, DestroyDelay);
            }
        }
    }
}
