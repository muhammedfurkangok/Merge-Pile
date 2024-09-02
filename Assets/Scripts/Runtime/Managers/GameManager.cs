using System;
using Runtime.Enums;
using Runtime.Extensions;
using Runtime.Signals;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Managers
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
         public GameStates GameStates { get; private set;}
         public event Action OnLevelSuccessful = delegate { };
         public event Action OnLevelFailed = delegate { };
         
         
        #region Shortcuts
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SetGameStateLevelFail();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                SetGameStateLevelComplete();
            }
        }
        #endregion
        
        public void SetGameStateGameplay()
        {
            GameStates = GameStates.Gameplay;
        }
        public void SetGameStateSettingsScreen()
        {
            GameStates = GameStates.SettingsScreen;
        }
        
        public void SetGameStateLevelComplete()
        {
            GameStates = GameStates.LevelComplete;
            OnLevelSuccessful?.Invoke();
        }
        
        public void SetGameStateLevelFail()
        {
            GameStates = GameStates.LevelFail;
            OnLevelFailed?.Invoke();
        }

        public int GetLevelCoinAmount()
        {
            return 100 * PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentLevelIndexInt);
        }
    }
}