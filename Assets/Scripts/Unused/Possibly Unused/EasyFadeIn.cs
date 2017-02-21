using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class EasyFadeIn : MonoBehaviour {
	
	/*
	About
		Easy Fade In
		By Desi Quintans (CowfaceGames.com), 18 August 2012.
 
	License
		Free as in speech, and free as in beer.
 
	Usage
		Attach this script to a GameObject with an AudioSource and enter a fade time. Easy Fade In will
		smoothly increase the audiosource's volume over this period of time until it reaches maximum
		volume, and then will destroy itself to prevent wasting a FixedUpdate() check.
	*/
	
	public int approxSecondsToFade = 10;
	
	void FixedUpdate ()
	{
		if (GetComponent<AudioSource>().volume < .5)
		{
			GetComponent<AudioSource>().volume = GetComponent<AudioSource>().volume + (Time.deltaTime / (approxSecondsToFade + 1));
		}
		else
		{
			Destroy (this);
		}
	}
}