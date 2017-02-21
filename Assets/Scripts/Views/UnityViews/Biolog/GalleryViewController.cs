using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryViewController : MonoBehaviour
{
    private BiologEntry _activeEntry;

#pragma warning disable 0067, 0649
    [SerializeField]
    private Image _imageViewer;
    [SerializeField]
    private BiologGalleryPreviewListElement _galleryPreviewPrefab;
    [SerializeField]
    private RectTransform _galleryItemGroup;
#pragma warning restore 0067, 0649

    private List<BiologGalleryPreviewListElement> _currentEntries = new List<BiologGalleryPreviewListElement>();

    public void SetEntry(BiologEntry entry)
    {
        _activeEntry = entry;

        foreach (BiologGalleryPreviewListElement element in _currentEntries)
        {
            GameObject.Destroy(element.gameObject);
        }
        _currentEntries.Clear();

        foreach (BiologEntryGalleryItem galleryItem in _activeEntry.GalleryItems)
        {
            BiologGalleryPreviewListElement galleryPreviewItem = GameObject.Instantiate(_galleryPreviewPrefab, _galleryItemGroup.transform.position, Quaternion.identity) as BiologGalleryPreviewListElement;

            _currentEntries.Add(galleryPreviewItem);
            galleryPreviewItem.transform.SetParent(_galleryItemGroup.transform);
            //galleryPreviewItem.transform.parent = _galleryItemGroup.transform;
            //galleryPreviewItem.transform.localScale = Vector3.one;
            galleryPreviewItem.GalleryItem = galleryItem;
            galleryPreviewItem.ButtonPressed.AddListener(GalleryPreviewImageClicked);
        }

        _imageViewer.sprite = _activeEntry.GalleryItems[0].GalleryImage;
    }

    public void SetEntry(BiologEntryListElement biologEntryListElement)
    {
        SetEntry(biologEntryListElement.Entry);
    }

    void GalleryPreviewImageClicked(BiologEntryGalleryItem galleryItem)
    {
        _imageViewer.sprite = galleryItem.GalleryImage;
    }
}
