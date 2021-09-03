using UnityEngine;
using Zenject;
using DG.Tweening;

//[RequireComponent(typeof(GameObjectContext))]
[RequireComponent(typeof(CanvasGroup))]
public class UI_BaseHud : MonoBehaviour
{

    public readonly InputReciver InputReciver = new InputReciver(false); 
    protected CanvasGroup CanvasGroup { get; private set; }
    private Sequence _currentSequence;

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        OnHudStarted();
        var sequence = CreateShowingSequence();
        if (sequence != null)
        {
            _currentSequence = sequence.SetUpdate(true);
        }
    }

    public void HideThenDestroy()
    {
        OnHudHidden();
        _currentSequence?.Kill();
        var sequence = CreateShowingSequence();
        if (sequence != null)
        {
            _currentSequence = sequence.
                OnComplete(() => Destroy(gameObject)).
                SetUpdate(true);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnHudStarted() { }
    protected virtual void OnHudHidden() { }
    protected virtual Sequence CreateShowingSequence() => null;
    protected virtual Sequence CreateHiddingSequence() => null;

}
