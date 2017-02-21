using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class GalleryPreviewButtonPressed : UnityEvent<BiologEntryGalleryItem>
{
}

public class BiologGalleryPreviewListElement : MonoBehaviour
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private GalleryPreviewButtonPressed _buttonPressed;
    [SerializeField]
    private Button _button;
    [SerializeField]
    private Image _previewImage;
#pragma warning restore 0067, 0649

    private BiologEntryGalleryItem _entry;

    public BiologEntryGalleryItem GalleryItem
    {
        get { return _entry; }
        set
        {
            _entry = value;
            _previewImage.sprite = _entry.GalleryPreview;
        }
    }

    public GalleryPreviewButtonPressed ButtonPressed
    {
        get { return _buttonPressed; }
    }

    void OnEnable()
    {
        _button.onClick.AddListener(onClickHandler);
    }

    void OnDisable()
    {
        _button.onClick.RemoveListener(onClickHandler);
    }

    void onClickHandler()
    {
        _buttonPressed.Invoke(_entry);
    }
}

