using UnityEngine;
using System.Collections;

namespace SlimUI.ModernMenu
{
	public class CheckMusicVolume : MonoBehaviour 
    {
		void  Start ()
        {
			GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
		}

		public void UpdateVolume ()
        {
			GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
		}
	}
}