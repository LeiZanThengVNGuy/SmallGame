using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    [SerializeField] float StartTime = 10f;
    float CountDown;
    [SerializeField] Transform[] Weapons;
    int SelectedWeapon;
    [HideInInspector]public bool SwitchingWeapon;
    [HideInInspector]public string CurrentWeaponName;
    private void Start() {
        SelectedWeapon = Random.Range(0, Weapons.Length);
        CountDown = StartTime;
        SetWeapons();
        Select(SelectedWeapon);
        SwitchingWeapon = true;
    }
    private void Update() {
        if(CountDown > 0f)
        {
            CountDown -= Time.deltaTime;
            SwitchingWeapon = false; 
        }
        if(CountDown <= 0f)
        {
            CountDown = StartTime;
            SelectedWeapon = Random.Range(0, Weapons.Length);
            Select(SelectedWeapon);
            SwitchingWeapon = true;
        }
    }
    void SetWeapons()
    {
        Weapons = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            Weapons[i] = transform.GetChild(i);
        }
    }
    void Select(int WeaponIndex)
    {
        for (int i = 0; i < Weapons.Length; i++)
        {
            Weapons[i].gameObject.SetActive(i == WeaponIndex);
            CurrentWeaponName = Weapons[i].gameObject.name;
        }
    }
}
