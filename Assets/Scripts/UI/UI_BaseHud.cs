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
        _canvasGroup.alpha = 0f;
        _canvasGroup.transform.localScale = Vector3.one * 0.5f;

        var newSequence = DOTween.Sequence();

        var scaleTween = _canvasGroup.transform.DOScale(Vector3.one, 0.35f);
        var alphaTween = _canvasGroup.DOFade(1f, 0.6f);

        newSequence.Insert(0, scaleTween);
        newSequence.Insert(0, alphaTween);

        return newSequence;
    }

    protected virtual Sequence CreateHiddingSequence()
    {
        var newSequence = DOTween.Sequence();

        var scaleTween = _canvasGroup.transform.DOScale(Vector3.one * 2, 0.35f);
        var alphaTween = _canvasGroup.DOFade(0f, 0.6f);

        newSequence.Insert(0, scaleTween);
        newSequence.Insert(0, alphaTween);

        return newSequence;
    }

}
