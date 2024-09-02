using Cysharp.Threading.Tasks;
using Runtime.Enums;
using Runtime.Extensions;
using UnityEngine;
using DG.Tweening;

namespace Runtime.Managers
{
    public class TimerManager : SingletonMonoBehaviour<TimerManager>
    {
        [SerializeField] private int _time;
        [SerializeField] private bool _isTimerActive;

        private Color _originalColor;
        private Vector3 _originalScale;

        private void Start()
        {
            if (!RemoteConfigDummy.hasTimer)
            {
                return;
            }

            Init();
            UpdateTimerText();
            PassTimeForCountDown();
        }

        private void Init()
        {
            _isTimerActive = true;
            
            if (PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentLevelIndexInt) < 0 || PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentLevelIndexInt) > RemoteConfigDummy.timers.Count)
            {
                Debug.LogError("Invalid timer index. Using default timer value.");
                _time = RemoteConfigDummy.DefaultTimer;
            }
            else
            {
               _time = RemoteConfigDummy.timers[PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentLevelIndexInt) - 1];
            }
            
            _originalColor = UIManager.Instance.timerText.color;
            _originalScale = UIManager.Instance.timerText.transform.localScale;
        }

        private void UpdateTimerText()
        {
            int minutes = _time / 60;
            int seconds = _time % 60;
            UIManager.Instance.timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            UIManager.Instance.timerText.transform.DOKill(); 
            UIManager.Instance.timerText.transform.localScale = Vector3.zero; 
            UIManager.Instance.timerText.transform.DOScale(_originalScale, 0.2f); 

         
            if (_time <= 10)
            {
                UIManager.Instance.timerText.DOColor(Color.red, 0.5f).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                UIManager.Instance.timerText.DOKill(); 
                UIManager.Instance.timerText.color = _originalColor; 
            }
        }

        private async void PassTimeForCountDown()
        {
            while (_isTimerActive)
            {
                await UniTask.WaitForSeconds(1);
                if (GameManager.Instance.GameStates != GameStates.Gameplay) await UniTask.WaitUntil(() => GameManager.Instance.GameStates == GameStates.Gameplay);

                _time--;
                UpdateTimerText();

                if (_time != 0) continue;
                OnTimerEnd();
                break;
            }
        }

        private void OnTimerEnd()
        {
            GameManager.Instance.SetGameStateLevelFail();
        }

        private void OnDisable()
        {
            _isTimerActive = false;
        }
    }
}
