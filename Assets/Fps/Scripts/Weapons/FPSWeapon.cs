using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSWeapon : FPSWeaponBase
{

    // Use this for initialization
    private GameObject muzzleFlash;

    private float nextFireTime = 0f;

    void Awake()
    {
        muzzleFlash = transform.Find("MuzzleFlash").gameObject;
        muzzleFlash.SetActive(false);
        
    }

    void Start()
    {
        Init();
    }
	IEnumerator TurnOnMuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.SetActive(false);
    }


    public void Shoot()
    {
        // active muzzle flash
        StartCoroutine(TurnOnMuzzleFlash());
    }


    #region weapon props methods
    public override void Init()
    {
        ReadyToFire = true;
    }

    public override bool Fire()
    {
        if(Time.time > nextFireTime)
        {
            ReadyToFire = true;
        }
        if(ReadyToFire)
        {           
            nextFireTime = Time.time + 1f / FireRate;
            ReadyToFire = false;
            Shoot();
            return true;
        }

        return false;
    }

    #endregion

}
