using UnityEngine;
using UnityEngine.UI;

public class QuestionSetUIElement : MonoBehaviour
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private RawImage _previewImage;

    [SerializeField]
    private Text _titleLabel;

    [SerializeField]
    private Text _descriptionLabel;

    [SerializeField]
    private Toggle _toggle;
#pragma warning restore 0067, 0649

    public Texture PreviewTexture
    {
        get { return _previewImage.texture; }
        set { _previewImage.texture = value; }
    }

    public string TitleText
    {
        get { return _titleLabel.text; }
        set { _titleLabel.text = value; }
    }

    public string Description
    {
        get { return _descriptionLabel.text; }
        set { _descriptionLabel.text = value; }
    }

    public Toggle Toggle
    {
        get { return _toggle; }
    }

    public QuestionSet QuestionSet
    {
        get;
        set;
    }
}

