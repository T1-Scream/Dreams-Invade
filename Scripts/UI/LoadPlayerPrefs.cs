using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using QuantumTek.EncryptedSave;

public class LoadPlayerPrefs : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private bool canUse = false;
    [SerializeField] private MenuManager menuMamager;

    [Header("Graphics Settings")]
    [SerializeField] private TMP_Text brightnessText = null;
    [SerializeField] private Slider brightnessSlider = null;

    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    [Header("Volume Settings")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private TMP_Text volumeText = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private TMP_Text menuVolumeText = null;
    [SerializeField] private Slider menuVolumeSlider = null;
    [SerializeField] private TMP_Text effectText = null;
    [SerializeField] private Slider effectSlider = null;

    [Header("Controls Settings")]
    [SerializeField] private TMP_Text mouseSensText = null;
    [SerializeField] private Slider mouseSensSlider = null;
    [SerializeField] private Toggle invertYAxisToggle = null;

    [Header("Language Settings")]
    [SerializeField] private TMP_Dropdown languageDropdown;
    [SerializeField] private TMP_Dropdown tutorialDropdown;
    [SerializeField] private GameObject tutorialButton;

    private void Awake()
    {
        if (canUse) {
            if (ES_Save.Exists("masterLanguage")) {
                int localLanguage = ES_Save.Load<int>("masterLanguage");
                languageDropdown.value = localLanguage;
            }

            if (ES_Save.Exists("masterTutorial")) {
                int localTutorial = ES_Save.Load<int>("masterTutorial");
                tutorialDropdown.value = localTutorial;
            }

            if (ES_Save.Exists("masterQuality")) {
                int localQuality = ES_Save.Load<int>("masterQuality");
                qualityDropdown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }

            if (ES_Save.Exists("masterResolution")) {
                int localResolution = ES_Save.Load<int>("masterResolution");
                resolutionDropdown.value = localResolution;
            }

            if (ES_Save.Exists("masterFullscreen")) {
                int localFullscreen = ES_Save.Load<int>("masterFullscreen");
                if (localFullscreen == 1) {
                    Screen.fullScreen = true;
                    fullscreenToggle.isOn = true;
                }
                else {
                    Screen.fullScreen = false;
                    fullscreenToggle.isOn = false;
                }
            }

            if (ES_Save.Exists("masterBrightness")) {
                float localBrightness = ES_Save.Load<float>("masterBrightness");
                brightnessText.text = localBrightness.ToString("0.0");
                brightnessSlider.value = localBrightness;
            }

            if (ES_Save.Exists("masterVolume") && ES_Save.Exists("masterEffectVolume")) {
                float localVolume = ES_Save.Load<float>("masterVolume");
                volumeText.text = localVolume.ToString("0%");
                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;

                float localMenuVolume = ES_Save.Load<float>("masterMenuVolume");
                menuVolumeText.text = localMenuVolume.ToString("0%");
                menuVolumeSlider.value = localMenuVolume;
                //audioManager.SetVolume("MenuBGM", localMenuVolume);

                float localEffVolume = ES_Save.Load<float>("masterEffectVolume");
                effectText.text = localEffVolume.ToString("0%");
                effectSlider.value = localEffVolume;

                audioManager.SetVolume("OpenDoor", localEffVolume);
                audioManager.SetVolume("CloseDoor", localEffVolume);
                audioManager.SetVolume("Elevator", localEffVolume);
                audioManager.SetVolume("PressButton", localEffVolume);
                audioManager.SetVolume("Stare30", localEffVolume);
                audioManager.SetVolume("Stare60", localEffVolume);
                audioManager.SetVolume("Stare90", localEffVolume);
                audioManager.SetVolume("Chase", localEffVolume);
                audioManager.SetVolume("Scan", localEffVolume);
                audioManager.SetVolume("AutoDoor", localEffVolume);
                audioManager.SetVolume("Draw", localEffVolume);
            }
            else {
                menuMamager.ResetDefaults("Audio");
            }

            if (ES_Save.Exists("masterSen")) {
                float localSensitivity = ES_Save.Load<float>("masterSen");
                mouseSensText.text = localSensitivity.ToString("0.0");
                mouseSensSlider.value = localSensitivity;
                menuMamager.mouseSenitivity = Mathf.Round(localSensitivity);
            }

            if (ES_Save.Exists("masterInvertY-Axis")) {
                if (ES_Save.Load<int>("masterInvertY-Axis") == 1)
                    invertYAxisToggle.isOn = true;
                else
                    invertYAxisToggle.isOn = false;
            }

            if (ES_Save.Exists("masterLevel")) {
                ES_Save.Load<float>("masterLevel");
            }
        }
    }
}
