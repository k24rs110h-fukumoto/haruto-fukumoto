using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Slider brightnessSlider;

    private void Start()
    {
        if (SettingsManager.Instance == null)
        {
            return;
        }

        masterSlider.value = SettingsManager.Instance.MasterVolume;
        bgmSlider.value = SettingsManager.Instance.BGMVolume;
        seSlider.value = SettingsManager.Instance.SEVolume;
        sensitivitySlider.value = SettingsManager.Instance.MouseSensitivity;
        brightnessSlider.value = SettingsManager.Instance.Brightness;
    }

    public void OnMasterVolumeChanged(float value)
    {
        SettingsManager.Instance.SetMasterVolume(value);
    }

    public void OnBGMVolumeChanged(float value)
    {
        SettingsManager.Instance.SetBGMVolume(value);
    }

    public void OnSEVolumeChanged(float value)
    {
        SettingsManager.Instance.SetSEVolume(value);
    }

    public void OnSensitivityChanged(float value)
    {
        SettingsManager.Instance.SetMouseSensitivity(value);
    }

    public void OnBrightnessChanged(float value)
    {
        SettingsManager.Instance.SetBrightness(value);
    }
}