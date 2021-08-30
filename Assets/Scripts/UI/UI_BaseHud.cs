using UnityEngine;
using Zenject;
using DG.Tweening;

//[RequireComponent(typeof(GameObjectContext))]
[RequireComponent(typeof(CanvasGroup))]
public class UI_BaseHud : MonoBehaviour
{

    public readonly InputReciver InputReciver = new InputReciver(false); 
    private CanvasGroup _canvasGroup;
    private Sequence _currentSequence;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        _currentSequence = CreateShowingSequence();
        _currentSequence.SetUpdate(true);
        OnHudStarted();
    }

    public void HideThenDestroy()
    {
        OnHudHidden();
        _currentSequence?.Kill();
        _currentSequence = CreateHiddingSequence().
            OnComplete(() => Destroy(gameObject));
        _currentSequence.SetUpdate(true);
    }

    protected virtual void OnHudStarted() { }
    protected virtual void OnHudHidden() { }

    protected virtual Sequence CreateShowingSequence()
    {
        var sequence = DOTween.Sequence();

        var scaleTween = _canvasGroup.transform.DOScale(Vector3.one, 0.35f).From(Vector3.zero);
        var alphaTween = _canvasGroup.DOFade(1f, 0.6f).From(0f);

        sequence.Append(scaleTween);
        sequence.Join(alphaTween);

        return sequence;
    }

    protected virtual Sequence CreateHiddingSequence()
    {
        var sequence = DOTween.Sequence();

        var scaleTween = _canvasGroup.transform.DOScale(Vector3.one * 2, 0.35f);
        var alphaTween = _canvasGroup.DOFade(0f, 0.6f);

        sequence.Append(scaleTween);
        sequence.Join(alphaTween);

        return sequence;
    }

}
