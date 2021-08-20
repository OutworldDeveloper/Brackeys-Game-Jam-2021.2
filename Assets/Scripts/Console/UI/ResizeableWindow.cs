using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResizeableWindow : MonoBehaviour, IDragHandler, IBeginDragHandler
{

    [SerializeField] private RectTransform parent = default;
    [SerializeField] private float minimalSizeX = 20f;
    [SerializeField] private float minimalSizeY = 150f;
    [SerializeField] private float maxSizeX = 40f;
    [SerializeField] private float maxSizeY = 200f;
    private bool ignoreMaximumSize = true;

    private Canvas canvas;

    public void OnBeginDrag(PointerEventData eventData)
    {
        //parent.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        float minX = parent.anchoredPosition.x + minimalSizeX / 2f;
        float minY = parent.anchoredPosition.y + minimalSizeY / 2f;

        float maxMoveX = Screen.width / 2f / canvas.scaleFactor;
        float maxMoveY = Screen.height / 2f / canvas.scaleFactor;

        float maxX = ignoreMaximumSize ? maxMoveX : Mathf.Min(parent.anchoredPosition.x + maxSizeX / 2f, maxMoveX);
        float maxY = ignoreMaximumSize ? maxMoveY : Mathf.Min(parent.anchoredPosition.y + maxSizeY / 2f, maxMoveY);

        var desiredOffset = parent.offsetMax + eventData.delta / canvas.scaleFactor;

        float finalX = Mathf.Min(Mathf.Max(desiredOffset.x, minX), maxX);
        float finalY = Mathf.Min(Mathf.Max(desiredOffset.y, minY), maxY);

        parent.offsetMax = new Vector2(finalX, finalY);
    }

    public void SetMinimalSize(float x, float y)
    {
        minimalSizeX = x;
        minimalSizeY = y;
    }

    public void SetMaximumSize(float x, float y)
    {
        maxSizeX = x;
        maxSizeY = y;
        ignoreMaximumSize = false;
    }

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

}