using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    public float MasterVolume { get; private set; }
    public float BGMVolume { get; private set; }
    public float SEVolume { get; private set; }
    public float MouseSensitivity { get; private set; }
    public float Brightness { get; private set; }

    private const string MasterVolumeKey = "MasterVolume";
    private const string BGMVolumeKey = "BGMVolume";
    private const string SEVolumeKey = "SEVolume";
    private const string MouseSensitivityKey = "MouseSensitivity";
    private const string BrightnessKey = "Brightness";

    private void Awake()
    {
        Instance = this;
        LoadSettings();
    }

    public void LoadSettings()
    {
        MasterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, 1.0f);
        BGMVolume = PlayerPrefs.GetFloat(BGMVolumeKey, 0.8f);
        SEVolume = PlayerPrefs.GetFloat(SEVolumeKey, 0.8f);
        MouseSensitivity = PlayerPrefs.GetFloat(MouseSensitivityKey, 120f);
        Brightness = PlayerPrefs.GetFloat(BrightnessKey, 1.0f);
    }

    public void SetMasterVolume(float value)
    {
        MasterVolume = value;
        PlayerPrefs.SetFloat(MasterVolumeKey, value);
        PlayerPrefs.Save();
    }

    public void SetBGMVolume(float value)
    {
        BGMVolume = value;
        PlayerPrefs.SetFloat(BGMVolumeKey, value);
        PlayerPrefs.Save();
    }

    public void SetSEVolume(float value)
    {
        SEVolume = value;
        PlayerPrefs.SetFloat(SEVolumeKey, value);
        PlayerPrefs.Save();
    }

    public void SetMouseSensitivity(float value)
    {
        MouseSensitivity = value;
        PlayerPrefs.SetFloat(MouseSensitivityKey, value);
        PlayerPrefs.Save();
    }

    public void SetBrightness(float value)
    {
        Brightness = value;
        PlayerPrefs.SetFloat(BrightnessKey, value);
        PlayerPrefs.Save();
    }
}