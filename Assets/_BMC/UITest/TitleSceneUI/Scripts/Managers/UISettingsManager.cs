using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SlimUI.ModernMenu
{
	public class UISettingsManager : MonoBehaviour 
    {
		public enum Platform {Desktop};
		public Platform platform;
		// toggle buttons

		[Header("VIDEO SETTINGS")]
		public GameObject fullscreentext;
		public GameObject vsynctext;
		public GameObject cameraeffectstext; 

		[Header("GAME SETTINGS")]
		public GameObject tooltipstext;

		[Header("CONTROLS SETTINGS")]
		public GameObject invertmousetext;

		// sliders
		public GameObject musicSlider;

		private float sliderValue = 0.0f;
		private float sliderValueXSensitivity = 0.0f;
		private float sliderValueYSensitivity = 0.0f;
		private float sliderValueSmoothing = 0.0f;
		

		void  Start ()
		{
			// check slider values
			musicSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume");

			// check full screen
			if(Screen.fullScreen == true){
				fullscreentext.GetComponent<TextMeshProUGUI>().text = "on";
			}
			else if(Screen.fullScreen == false){
				fullscreentext.GetComponent<TextMeshProUGUI>().text = "off";
			}

			// check tool tip value
			if(PlayerPrefs.GetInt("ToolTips")==0)
			{
				tooltipstext.GetComponent<TextMeshProUGUI>().text = "off";
			}
			else{
				tooltipstext.GetComponent<TextMeshProUGUI>().text = "on";
			}


			// check vsync
			if(QualitySettings.vSyncCount == 0)
			{
				vsynctext.GetComponent<TextMeshProUGUI>().text = "off";
			}
			else if(QualitySettings.vSyncCount == 1)
			{
				vsynctext.GetComponent<TextMeshProUGUI>().text = "on";
			}
		}

		public void Update ()
        {
			//sliderValue = musicSlider.GetComponent<Slider>().value;
		}

		public void FullScreen ()
		{
			Screen.fullScreen = !Screen.fullScreen;

			if(Screen.fullScreen == true)
			{
				fullscreentext.GetComponent<TextMeshProUGUI>().text = "on";
			}
			else if(Screen.fullScreen == false)
			{
				fullscreentext.GetComponent<TextMeshProUGUI>().text = "off";
			}
		}

		public void MusicSlider ()
		{
			//PlayerPrefs.SetFloat("MusicVolume", sliderValue);
			PlayerPrefs.SetFloat("MusicVolume", musicSlider.GetComponent<Slider>().value);
		}

		public void SensitivitySmoothing ()
		{
			PlayerPrefs.SetFloat("MouseSmoothing", sliderValueSmoothing);
			Debug.Log(PlayerPrefs.GetFloat("MouseSmoothing"));
		}

		// show tool tips like: 'How to Play' control pop ups
		public void ToolTips ()
		{
			if(PlayerPrefs.GetInt("ToolTips")==0)
			{
				PlayerPrefs.SetInt("ToolTips",1);
				tooltipstext.GetComponent<TextMeshProUGUI>().text = "on";
			}
			else if(PlayerPrefs.GetInt("ToolTips")==1)
			{
				PlayerPrefs.SetInt("ToolTips",0);
				tooltipstext.GetComponent<TextMeshProUGUI>().text = "off";
			}
		}

        //public void ShadowsHigh ()
        //{
        //	PlayerPrefs.SetInt("Shadows",2);
        //	QualitySettings.shadowCascades = 4; // Off: 0, Low: 2, Medium: 4, High: 4
        //	QualitySettings.shadowDistance = 500; // Off: 0, Low: 75, Medium: 150, High: 500
        //	shadowofftextLINE.gameObject.SetActive(false);
        //	shadowlowtextLINE.gameObject.SetActive(false);
        //	shadowhightextLINE.gameObject.SetActive(true);
        //}

        public void vsync ()
		{
			if(QualitySettings.vSyncCount == 0)
			{
				QualitySettings.vSyncCount = 1;
				vsynctext.GetComponent<TextMeshProUGUI>().text = "on";
			}
			else if(QualitySettings.vSyncCount == 1)
			{
				QualitySettings.vSyncCount = 0;
				vsynctext.GetComponent<TextMeshProUGUI>().text = "off";
			}
		}

		public void InvertMouse ()
        {
			if(PlayerPrefs.GetInt("Inverted")==0)
			{
				PlayerPrefs.SetInt("Inverted",1);
				invertmousetext.GetComponent<TextMeshProUGUI>().text = "on";
			}
			else if(PlayerPrefs.GetInt("Inverted")==1)
			{
				PlayerPrefs.SetInt("Inverted",0);
				invertmousetext.GetComponent<TextMeshProUGUI>().text = "off";
			}
		}

		public void CameraEffects ()
		{
			if(PlayerPrefs.GetInt("CameraEffects")==0)
			{
				PlayerPrefs.SetInt("CameraEffects",1);
				cameraeffectstext.GetComponent<TextMeshProUGUI>().text = "on";
			}
			else if(PlayerPrefs.GetInt("CameraEffects")==1)
            {
				PlayerPrefs.SetInt("CameraEffects",0);
				cameraeffectstext.GetComponent<TextMeshProUGUI>().text = "off";
			}
		}
	}
}