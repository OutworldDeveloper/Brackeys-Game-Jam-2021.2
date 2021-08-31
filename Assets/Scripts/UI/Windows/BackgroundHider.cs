using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(CanvasGroup))]
public class BackgroundHider : MonoBehaviour
{

    private Image _image;
    private CanvasGroup _canvasGroup;
    private Color _defaultColor;
    private float _speed;
    private Sequence _currentSequence;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Init(Color defaultColor, float speed)
    {
        _defaultColor = defaultColor;
        _speed = speed;
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _image.color = _defaultColor;
    }

    public void StartHidding(Color color)
    {
        _currentSequence?.Kill();
        _currentSequence = DOTween.Sequence();
        _currentSequence.Insert(0f, _image.DOColor(color, _speed).SetSpeedBased());
        _currentSequence.Insert(0f, _canvasGroup.DOFade(1f, _speed).SetSpeedBased());
        _currentSequence.SetUpdate(true);
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
    }

    public void StopHidding()
    {
        _currentSequence?.Kill();
        _currentSequence = DOTween.Sequence();
        _currentSequence.Insert(0f, _image.DOColor(_defaultColor, _speed).SetSpeedBased());
        _currentSequence.Insert(0f, _canvasGroup.DOFade(0f, _speed).SetSpeedBased());
        _currentSequence.SetSpeedBased().SetUpdate(true);
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }

}