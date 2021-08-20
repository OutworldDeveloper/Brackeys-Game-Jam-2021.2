using UnityEngine;
using UnityEngine.UI;

public class TextTooltip : TooltipBase<TextTooltip.Info>
{

    [SerializeField] private Text _title;
    [SerializeField] private Text _text;

    protected override void Setup(Info target)
    {
        _title.text = target.Title;
        _text.text = target.Text;
    }

    [System.Serializable]
    public struct Info
    {
        public string Title;
        [TextArea(4, 6)]
        public string Text;

    }

}
