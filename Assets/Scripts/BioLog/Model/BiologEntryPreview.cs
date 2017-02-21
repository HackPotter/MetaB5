using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class BiologEntryPreview
{
    [SerializeField]
    private Sprite _previewImage;
    [SerializeField]
    private string _previewImageName;

    public Sprite PreviewImage
    {
        get { return _previewImage; }
        set { _previewImage = value; }
    }

    public string PreviewImageName
    {
        get { return _previewImageName; }
        set { _previewImageName = value; }
    }

    public BiologEntryPreview()
    {
        if (_previewImageName == null)
        {
            _previewImageName = "";
        }
    }
}
