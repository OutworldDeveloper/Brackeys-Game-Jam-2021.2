using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class LuckHealthBar : MonoBehaviour
{

    [SerializeField] private Image _healthImage;
    [SerializeField] private Image _delayedHealthImage;
    [SerializeField] private Image _fullHealthIndicator;
    [SerializeField] private Setting_Options _healthBarSetting;
    [SerializeField] private Color _fullHealthColor;
    [SerializeField] private Color _emptyHealthColor;

    private PlayerPawn _playerPawn;

    private Camera _mainCamera;
    private CanvasGroup _canvasGroup;

    private Tween _currentTween;
    private Vector3 _defaultPosition;

    private bool _followLuck;

    [Inject]
    public void Initialize(PlayerPawn playerPawn)
    {
        _playerPawn = playerPawn;
        _canvasGroup = GetComponent<CanvasGroup>();
        _mainCamera = _playerPawn.PlayerController.PlayerCamera;
    }

    private void Start()
    {
        _defaultPosition = GetComponent<RectTransform>().anchoredPosition;
        _healthImage.fillAmount = _playerPawn.Luck.NormalizedHealth;
        _delayedHealthImage.fillAmount = _playerPawn.Luck.NormalizedHealth;
        SetupHealthBar();
    }

    private void OnEnable()
    {
        SettingsManager.OnSettingsChanged += SetupHealthBar;
        _playerPawn.Luck.HealthChanged += OnLuckHealthChanged;
    }

    private void OnDisable()
    {
        SettingsManager.OnSettingsChanged -= SetupHealthBar;
        _playerPawn.Luck.HealthChanged -= OnLuckHealthChanged;
    }

    private void SetupHealthBar()
    {
        switch (_healthBarSetting.GetValue())
        {
            case 0:
                _canvasGroup.alpha = 0f;
                break;
            case 1:
                _followLuck = true;
                GetComponent<RectTransform>().localScale = Vector2.one * 0.75f;
                _canvasGroup.alpha = 1f;
                break;
            case 2:
                _followLuck = false;
                GetComponent<RectTransform>().localScale = Vector2.one;
                GetComponent<RectTransform>().anchoredPosition = _defaultPosition;
                _canvasGroup.alpha = 1f;
                break;
        }
    }

    private void OnLuckHealthChanged(int health, int healthBefore)
    {
        _currentTween?.Kill();
        _healthImage.fillAmount = _playerPawn.Luck.NormalizedHealth;
        if (health < healthBefore)
        {
            _currentTween = _delayedHealthImage.DOFillAmount(_playerPawn.Luck.NormalizedHealth, 0.5f);
        }
        else
        {
            _delayedHealthImage.fillAmount = _playerPawn.Luck.NormalizedHealth;
        }

        var isFullHealth = _playerPawn.Luck.NormalizedHealth >= 1;
        _fullHealthIndicator.color = isFullHealth ? _fullHealthColor : _emptyHealthColor;
    }

    private void LateUpdate()
    {
        if (_followLuck)
        {
            transform.position = _mainCamera.WorldToScreenPoint(_playerPawn.Luck.transform.position + Vector3.up * 2.5f);
        }
    }

}