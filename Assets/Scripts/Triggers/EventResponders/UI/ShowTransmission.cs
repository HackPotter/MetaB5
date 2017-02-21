using UnityEngine;

[Trigger(DisplayPath = "UI")]
public class ShowTransmission : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private string _sender;
    [SerializeField]
    private string _transmissionText;
    [SerializeField]
    private float _duration;
    [SerializeField]
    private Sprite _image;
#pragma warning restore 0067, 0649

    public string TransmissionText
    {
        get { return _transmissionText; }
    }

    public float Duration
    {
        get { return _duration; }
    }

    public override void OnEvent(ExecutionContext context)
    {
        MetablastUI.Instance.HudView.TransmissionView.ShowTransmission(_sender, _transmissionText, _image, _duration);
    }
}

