using Runtime.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Managers
{
    public class SettingsManager : SingletonMonoBehaviour<SettingsManager>
    {
        public bool isSoundActive = true;
        public bool isVibrationActive = true;

        [SerializeField] private Button soundButton;
        [SerializeField] private Button vibrationButton;

        private void Start()
        {
            isSoundActive = PlayerPrefs.GetInt(PlayerPrefsKeys.IsSoundActiveInt, 1) == 1;
            isVibrationActive = PlayerPrefs.GetInt(PlayerPrefsKeys.IsVibrationActiveInt, 1) == 1;

            UpdateSoundButton();
            UpdateVibrationButton();
            
        }

        public void ToggleSound()
        {
            isSoundActive = !isSoundActive;
            PlayerPrefs.SetInt(PlayerPrefsKeys.IsSoundActiveInt, isSoundActive ? 1 : 0);

            if (isSoundActive)
            {
                Debug.Log("Sound enabled.");
            }
            else
            {
                Debug.Log("Sound disabled.");
            }
            UpdateSoundButton();
        }

        public void ToggleVibration()
        {
            isVibrationActive = !isVibrationActive;
            PlayerPrefs.SetInt(PlayerPrefsKeys.IsVibrationActiveInt, isVibrationActive ? 1 : 0);

            if (isVibrationActive)
            {
                // Enable vibration
                Debug.Log("Vibration enabled.");
            }
            else
            {
                // Disable vibration
                Debug.Log("Vibration disabled.");
            }

            UpdateVibrationButton();
        }

        private void UpdateSoundButton()
        {
            soundButton.GetComponentInChildren<TextMeshProUGUI>().text = isSoundActive ? "On" : "Off";
        }

        private void UpdateVibrationButton()
        {
            vibrationButton.GetComponentInChildren<TextMeshProUGUI>().text = isVibrationActive ? "On" : "Off";
        }
    }
    
}
