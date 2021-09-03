using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UI_QuestText : MonoBehaviour
{

    [Inject] private LuckGameplayBase _gameplay;
    [SerializeField] private Text _text;
    [SerializeField] private CanvasGroup _canvasGroup;

    private Sequence _currentSequence;
    private bool _hasQuest;

    private void OnEnable ()
    {
        _gameplay.QuestUpdated += OnQuestUpdated;
    }

    private void OnDisable()
    {
        _gameplay.QuestUpdated -= OnQuestUpdated;
    }

    private void OnQuestUpdated(string newQuest)
    {
        if (_hasQuest == false)
        {
            _text.text = newQuest;
            _hasQuest = true;
            return;
        }

        var currentPosition = _text.rectTransform.localPosition;
        var targetPosition = _text.rectTransform.localPosition + Vector3.up * 100f * GetComponentInParent<Canvas>().scaleFactor;

        _currentSequence?.Kill();
        _currentSequence = DOTween.Sequence();

        _currentSequence.Append(_canvasGroup.DOFade(0f, 0.2f));
        _currentSequence.AppendCallback(() => _text.text = newQuest);
        _currentSequence.AppendCallback(() => _text.rectTransform.localPosition = targetPosition);
        _currentSequence.Append(_canvasGroup.DOFade(1f, 0.2f));
        _currentSequence.Join(_text.rectTransform.DOLocalMove(currentPosition, 0.25f, true));
    }

}