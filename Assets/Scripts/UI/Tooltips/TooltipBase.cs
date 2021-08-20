using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
public abstract class TooltipBase<TTarget> : MonoBehaviour where TTarget : struct
{

    public RectTransform Arrow => _arrow;
    [SerializeField] private RectTransform _arrow;

    public Canvas Canvas => GetComponentInParent<Canvas>();
    public RectTransform RectTransform => GetComponent<RectTransform>();

    private CanvasGroup _canvasGroup;
    private Sequence _currentSequence;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        _canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    public void Show(TTarget target)
    {
        Setup(target);
        gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(RectTransform);
        _currentSequence?.Kill();
        _currentSequence = CreateShowingSequence();
    }

    // Exp
    public void UpdateInformation(TTarget target)
    {
        Setup(target);
    }

    public void Close()
    {
        _currentSequence?.Kill();
        _currentSequence = CreateHiddingSequence();
    }

    protected abstract void Setup(TTarget target);

    protected virtual Sequence CreateShowingSequence()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(_canvasGroup.DOFade(1f, 0.25f).SetSpeedBased(true));
        sequence.SetUpdate(true);
        return sequence;
    }

    protected virtual Sequence CreateHiddingSequence()
    {
        var sequence = DOTween.Sequence();        
        sequence.Append(_canvasGroup.DOFade(0f, 0.25f).SetSpeedBased(true).OnComplete(() => gameObject.SetActive(false)));
        sequence.SetUpdate(true);
        return sequence;
    }

}