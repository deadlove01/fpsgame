using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	[SerializeField] private AudioClip reloadClipOutSound;
	[SerializeField] private AudioClip reloadClipInSound;
	[SerializeField] private AudioClip reloadBoltPullSound;
    [SerializeField] private AudioClip silenceOffSound;
    [SerializeField] private AudioClip silenceOnSound;
    [SerializeField] private AudioSource audio;


	public  void ReloadClipOut()
	{
		audio.PlayOneShot (reloadClipOutSound);
	}

	public  void ReloadClipIn()
	{
		audio.PlayOneShot (reloadClipInSound);
	}


	public  void ReloadBoltPull()
	{
		audio.PlayOneShot (reloadBoltPullSound);
	}


    public void SilenceOff()
    {

        audio.PlayOneShot(silenceOffSound);
    }


    public void SilenceOn()
    {
        audio.PlayOneShot(silenceOnSound);
    }
}
