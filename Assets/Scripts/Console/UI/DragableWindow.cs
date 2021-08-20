using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragableWindow : MonoBehaviour, IDragHandler
{

    [SerializeField] private RectTransform parent = default;

    private Canvas canvas;

    public void OnDrag(PointerEventData eventData)
    {
        var targetPosition = (Vector2)parent.transform.position + eventData.delta;

        float parentWidth = parent.rect.width / 2f * canvas.scaleFactor;

        if (targetPosition.x - parentWidth < 0f)
            targetPosition.x = parentWidth;

        if (targetPosition.x + parentWidth > Screen.width)
            targetPosition.x = Screen.width - parentWidth;

        float parentHeight = parent.rect.height / 2f * canvas.scaleFactor;

        if (targetPosition.y - parentHeight < 0f)
            targetPosition.y = parentHeight;

        if (targetPosition.y + parentHeight > Screen.height)
            targetPosition.y = Screen.height - parentHeight;

        parent.position = targetPosition;
    }

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

}