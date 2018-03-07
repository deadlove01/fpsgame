using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FPSWeaponBase : MonoBehaviour {

    public enum WeaponType
    {
        Pistol,
        Rifle
    }
    public WeaponType CurrentWeaponType;
    public int Ammo { get; set; }
    public int Rounds { get; set; }
    public int MaxAmmoPerRound { get; set; }
    public float FireRate;
    public float Recoil { get; set; }
    
    public bool ReadyToFire { get; set; }
    public float Damage { get; set; }

    public virtual void Init() { }

    public virtual void CanFire() { }
    public virtual bool Fire() { return false; }
    

}
