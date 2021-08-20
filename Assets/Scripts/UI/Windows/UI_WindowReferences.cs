using UnityEngine;
using UnityEngine.UI;
// This class exists because we don't want to setup all this references
// manually in each window's prefab.
public sealed class UI_WindowReferences : MonoBehaviour
{

    [SerializeField] private Button _closeButton = default;
    public Button CloseButton => _closeButton;

    [SerializeField] private CanvasGroup _canvasGroup = default;
    public CanvasGroup CanvasGroup => _canvasGroup;

    [SerializeField] private Text _titleText = default;
    public Text TitleText => _titleText;

    public RectTransform RectTransform { get; private set; }

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }

}
