using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSHandWeapon : FPSWeaponBase
{
    public AudioClip shootClip;
    public AudioClip shootSilencerClip;
    public AudioClip[] reloadClip;

    private Animator anim;

    [SerializeField]
    private GameObject muzzleFlash;

    private AudioSource audioManager;

    private const string RELOAD = "Reload";
    private const string SHOOT = "Shoot";
    private const string SHOOT_SILENCER = "ShootSilencer";
    private const string SILENCER = "AddSilencer";
    private bool addSilencer = false;
    void Awake()
    {
        anim = GetComponent<Animator>();
        //muzzleFlash = transform.Find("MuzzleFlash").gameObject;
        muzzleFlash.SetActive(false);
        audioManager = GetComponent<AudioSource>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void Reload()
    {
        //if (reloadClip != null)
        //{
        //    audioManager.clip = reloadClip;
        //}
        //audioManager.Play();
      
        anim.SetTrigger(RELOAD);
    }


    public void Shoot()
    {
        //anim.SetTrigger(SHOOT);
        var shootSoundClip = addSilencer ? shootSilencerClip : shootClip;
        if(shootSoundClip != null)
        {
            audioManager.clip = shootSoundClip;
        }
        audioManager.Play();
        var shootAnim = addSilencer ? SHOOT_SILENCER : SHOOT;

        if(!addSilencer)
            StartCoroutine(TurnOnMuzzleFlash());
        anim.CrossFadeInFixedTime(shootAnim, 0.15f);
  
    }


    public void AddSilencer()
    {
        addSilencer = !addSilencer;
        anim.SetBool(SILENCER, addSilencer);
    }
    IEnumerator TurnOnMuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        // yield return new WaitForSeconds(0.01f);
        yield return null;
        muzzleFlash.SetActive(false);
    }

}
