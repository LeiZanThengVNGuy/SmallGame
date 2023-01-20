using System.Collections;
using UnityEngine;

public class PatrolPointData : MonoBehaviour
{
    public Transform[] PatrolPoints;
    private void Awake() {
        PatrolPoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            PatrolPoints[i] = transform.GetChild(i);
        }
    }
}
