using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSAnimController : MonoBehaviour {

    private Animator anim;

    private const string MOVE = "Move";
    private const string VELOCITY_Y = "VelocityY";
    private const string CROUCH_WALK = "CrouchWalk";
    private const string CROUCH = "Crouch";


    private const string PISTOL_STANDSHOOT = "StandShoot";
    private const string PISTOL_CROUCHSHOOT = "CrouchShoot";
    private const string PISTOL_RELOAD = "Reload";


    private const string MG_STANDSHOOT = "MgStandShoot";
    private const string MG_CROUCHSHOOT = "MgCrouchShoot";
    private const string MG_RELOAD = "MgReload";


    public RuntimeAnimatorController animControllerPistol, animControllerGun;

    private Transform chest;
    void Awake()
    {
        anim = GetComponent<Animator>();

    }

    void Start()
    {
        chest = anim.GetBoneTransform(HumanBodyBones.Chest);
    }


    public void UpdateLook(Vector3 targetPos, Vector3 offset)
    {
        chest.LookAt(targetPos);
        chest.rotation = chest.rotation * Quaternion.Euler(offset);
    }
    public void Movement(float magnitude)
    {
        anim.SetFloat(MOVE, magnitude);
    }

    public void PlayerJump(float velocity)
    {
        anim.SetFloat(VELOCITY_Y, velocity);
    }

    public void PlayerCrouch(bool isCrouching)
    {
        anim.SetBool(CROUCH, isCrouching);
    }

    public void PlayerCrouchWalk(float magnitude)
    {
        anim.SetFloat(CROUCH_WALK, magnitude);
    }


    public void Shoot(bool isStanding)
    {
        if(isStanding)
        {
            anim.SetTrigger(PISTOL_STANDSHOOT);
        }else
        {
            anim.SetTrigger(PISTOL_CROUCHSHOOT);
        }
    }

   

    public void Reload()
    {
        anim.SetTrigger(PISTOL_RELOAD);
    }

    public void ChangeAnimationController(bool isPistol)
    {
        if(isPistol)
        {
            anim.runtimeAnimatorController = animControllerPistol;
        }else
        {
            anim.runtimeAnimatorController = animControllerGun;
        }
    }

    //public void MachineGunShoot(bool isStanding)
    //{
    //    if (isStanding)
    //    {
    //        anim.SetTrigger(MG_STANDSHOOT);
    //    }
    //    else
    //    {
    //        anim.SetTrigger(MG_CROUCHSHOOT);
    //    }
    //}

    //public void MachineGunReload(bool isStanding)
    //{
    //    anim.SetTrigger(MG_RELOAD);
    //}
}
