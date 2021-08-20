using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public abstract class BaseConsoleWindow<T> : MonoBehaviour, IPointerClickHandler where T : BaseConsoleWindow<T>
{

    [SerializeField] private Text TitleText = default;
    [Inject] protected ConsoleFilesManager FileManager = default;
    [Inject] private CursorManager _cursorManager;
    [Inject] private InputSystem _inputSystem;

    protected abstract string WindowDisplayName { get; }
    protected abstract string WindowId { get; }
    protected InputReciver InputReciver;
    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
        InputReciver = new InputReciver(true, WindowId);
    }

    protected virtual void Start()
    {
        TitleText.text = WindowDisplayName;

        InputReciver.BindInputActionPressed("pause", Close);
        _inputSystem.AddReciver(InputReciver);

        _cursorManager.Show(this);

        if (FileManager.DataContainer.TryGetData(WindowId, out WindowInfo info))
        {
            transform.position = new Vector2(info.PosX, info.PosY);
            GetComponent<RectTransform>().sizeDelta = new Vector2(info.SizeX, info.SizeY);
        }
    }

    private void OnDisable()
    {
        FileManager.DataContainer.SetData(WindowId, new WindowInfo()
        {
            PosX = transform.position.x,
            PosY = transform.position.y,
            SizeX = GetComponent<RectTransform>().sizeDelta.x,
            SizeY = GetComponent<RectTransform>().sizeDelta.y,
        });
    }

    protected virtual void OnDestroy()
    {
        _inputSystem.RemoveReciver(InputReciver);
        _cursorManager.Hide(this);
    }

    public void Close()
    {
        Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
        _inputSystem.RemoveReciver(InputReciver);
        _inputSystem.AddReciver(InputReciver);
    }

    [System.Serializable]
    public struct WindowInfo
    {
        public float PosX;
        public float PosY;
        public float SizeX;
        public float SizeY;
    }

    //public class FactoryTest : PlaceholderFactory<T> { }

}