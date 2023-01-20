using UnityEngine;
using TMPro;

public class CurrentWeaponText : MonoBehaviour
{
    public TextMeshProUGUI CurrentText;
    void Update()
    {
        CurrentText.SetText(GameObject.FindGameObjectWithTag("Gun").name);
        Debug.Log(GameObject.FindGameObjectWithTag("Gun").name);
    }
}
