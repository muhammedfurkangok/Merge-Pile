using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.Enums;
using Runtime.Extensions;
using Runtime.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Managers
{
    public class UIManager : SingletonMonoBehaviour<UIManager>
    {
        [Header("Panels")]
        [SerializeField] private GameObject startPanel;
        [SerializeField] private GameObject levelCompletePanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject levelFailPanel;
        [SerializeField] private GameObject moveCountPanel;
        
        [Header("Buttons")]
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private Button retryLevelButton;
        [SerializeField] private Button coinButton;
        [SerializeField] private Button cancelUtilityButton;
        [SerializeField] private Button bombButton;
        [SerializeField] private Button unlockButton;
        
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI utilityText;
        [SerializeField] private TextMeshProUGUI bombCountText;
        [SerializeField] private TextMeshProUGUI unlockCountText;
        
        [SerializeField] public TextMeshProUGUI timerText;
        [SerializeField] public TextMeshProUGUI moverText;
        
        [Header("Images")]
        [SerializeField] private Image levelWinEmoji;
        [SerializeField] private Image levelWinButton;
        [SerializeField] private Image levelFailEmoji;
        [SerializeField] private Image levelFailButton;
        
        
        [Header("Canvases")]
        [SerializeField]
        public GameObject utilityCanvas;
        
        
        
        [Header("Private's")]
        private bool isSettingsClicked = false;
        private Tween coinTween;
        private Vector3 moverTextScale;
        private Color moverTextColor;

      
        private void Start()
        {
            CheckRemoteConfig();
            AddListeners();
            SubscribeEvents();
            UpdateCoinText();
            UpdateBombCountText();
            UpdateUnlockCountText();
            moverTextScale = moverText.transform.localScale;
            moverTextColor = moverText.color;
           
            levelText.text = $"LVL {PlayerPrefs.GetInt(PlayerPrefsKeys.FakeLevelIndexInt).ToString()}";
        }

      

        private void CheckRemoteConfig()
        {
            if (!RemoteConfigDummy.hasTimer)
            {
                timerText.gameObject.SetActive(false);
            }
            else if (RemoteConfigDummy.hasTimer)
            {
                   timerText.gameObject.SetActive(true);
            }
            else if (!RemoteConfigDummy.hasMoveCounter)
            {
                moveCountPanel.SetActive(false);    
            }
            else if (!RemoteConfigDummy.hasMoveCounter)
            {
                moveCountPanel.SetActive(false);
            }
        }

        private void UpdateCoinText()
        {
            coinText.text = CurrencyManager.Instance.GetCoinAmount().ToString(); 
        }

        public void UpdateUnlockCountText()
        {
            unlockCountText.text = UtilityManager.Instance.unlockCount.ToString();
        }

        public void UpdateBombCountText()
        {
            bombCountText.text = UtilityManager.Instance.bombCount.ToString();
        }

        public void UpdateMoverText()
        {
            MoverManager._moveCount--;
            moverText.text = MoverManager._moveCount.ToString();
            
            moverText.DOKill(false);
            moverText.color = moverTextColor;

            if (MoverManager._moveCount <= 5 && MoverManager._moveCount > 0)
            {
                
                moverText.DOColor(Color.red, 0.3f).SetLoops(-1, LoopType.Yoyo);
            }
            
            moverText.transform.DOScale(Vector3.zero, 0.2f)
                .OnComplete(() => moverText.transform.DOScale(moverTextScale, 0.2f)); 
        }
        
        #region Buttons
        private void AddListeners()
        {
            settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            coinButton.onClick.AddListener(OnCoinButtonClicked);
            nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
            retryLevelButton.onClick.AddListener(OnRestartLevelButtonClicked);
            bombButton.onClick.AddListener(OnBombButtonClicked);
            unlockButton.onClick.AddListener(OnUnlockButtonClicked);
            cancelUtilityButton.onClick.AddListener(OnCancelUtilityButtonClicked);
        }

        private void OnCancelUtilityButtonClicked()
        {
            SoundManager.Instance.PlaySound(GameSoundType.ButtonClick);
            InputManager.Instance.SetUtilityInactive();
            UtilityManager.Instance.isAnyUtilityActive = false;
            utilityCanvas.gameObject.SetActive(false);
        }

        private void OnUnlockButtonClicked()
        {
            if(ItemManager.Instance.ActiveCubeCount() == 1) return;
            
            if (UtilityManager.Instance.unlockCount <= 0)
            {
                Debug.Log("No unlock left");
                return;
            }
            
            if(UtilityManager.Instance.isAnyUtilityActive) return;
            
            if(ItemManager.Instance.CheckAllItemsClickable())
            {
                Debug.Log("No item to unlock");
                return;
            }
            
         
            SoundManager.Instance.PlaySound(GameSoundType.ButtonClick);
            utilityText.text = "Select a Item to Unlock!";
            utilityCanvas.gameObject.SetActive(true);
            UtilityManager.Instance.UseUtility(UtilityType.Unlock);
        }

        private void OnBombButtonClicked()
        {
            if(ItemManager.Instance.ActiveCubeCount() == 1) return;
            
            if(UtilityManager.Instance.bombCount <= 0)
            {
                Debug.Log("No bomb left");
                return;
            }
            if(UtilityManager.Instance.isAnyUtilityActive) return;
            
            if(ItemManager.Instance.CheckAllItemsClickable())
            {
                Debug.Log("No item to bomb");
                return;
            }
            
           
            SoundManager.Instance.PlaySound(GameSoundType.ButtonClick);
            utilityText.text = "Ready to Bomb!";
            utilityCanvas.gameObject.SetActive(true);
            UtilityManager.Instance.UseUtility(UtilityType.Bomb);
        }

        private void OnCoinButtonClicked()
        {
            SoundManager.Instance.PlaySound(GameSoundType.ButtonClick);
            coinTween?.Kill();
            coinButton.transform.localScale = Vector3.one;
            coinTween = coinButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f, 10, 1);
            
        }

        private void OnSettingsButtonClicked()
        {
            isSettingsClicked = !isSettingsClicked;
            settingsButton.transform.DORotate( new Vector3(0,0, isSettingsClicked ? 180 : 0), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.OutBack);

            if (isSettingsClicked)
            {
                settingsPanel.SetActive(true);
                GameManager.Instance.SetGameStateSettingsScreen();
            }
            else
            {
                settingsPanel.SetActive(false);
                GameManager.Instance.SetGameStateGameplay();
            }
            SoundManager.Instance.PlaySound(GameSoundType.ButtonClick);
        }
        
        private async void OnNextLevelButtonClicked()
        {
            CurrencyManager.Instance.IncressCoinAmount();
            SoundManager.Instance.PlaySound(GameSoundType.ButtonClick);
            nextLevelButton.enabled = false;
            //anim
            await UniTask.WaitForSeconds(2);
            nextLevelButton.enabled = true;
            LevelManager.Instance.NextLevel();
        }
        
        private void OnRestartLevelButtonClicked()
        {
            SoundManager.Instance.PlaySound(GameSoundType.ButtonClick);
            LevelManager.Instance.RestartLevel();
        }
        
        private void RemoveListeners()
        {
            settingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
            coinButton.onClick.RemoveListener(OnCoinButtonClicked);
            nextLevelButton.onClick.RemoveListener(OnNextLevelButtonClicked);
            retryLevelButton.onClick.RemoveListener(OnRestartLevelButtonClicked);
            bombButton.onClick.RemoveListener(OnBombButtonClicked);
            unlockButton.onClick.RemoveListener(OnUnlockButtonClicked);
            cancelUtilityButton.onClick.RemoveListener(OnCancelUtilityButtonClicked);
        }
        #endregion
        #region Events
        private void SubscribeEvents()
        {
            GameManager.Instance.OnLevelSuccessful += OnLevelSuccessful;
            GameManager.Instance.OnLevelFailed += OnLevelFailed;
        }
       private void OnLevelSuccessful()
       {
        SoundManager.Instance.PlaySound(GameSoundType.LevelComplete);
        levelCompletePanel.SetActive(true);
        
        levelWinEmoji.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 3f, 5, 1).SetLoops(-1, LoopType.Yoyo);
        levelWinEmoji.transform.DORotate( new Vector3(0,0, 10), 1f).SetLoops(-1, LoopType.Yoyo);
        levelWinButton.transform.DOPunchPosition(new Vector3(0, 60f, 0), 4f, 5, 1);
        }

        
        private void OnLevelFailed()
        {
            //Disable All Buttons
            DisableUtilityButtons(); 
            
            
            SoundManager.Instance.PlaySound(GameSoundType.LevelFail);
            levelFailPanel.SetActive(true);
            levelFailEmoji.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 3f, 5, 1).SetLoops(-1, LoopType.Yoyo);
            levelFailEmoji.transform.DORotate( new Vector3(0,0, 10), 1f).SetLoops(-1, LoopType.Yoyo);
            levelFailButton.transform.DOPunchPosition(new Vector3(0, 60f, 0), 4f, 5, 1);
        }

        private void DisableUtilityButtons()
        {
            bombButton.interactable = false;
            unlockButton.interactable = false;
        }

        private void UnSubscribeEvents()
        {
            GameManager.Instance.OnLevelSuccessful -= OnLevelSuccessful;
            GameManager.Instance.OnLevelFailed -= OnLevelFailed;
        }

        #endregion
        private void OnDisable()
        {
            UnSubscribeEvents();
            RemoveListeners();
        }
    }
}