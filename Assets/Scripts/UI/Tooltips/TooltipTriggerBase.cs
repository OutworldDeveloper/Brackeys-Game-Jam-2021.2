using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(RectTransform))]
public class TooltipTriggerBase<TTooltip, TTarget> : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    where TTooltip : TooltipBase<TTarget>
    where TTarget : struct
{

    private const float SAFEZONE = 25f;

    [Inject] protected TTooltip Tooltip { get; private set; }
    [SerializeField] private Orientation _orientation;
    [SerializeField] private Direction _preferableDirection;

    public TTarget Target;
    protected bool IsFocused { get; private set; }

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnDisable()
    {
        if (IsFocused)
        {
            Tooltip.Close();
            IsFocused = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsFocused = true;
        Tooltip.Show(Target);
        CalculatePosition();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsFocused = false;
        Tooltip.Close();
    }

    private void CalculatePosition()
    {
        float scaleFactor = Tooltip.Canvas.scaleFactor;

        float safeVerticalMin = SAFEZONE * scaleFactor;
        float safeVerticalMax =  Screen.height - SAFEZONE * scaleFactor;
        float safeHorizontalMin = SAFEZONE * scaleFactor;
        float safeHorizontalMax = Screen.width - SAFEZONE * scaleFactor;

        float virtualPositionY = transform.position.y;
        float virtualPositionX = transform.position.x;

        float arrowSize = 20f;
        int arrowPosition = 0;

        if (_orientation == Orientation.Vertical)
        {
            virtualPositionY = CalculateVerticalPosition(_preferableDirection, arrowSize, scaleFactor);

            if (_preferableDirection == Direction.Positive)
            {
                arrowPosition = 1;
                float f = _rectTransform.position.y + (_rectTransform.rect.yMax + Tooltip.RectTransform.rect.height + arrowSize) * scaleFactor;
                if (f > safeVerticalMax)
                {
                    virtualPositionY = CalculateVerticalPosition(Direction.Negative, arrowSize, scaleFactor);
                    arrowPosition = 3;
                }
            }
            else
            {
                arrowPosition = 3;
                float f = _rectTransform.position.y + (_rectTransform.rect.yMin - Tooltip.RectTransform.rect.height - arrowSize) * scaleFactor;
                if (f < safeVerticalMin)
                {
                    virtualPositionY = CalculateVerticalPosition(Direction.Positive, arrowSize, scaleFactor);
                    arrowPosition = 1;
                }
            }

            float xMin = transform.position.x + Tooltip.RectTransform.rect.xMin * scaleFactor;
            float xMax = transform.position.x + Tooltip.RectTransform.rect.xMax * scaleFactor;
            if (xMin < safeHorizontalMin)
            {
               virtualPositionX += safeHorizontalMin - xMin;
            }
            else if (xMax > safeHorizontalMax)
            {
                virtualPositionX -= xMax - safeHorizontalMax;
            }          
        }
        else
        {
            virtualPositionX = CalculateHorizontalPosition(_preferableDirection, arrowSize, scaleFactor);

            if (_preferableDirection == Direction.Positive)
            {
                arrowPosition = 2;
                float f = _rectTransform.position.x + (_rectTransform.rect.xMax + Tooltip.RectTransform.rect.width + arrowSize) * scaleFactor;
                if (f > safeHorizontalMax)
                {
                    arrowPosition = 4;
                    virtualPositionX = CalculateHorizontalPosition(Direction.Negative, arrowSize, scaleFactor);
                }
            }
            else
            {
                arrowPosition = 4;
                float f = _rectTransform.position.x + (_rectTransform.rect.xMin - Tooltip.RectTransform.rect.width - arrowSize) * scaleFactor;
                if (f < safeHorizontalMin)
                {
                    arrowPosition = 2;
                    virtualPositionX = CalculateHorizontalPosition(Direction.Positive, arrowSize, scaleFactor);
                }
            }

            float yMin = transform.position.y + Tooltip.RectTransform.rect.yMin * scaleFactor;
            float yMax = transform.position.y + Tooltip.RectTransform.rect.yMax * scaleFactor;
            if (yMin < safeVerticalMin)
            {
                virtualPositionY += safeVerticalMin - yMin;
            }
            else if (yMax > safeVerticalMax)
            {
                virtualPositionY -= yMax - safeVerticalMax;
            }
        }

        Tooltip.RectTransform.position = new Vector2(virtualPositionX, virtualPositionY);

        float arrowX = 0f;
        float arrowY = 0f;
        float arrowR = 0f;
        switch (arrowPosition)
        {
            case 1:
                {
                    arrowX = _rectTransform.position.x;
                    arrowY = _rectTransform.position.y + (_rectTransform.rect.yMax + arrowSize / 2) * scaleFactor;
                    arrowR = 0f;
                    break;
                }
            case 2:
                {
                    arrowX = _rectTransform.position.x + (_rectTransform.rect.xMax + arrowSize / 2) * scaleFactor;
                    arrowY = _rectTransform.position.y;
                    arrowR = -90f;
                    break;
                }
            case 3:
                {
                    arrowX = _rectTransform.position.x;
                    arrowY = _rectTransform.position.y + (_rectTransform.rect.yMin - arrowSize / 2) * scaleFactor;
                    arrowR = 180f;
                    break;
                }
            case 4:
                {
                    arrowX = _rectTransform.position.x + (_rectTransform.rect.xMin - arrowSize / 2) * scaleFactor;
                    arrowY = _rectTransform.position.y;
                    arrowR = 90f;
                    break;
                }
        }

        Tooltip.Arrow.position = new Vector2(arrowX, arrowY);
        Tooltip.Arrow.rotation = Quaternion.Euler(0f, 0f, arrowR);
    }

    private float CalculateVerticalPosition(Direction direction, float offset, float scaleFactor)
    {
        if (direction == Direction.Positive)
        {
            return _rectTransform.position.y +
                (_rectTransform.rect.yMax + Tooltip.RectTransform.rect.yMax + offset) * scaleFactor;
        }
        else
        {
            return _rectTransform.position.y +
                (_rectTransform.rect.yMin - Tooltip.RectTransform.rect.yMax - offset) * scaleFactor;
        }
    }

    private float CalculateHorizontalPosition(Direction direction, float offset, float scaleFactor)
    {
        if (direction == Direction.Positive)
        {
            return _rectTransform.position.x +
                (_rectTransform.rect.xMax + Tooltip.RectTransform.rect.xMax + offset) * scaleFactor;
        }
        else
        {
            return _rectTransform.position.x +
                (_rectTransform.rect.xMin - Tooltip.RectTransform.rect.xMax - offset) * scaleFactor;
        }
    }

    private enum Orientation
    {
        Vertical,
        Horizontal,
    }

    private enum Direction
    {
        Positive = 1,
        Negative = -1,
    }

}