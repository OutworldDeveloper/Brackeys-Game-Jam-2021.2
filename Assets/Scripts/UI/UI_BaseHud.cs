using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

// 2 types of Panels - Huds and Windows
// Huds should always be at the bottom
public abstract class UI_BaseHud : UI_BasePanel<UI_BaseHud>
{
    public override bool HideWindowsUnderneath => false;
    public override bool HideBackground => false;


}