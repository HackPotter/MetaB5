using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class BiologEntry
{
    [SerializeField]
    private string _name;
    [SerializeField]
    private string _descriptionText;
    [SerializeField]
    private string _scale;
    [SerializeField]
    private List<BiologEntryGalleryItem> _galleryItems;
    [SerializeField]
    private List<BiologEntryDetailSection> _detailSections;
    [SerializeField]
    private List<string> _tags;

    public string EntryName
    {
        get { return _name; }
        set { _name = value; }
    }

    public string Scale
    {
        get { return _scale; }
        set { _scale = value; }
    }

    public string DescriptionText
    {
        get { return _descriptionText; }
        set { _descriptionText = value; }
    }

    public List<BiologEntryGalleryItem> GalleryItems
    {
        get { return _galleryItems; }
    }

    public List<BiologEntryDetailSection> DetailSections
    {
        get { return _detailSections; }
    }

    public List<string> Tags
    {
        get { return _tags; }
    }

    public BiologEntry()
    {
        if (_name == null)
        {
            _name = "";
        }
        if (_scale == null)
        {
            _scale = "";
        }
        if (_descriptionText == null)
        {
            _descriptionText = "";
        }
        if (_galleryItems == null)
        {
            _galleryItems = new List<BiologEntryGalleryItem>();
        }
        if (_detailSections == null)
        {
            _detailSections = new List<BiologEntryDetailSection>();
        }
        if (_tags == null)
        {
            _tags = new List<string>();
        }
    }
}