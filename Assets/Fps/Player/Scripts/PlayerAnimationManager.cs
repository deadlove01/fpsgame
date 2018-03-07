using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class PlayerAnimationManager : MonoBehaviour {

	[SerializeField] private Animator animator;
	[SerializeField] private float fireRate = 1f;
	[SerializeField] private float animRate = 0.01f;
	[SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip unsilShootSound;

    private bool isIdle = false;
	private bool isReload = false;
	private bool isShooting = false;
    private bool isSilencerAttached = false;
    private bool isAttaching = false;

	private float fireTimer = 0;
	private string[] shootAnims = new []{"Fire", "Fire2", "Fire3"};
	private AudioSource audio;

	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> ();
		audio.clip = shootSound;
	}
	
	// Update is called once per frame
	void Update () {
		//ChangeToIdle ();

	    if (Input.GetMouseButtonDown(1))
	    {
	        isAttaching = true;
	        AttachSilencer();
	    }
        
        if (Input.GetKeyDown (KeyCode.R)) {
			Reload ();
		}
		if (Input.GetButton ("Fire1")) {
			isShooting = true;
			Fire ();
		} else {
			isShooting = false;
		}
        

		if (fireTimer < fireRate)
			fireTimer += Time.deltaTime;
	}

	void FixedUpdate()
	{
		var animState = animator.GetCurrentAnimatorStateInfo (0);
	
		if (animState.IsName ("Fire"))
			animator.SetBool ("Fire", false);
		else if (animState.IsName ("Fire3"))
			animator.SetBool ("Fire3", false);
		else if (animState.IsName ("Fire2"))
			animator.SetBool ("Fire2", false);
		if (isReload && !animState.IsName ("Reload") && !animator.IsInTransition(0)) {
			isReload = false;
			animator.SetBool ("Reload", false);
		}
	}


    private void AttachSilencer()
    {
        if (isSilencerAttached)
        {
            animator.SetBool("AttachSilencer", false);
        }
        else
        {
            animator.SetBool("AttachSilencer", true);
        }

        isSilencerAttached = !isSilencerAttached;
    }

	private void Reload()
	{
		isReload = true;
	    var reloadAnim = isSilencerAttached ? "Reload" : "Reload";
        animator.SetTrigger(reloadAnim);
//        animator.CrossFade (reloadAnim, 0.15f);
//        animator.SetBool ("Reload", true);
	}

	private void Fire()
	{
		if (fireTimer < fireRate || isReload)
			return;
	    var fireAnim = isSilencerAttached ? "UnsilFire" : "Fire";
        var anim = shootAnims[Random.Range (0, shootAnims.Length)];
		animator.CrossFadeInFixedTime (fireAnim, animRate);
//        animator.SetTrigger("Fire");
		fireTimer = 0;
	    if (isSilencerAttached)
	    {
	        audio.clip = unsilShootSound;
        }
	    else
	    {
	        audio.clip = shootSound;
	    }
	    audio.Play();
    }



	private void ResetAllAnims()
	{

	}
}
