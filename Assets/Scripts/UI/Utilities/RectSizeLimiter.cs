using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteInEditMode]
public class RectSizeLimiter : UIBehaviour, ILayoutSelfController
{

    public RectTransform rectTransform;

    [SerializeField]
    protected Vector2 m_maxSize = Vector2.zero;

    public Vector2 maxSize
    {
        get { return m_maxSize; }
        set
        {
            if (m_maxSize != value)
            {
                m_maxSize = value;
                SetDirty();
            }
        }
    }

    private DrivenRectTransformTracker m_Tracker;

    protected override void OnEnable()
    {
        base.OnEnable();
        SetDirty();
    }

    protected override void OnDisable()
    {
        m_Tracker.Clear();
        LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        base.OnDisable();
    }

    protected void SetDirty()
    {
        if (!IsActive())
            return;

        LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
    }

    public void SetLayoutHorizontal()
    {
        if (m_maxSize.x > 0f && rectTransform.rect.width > m_maxSize.x)
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxSize.x);
            m_Tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaX);
        }
    }

    public void SetLayoutVertical()
    {
        if (m_maxSize.y > 0f && rectTransform.rect.height > m_maxSize.y)
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxSize.y);
            m_Tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaY);
        }
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        SetDirty();
    }
#endif

}