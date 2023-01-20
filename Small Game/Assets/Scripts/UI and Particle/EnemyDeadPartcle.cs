using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadPartcle : MonoBehaviour
{
    [SerializeField]float LifeTime = 0.2f;
    private void Start() {
        Destroy(gameObject, LifeTime);
    }
}
