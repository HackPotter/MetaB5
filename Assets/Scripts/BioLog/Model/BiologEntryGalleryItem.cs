using System;
using UnityEngine;

[Serializable]
public class BiologEntryGalleryItem
{
    [SerializeField]
    private Sprite _galleryImage;

    [SerializeField]
    private Sprite _galleryPreview;

    [SerializeField]
    private string _caption;

    [SerializeField]
    private bool _is3D;

    [SerializeField]
    private GameObject _3dScene;

    public bool Is3D
    {
        get { return _is3D; }
        set { _is3D = value; }
    }

    public GameObject Scene3D
    {
        get { return _3dScene; }
        set { _3dScene = value; }
    }

    public Sprite GalleryImage
    {
        get { return _galleryImage; }
        set { _galleryImage = value; }
    }

    public Sprite GalleryPreview
    {
        get { return _galleryPreview; }
        set { _galleryPreview = value; }
    }

    public string Caption
    {
        get { return _caption; }
        set { _caption = value; }
    }

    public BiologEntryGalleryItem()
    {
        if (_caption == null)
        {
            _caption = "";
        }
    }
}
