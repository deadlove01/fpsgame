using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FPSAnimController : NetworkBehaviour {

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
    private const string STANDING_DEATH = "StandingDeath";
    private const string CROUCHING_DEATH = "CrouchingDeath";

    public RuntimeAnimatorController animControllerPistol, animControllerGun;

    private NetworkAnimator networkAnim;
    private Transform chest;
    void Awake()
    {
        anim = GetComponent<Animator>();
        networkAnim = GetComponent<NetworkAnimator>();
        chest = anim.GetBoneTransform(HumanBodyBones.Chest);
    }

    void Start()
    {
       
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
            networkAnim.SetTrigger(PISTOL_STANDSHOOT);
        }
        else
        {
            anim.SetTrigger(PISTOL_CROUCHSHOOT);
            networkAnim.SetTrigger(PISTOL_CROUCHSHOOT);
        }
    }

   

    public void Reload()
    {
        anim.SetTrigger(PISTOL_RELOAD);
        networkAnim.SetTrigger(PISTOL_RELOAD);
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

    public void Death(bool isCrouching)
    {
        if (isCrouching)
        {
            anim.SetBool(CROUCHING_DEATH, true);
        }
        else
        {
            anim.SetBool(STANDING_DEATH, true);
        }
    }

    public void Respawn()
    {
        anim.SetBool(CROUCHING_DEATH, false);
        anim.SetBool(STANDING_DEATH, false);
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
