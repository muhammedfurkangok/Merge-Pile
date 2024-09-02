using System;
using DG.Tweening;
using Runtime.Data.UnityObject;
using Runtime.Extensions;
using UnityEngine;

namespace Runtime.Managers
{
    public class MoverManager : SingletonMonoBehaviour<MoverManager>
    {
        
         public static int _moveCount;
        
        private void Start()
        {
            if (!RemoteConfigDummy.hasMoveCounter)
            {
                return;
            }

            Init();
        }

        private void Init()
        {
            if (PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentLevelIndexInt) < 0 || PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentLevelIndexInt) > RemoteConfigDummy.moves.Count)
            {
                Debug.LogError("Invalid timer index. Using default timer value.");
                _moveCount = RemoteConfigDummy.defaultMoveCounter;
            }
            else
            {
                _moveCount = RemoteConfigDummy.moves[PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentLevelIndexInt) - 1];
            }
            
            UIManager.Instance.moverText.text = _moveCount.ToString();
        }

        public void OnInputTaken()
        {
            if (!RemoteConfigDummy.hasMoveCounter)
            {
                return;
            }
            
            UIManager.Instance.UpdateMoverText();
            
            if (_moveCount <= 0)
            {
                OnMoveEnd();
            }
        }

        private void OnMoveEnd()
        {
            GameManager.Instance.SetGameStateLevelFail();
        }
        
    }
}