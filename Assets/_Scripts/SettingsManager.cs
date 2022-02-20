using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Image soundToggle, musicToggle, vibrationToggle, leftHandToggle;

    public Sprite toggleOn, toggleOff;

    public GameObject scorePanel;

    public VibrationManager vibrator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SettingsCheck()
    {
        if (PlayerPrefsSafe.GetInt("Sound") == 0)
            soundToggle.sprite = toggleOff;
        else
            soundToggle.sprite = toggleOn;

        if (PlayerPrefsSafe.GetInt("Music") == 0)
            musicToggle.sprite = toggleOff;
        else
            musicToggle.sprite = toggleOn;

        if (PlayerPrefsSafe.GetInt("Vibration") == 0)
            vibrationToggle.sprite = toggleOff;
        else
            vibrationToggle.sprite = toggleOn;

        if (PlayerPrefsSafe.GetInt("LeftHand") == 0)
        {
            leftHandToggle.sprite = toggleOff;

            scorePanel.GetComponent<RectTransform>().localPosition = new Vector2(-324, scorePanel.GetComponent<RectTransform>().localPosition.y);
        }
        else
        {
            leftHandToggle.sprite = toggleOn;

            scorePanel.GetComponent<RectTransform>().localPosition = new Vector2(324, scorePanel.GetComponent<RectTransform>().localPosition.y);
        }
    }

    public void ToggleSound()
    {
        if (PlayerPrefsSafe.GetInt("Sound") == 0)
        { 
            soundToggle.sprite = toggleOn;
            PlayerPrefsSafe.SetInt("Sound", 1);

            //audioManager.PlaySound("PressButton");
        }
        else
        {
            soundToggle.sprite = toggleOff;
            PlayerPrefsSafe.SetInt("Sound", 0);
        }

        vibrator.Vibrate(VibrationManager.VibraType.Light);
    }

    public void ToggleMusic()
    {
        if (PlayerPrefsSafe.GetInt("Music") == 0)
        {
            musicToggle.sprite = toggleOn;
            PlayerPrefsSafe.SetInt("Music", 1);

            //audioManager.PlaySound("PressButton");
        }
        else
        {
            musicToggle.sprite = toggleOff;
            PlayerPrefsSafe.SetInt("Music", 0);
        }

        vibrator.Vibrate(VibrationManager.VibraType.Light);
    }

    public void ToggleVibration()
    {
        if (PlayerPrefsSafe.GetInt("Vibration") == 0)
        {
            vibrationToggle.sprite = toggleOn;
            PlayerPrefsSafe.SetInt("Vibration", 1);

            vibrator.Vibrate(VibrationManager.VibraType.Light);

            //audioManager.PlaySound("PressButton");
        }
        else
        {
            vibrationToggle.sprite = toggleOff;
            PlayerPrefsSafe.SetInt("Vibration", 0);
        }
    }

    public void ToggleLeftHand()
    {
        if (PlayerPrefsSafe.GetInt("LeftHand") == 0)
        {
            leftHandToggle.sprite = toggleOn;
            PlayerPrefsSafe.SetInt("LeftHand", 1);

            scorePanel.GetComponent<RectTransform>().localPosition = new Vector2(324, scorePanel.GetComponent<RectTransform>().localPosition.y);

            //audioManager.PlaySound("PressButton");
        }
        else
        {
            leftHandToggle.sprite = toggleOff;
            PlayerPrefsSafe.SetInt("LeftHand", 0);

            scorePanel.GetComponent<RectTransform>().localPosition = new Vector2(-324, scorePanel.GetComponent<RectTransform>().localPosition.y);
        }

        vibrator.Vibrate(VibrationManager.VibraType.Light);
    }


}
