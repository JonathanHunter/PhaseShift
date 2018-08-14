using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmManager : MonoBehaviour {

	private bool alarmTripped = false;
	public AudioSource AlarmSound, LevelMusic;
	public SpriteRenderer AlarmFlash;
	private float timeForColorBlink = 0;

	void OnTriggerEnter(Collider other)
    {
		if(!alarmTripped)
		{
			LevelMusic.volume = 0.6f;
			AlarmSound.volume = 0.55f;
		}
        alarmTripped = true;
    }
	
	void Update()
	{
		if(alarmTripped)
		{
			float blinkingAlpha = (0.4f * Mathf.Cos(timeForColorBlink)) ;
			AlarmFlash.color = new Color(AlarmFlash.color.r, AlarmFlash.color.g, AlarmFlash.color.b, blinkingAlpha);
			timeForColorBlink+=0.075f;
		}
	}
}
