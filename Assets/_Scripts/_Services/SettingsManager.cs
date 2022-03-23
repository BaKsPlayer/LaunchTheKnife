using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Image soundToggle;
    [SerializeField] private Image musicToggle;
    [SerializeField] private Image vibrationToggle;
    [SerializeField] private Image leftHandToggle;

    [SerializeField] private Sprite toggleOn;
    [SerializeField] private Sprite toggleOff;

    [SerializeField] private GameObject scorePanel;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OpenSettings()
    {
        gameObject.SetActive(true);
        _animator.SetTrigger("Open");
    }

    public void CloseSettings()
    {
        _animator.SetTrigger("Close");
        StartCoroutine(DeactivateSettings(0.5f));
    }

    private IEnumerator DeactivateSettings(float delay)
    {
        yield return new WaitForSeconds(delay);

        gameObject.SetActive(false);
    }

    public void Initialize()
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

    public void FirstLaunchInitialize()
    {
        PlayerPrefsSafe.SetInt("Sound", 0);
        PlayerPrefsSafe.SetInt("Music", 0);
        PlayerPrefsSafe.SetInt("Vibration", 1);
        PlayerPrefsSafe.SetInt("LeftHand", 0);
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

        VibrationManager.Instance.Vibrate(VibrationType.Light);
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

        VibrationManager.Instance.Vibrate(VibrationType.Light);
    }

    public void ToggleVibration()
    {
        if (PlayerPrefsSafe.GetInt("Vibration") == 0)
        {
            vibrationToggle.sprite = toggleOn;
            PlayerPrefsSafe.SetInt("Vibration", 1);

            VibrationManager.Instance.Vibrate(VibrationType.Light);

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

        VibrationManager.Instance.Vibrate(VibrationType.Light);
    }


}
