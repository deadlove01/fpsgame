using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class M4A1_Silence : WeaponBase
{
    [SerializeField] private bool hasSilencer = true;
    [SerializeField] private string FireSilenceAnimName = "Fire";
    [SerializeField] private string fireUnsilenceAnimName = "UnsilFire";



    private bool isSilencerAttached = false;
    private bool isReload = false;
    private SoundManager _soundManager;
    void Start()
    {
        base.Start();
        print("this is value of fire rate: " + fireRate);

        _soundManager = GetComponent<SoundManager>();
    }


    void Update()
    {

        if (Input.GetButton("Fire1"))
        {
            print("fire!");
            Fire();
        }
        else if(Input.GetMouseButtonDown(1) && !isReload)
        {
            
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            isReload = true;
            var reloadAnim = isSilencerAttached ? "Reload" : "Reload";
            animator.SetTrigger(reloadAnim);
        }

        if (fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        var animState = animator.GetCurrentAnimatorStateInfo(0);

        if (animState.IsName("Fire"))
            animator.SetBool("Fire", false);
        else if (animState.IsName("Fire3"))
            animator.SetBool("Fire3", false);
        else if (animState.IsName("Fire2"))
            animator.SetBool("Fire2", false);
        if (isReload && !animState.IsName("Reload") && !animator.IsInTransition(0))
        {
            isReload = false;
            animator.SetBool("Reload", false);
        }
    }

    public override void Fire()
    {
        if (fireTimer < fireRate)
            return;
        var fireAnim = isSilencerAttached ? "UnsilFire" : "Fire";
//        var anim = fireAnims[Random.Range(0, fireAnims.Length)];
        animator.CrossFadeInFixedTime(fireAnim, animRate);
        fireTimer = 0;

        PlaySound();
    }

    public override void Reload()
    {
       // throw new System.NotImplementedException();
    }

    public override void PlayAnimation()
    {
        //throw new System.NotImplementedException();
    }

    public override void PlaySound()
    {
        base.audio.PlayOneShot(weaponSounds[0]);
       // throw new System.NotImplementedException();
    }
}
