using UnityEngine;
using UnityEngine.UI;

// This class exists because we don't want to setup all the references
// manually in each window's prefab.

[RequireComponent(typeof(RectTransform))]
public sealed class UI_WindowReferences : MonoBehaviour
{

    [SerializeField] private Button _closeButton = default;
    [SerializeField] private CanvasGroup _canvasGroup = default;
    [Header("Texts")]
    [SerializeField] private Text _titleText = default;
    [SerializeField] private Text _descriptionText = default;
    [Header("Animations")]
    [SerializeField] private GenericWindowAnimation _genericOpeningAnimation;
    [SerializeField] private GenericWindowAnimation _genericClosingAnimation;
    [Header("Background")]
    [SerializeField] private bool _overrideBackground;
    [SerializeField] private Color _backgroundColorOverride;

    public Button CloseButton => _closeButton;
    public CanvasGroup CanvasGroup => _canvasGroup;
    public Text TitleText => _titleText;
    public Text DescriptionText => _descriptionText;
    public RectTransform RectTransform { get; private set; }
    public GenericWindowAnimation GenericClosingAnimation => _genericClosingAnimation;
    public GenericWindowAnimation GenericOpeningAnimation => _genericOpeningAnimation;
    public Color? OverrideBackgroundColor
    {
        get
        {
            if (_overrideBackground)
                return _backgroundColorOverride;
            return null;
        }
    }

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }

}