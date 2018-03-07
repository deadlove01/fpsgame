using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{

    [SerializeField] protected float fireRate = 1f;
    [SerializeField] protected float animRate = 0.01f;
    [SerializeField] protected AudioClip[] weaponSounds;
    [SerializeField] protected GameObject player;
    [SerializeField] protected string[] fireAnims;

    protected float fireTimer = 0;
    protected AudioSource audio;
    protected Animator animator;
    protected void Start()
    {
        audio = player.GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        print("call weapon base start method!");
    }

    public abstract void Fire();
    public abstract void Reload();
    public abstract void PlayAnimation();
    public abstract void PlaySound();



}
